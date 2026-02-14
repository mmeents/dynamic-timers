using DynamicTimer.Scheduling.Models;
using DynamicTimer.Scheduling.Notifications;
using DynamicTimer.Scheduling.Scheduling;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DynamicTimer.Scheduling.Services;

/// <summary>
/// Hosted service that wraps the CronScheduler and manages its lifecycle.
/// Provides thread-safe access to the scheduler and publishes job execution notifications via MediatR.
/// </summary>
public class CronSchedulerHostedService : IHostedService
{
    private readonly CronScheduler _scheduler;
    private readonly IMediator _mediator;
    private readonly ILogger<CronSchedulerHostedService> _logger;
    public event EventHandler<JobExecutedNotification>? JobExecuted;
    public CronSchedulerHostedService(        
        IMediator mediator,
        ILogger<CronSchedulerHostedService> logger)
    {
        _scheduler = new CronScheduler();
        _mediator = mediator;
        _logger = logger;

        // Subscribe to job execution events
        _scheduler.JobExecuted += OnJobExecuted;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("CronSchedulerHostedService starting");
        // Note: We don't auto-start the scheduler here.
        // It will be started via StartSchedulerCommand
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("CronSchedulerHostedService stopping");
        _scheduler.Stop();
        return Task.CompletedTask;
    }

    // caught by notification handler it calls this method to raise the event to notify the handlers
    internal void DoJobExecutedNotify(JobExecutedNotification notification)
    {  
        JobExecuted?.Invoke(this, notification);              
    }

    private async void OnJobExecuted(object? sender, JobExecutedEventArgs e)
    {
        try
        {             
            var notification = new JobExecutedNotification(
                e.Job.Id,
                e.Job.Name,
                e.ExecutedAt,
                e.Error?.Message
            );            
            await _mediator.Publish(notification);  // Publish the notification via MediatR
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing job executed notification");
        }
    }

    // Internal methods for handlers to interact with scheduler
    internal void AddJob(ScheduledJob job) => _scheduler.AddJob(job);

    internal bool RemoveJob(string jobId) => _scheduler.RemoveJob(jobId);

    internal bool UpdateJob(string jobId, string? name = null, string? cronExpression = null, bool? isEnabled = null)
        => _scheduler.UpdateJob(jobId, name, cronExpression, isEnabled);

    internal List<ScheduledJob> GetJobs() => _scheduler.GetJobs();

    internal ScheduledJob? GetJobById(string jobId) => _scheduler.GetJobById(jobId);

    internal bool IsRunning => _scheduler.IsRunning;

    internal void Start() => _scheduler.Start();

    internal void Stop() => _scheduler.Stop();
}
