using System.Timers;
using DynamicTimer.Scheduling.Models;
using Microsoft.Extensions.Logging;

namespace DynamicTimer.Scheduling.Scheduling;

/// <summary>
/// Cron-based job scheduler that manages job execution based on cron expressions.
/// Note: This class is public for DI registration but should only be accessed through CronSchedulerHostedService.
/// </summary>
public class CronScheduler
{
    private readonly List<ScheduledJob> _jobs;
    private readonly System.Timers.Timer _timer;
    private readonly object _lock = new object();
    private readonly ILogger<CronScheduler>? _logger;

    public bool IsRunning { get; private set; }

    public event EventHandler<JobExecutedEventArgs>? JobExecuted;

    public CronScheduler(ILogger<CronScheduler>? logger = null)
    {
        _jobs = new List<ScheduledJob>();
        _timer = new System.Timers.Timer(60000); // 60 seconds
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _logger = logger;
    }

    public void AddJob(ScheduledJob job)
    {
        lock (_lock)
        {
            // Validate cron expression
            if (!CronExpression.TryParse(job.CronExpression, out var cronExpr))
            {
                throw new ArgumentException($"Invalid cron expression: {job.CronExpression}");
            }

            // Calculate next run time
            job.NextRun = cronExpr!.GetNextOccurrence(DateTime.Now);

            _jobs.Add(job);
            _logger?.LogInformation("Job added: {JobName} ({JobId}) with cron: {CronExpression}",
                job.Name, job.Id, job.CronExpression);
        }
    }

    public bool RemoveJob(string jobId)
    {
        lock (_lock)
        {
            var job = _jobs.FirstOrDefault(j => j.Id == jobId);
            if (job != null)
            {
                var removed = _jobs.Remove(job);
                if (removed)
                {
                    _logger?.LogInformation("Job removed: {JobName} ({JobId})", job.Name, job.Id);
                }
                return removed;
            }
            return false;
        }
    }

    public bool UpdateJob(string jobId, string? name = null, string? cronExpression = null, bool? isEnabled = null)
    {
        lock (_lock)
        {
            var job = _jobs.FirstOrDefault(j => j.Id == jobId);
            if (job == null)
                return false;

            if (name != null)
                job.Name = name;

            if (cronExpression != null)
            {
                if (!CronExpression.TryParse(cronExpression, out var cronExpr))
                {
                    throw new ArgumentException($"Invalid cron expression: {cronExpression}");
                }
                job.CronExpression = cronExpression;
                job.NextRun = cronExpr!.GetNextOccurrence(DateTime.Now);
            }

            if (isEnabled.HasValue)
                job.IsEnabled = isEnabled.Value;

            _logger?.LogInformation("Job updated: {JobName} ({JobId})", job.Name, job.Id);
            return true;
        }
    }

    public List<ScheduledJob> GetJobs()
    {
        lock (_lock)
        {
            return new List<ScheduledJob>(_jobs);
        }
    }

    public ScheduledJob? GetJobById(string jobId)
    {
        lock (_lock)
        {
            return _jobs.FirstOrDefault(j => j.Id == jobId);
        }
    }

    public void Start()
    {
        if (!IsRunning)
        {
            IsRunning = true;
            _timer.Start();
            _logger?.LogInformation("Scheduler started");

            // Immediately check for jobs that should run
            CheckAndExecuteJobs();
        }
    }

    public void Stop()
    {
        if (IsRunning)
        {
            IsRunning = false;
            _timer.Stop();
            _logger?.LogInformation("Scheduler stopped");
        }
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        CheckAndExecuteJobs();
    }

    private void CheckAndExecuteJobs()
    {
        var now = DateTime.Now;
        var currentMinute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

        List<ScheduledJob> jobsToExecute;

        lock (_lock)
        {
            // Find jobs that should execute
            jobsToExecute = _jobs
                .Where(j => j.IsEnabled && j.NextRun.HasValue && j.NextRun.Value <= currentMinute)
                .ToList();
        }

        // Execute jobs outside the lock to prevent blocking
        foreach (var job in jobsToExecute)
        {
            try
            {
                var executedAt = DateTime.Now;
                _logger?.LogInformation("Executing job: {JobName} ({JobId})", job.Name, job.Id);

                // Update job status
                lock (_lock)
                {
                    job.LastRun = executedAt;

                    // Calculate next run time
                    if (CronExpression.TryParse(job.CronExpression, out var cronExpr))
                    {
                        job.NextRun = cronExpr!.GetNextOccurrence(executedAt);
                    }
                }

                // Raise event
                JobExecuted?.Invoke(this, new JobExecutedEventArgs(job, executedAt));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error executing job: {JobName} ({JobId})", job.Name, job.Id);
                // Log error but continue with other jobs
                JobExecuted?.Invoke(this, new JobExecutedEventArgs(job, DateTime.Now, ex));
            }
        }
    }
}


