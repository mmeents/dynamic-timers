namespace DynamicTimer.Scheduling.Models;

/// <summary>
/// Data Transfer Object for scheduler status information.
/// </summary>
public class SchedulerStatusDto
{
    /// <summary>
    /// Indicates whether the scheduler is currently running.
    /// </summary>
    public bool IsRunning { get; set; }

    /// <summary>
    /// Total number of jobs in the scheduler.
    /// </summary>
    public int JobCount { get; set; }

    /// <summary>
    /// The next scheduled execution time across all jobs (earliest NextRun).
    /// </summary>
    public DateTime? NextExecutionTime { get; set; }

    /// <summary>
    /// Number of enabled jobs.
    /// </summary>
    public int EnabledJobCount { get; set; }

    /// <summary>
    /// Number of disabled jobs.
    /// </summary>
    public int DisabledJobCount { get; set; }
}
