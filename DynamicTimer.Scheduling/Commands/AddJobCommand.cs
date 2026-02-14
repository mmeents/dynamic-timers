using FluentResults;
using MediatR;

namespace DynamicTimer.Scheduling.Commands;

/// <summary>
/// Command to add a new scheduled job to the scheduler.
/// </summary>
public class AddJobCommand : IRequest<Result<string>>
{
    public string Name { get; set; } = string.Empty;
    public string CronExpression { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
}
