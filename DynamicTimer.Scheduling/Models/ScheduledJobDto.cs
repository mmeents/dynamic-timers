namespace DynamicTimer.Scheduling.Models;

/// <summary>
/// Data Transfer Object for scheduled jobs. This is the public-facing model.
/// </summary>
public class ScheduledJobDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CronExpression { get; set; } = string.Empty;
    public DateTime? LastRun { get; set; }
    public DateTime? NextRun { get; set; }
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Maps an internal ScheduledJob to a public DTO.
    /// </summary>
    internal static ScheduledJobDto FromInternal(ScheduledJob job)
    {
        return new ScheduledJobDto
        {
            Id = job.Id,
            Name = job.Name,
            CronExpression = job.CronExpression,
            LastRun = job.LastRun,
            NextRun = job.NextRun,
            IsEnabled = job.IsEnabled
        };
    }
}
