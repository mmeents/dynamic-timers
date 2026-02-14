using MediatR;

namespace DynamicTimer.Scheduling.Notifications;

/// <summary>
/// Notification published when a scheduled job is executed.
/// </summary>
public class JobExecutedNotification : INotification
{
    public string JobId { get; set; } = string.Empty;
    public string JobName { get; set; } = string.Empty;
    public DateTime ExecutedAt { get; set; }
    public string? Error { get; set; }

    public JobExecutedNotification(string jobId, string jobName, DateTime executedAt, string? error = null)
    {
        JobId = jobId;
        JobName = jobName;
        ExecutedAt = executedAt;
        Error = error;
    }
}
