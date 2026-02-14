using System.Timers;
using DynamicTimer.Models;

namespace DynamicTimer.Scheduling;

public class CronScheduler
{
    private readonly List<ScheduledJob> _jobs;
    private readonly System.Timers.Timer _timer;
    private readonly object _lock = new object();

    public bool IsRunning { get; private set; }

    public event EventHandler<JobExecutedEventArgs>? JobExecuted;

    public CronScheduler()
    {
        _jobs = new List<ScheduledJob>();
        _timer = new System.Timers.Timer(60000); // 60 seconds
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
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
        }
    }

    public bool RemoveJob(string jobId)
    {
        lock (_lock)
        {
            var job = _jobs.FirstOrDefault(j => j.Id == jobId);
            if (job != null)
            {
                return _jobs.Remove(job);
            }
            return false;
        }
    }

    public List<ScheduledJob> GetJobs()
    {
        lock (_lock)
        {
            return new List<ScheduledJob>(_jobs);
        }
    }

    public void Start()
    {
        if (!IsRunning)
        {
            IsRunning = true;
            _timer.Start();

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
                // Log error but continue with other jobs
                JobExecuted?.Invoke(this, new JobExecutedEventArgs(job, DateTime.Now, ex));
            }
        }
    }
}

public class JobExecutedEventArgs : EventArgs
{
    public ScheduledJob Job { get; }
    public DateTime ExecutedAt { get; }
    public Exception? Error { get; }

    public JobExecutedEventArgs(ScheduledJob job, DateTime executedAt, Exception? error = null)
    {
        Job = job;
        ExecutedAt = executedAt;
        Error = error;
    }
}
