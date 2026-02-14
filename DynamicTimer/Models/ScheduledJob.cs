namespace DynamicTimer.Models;

public class ScheduledJob
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CronExpression { get; set; } = string.Empty;
    public DateTime? LastRun { get; set; }
    public DateTime? NextRun { get; set; }
    public bool IsEnabled { get; set; }

    public ScheduledJob()
    {
        Id = Guid.NewGuid().ToString();
        IsEnabled = true;
    }

    public ScheduledJob(string name, string cronExpression) : this()
    {
        Name = name;
        CronExpression = cronExpression;
    }
}
