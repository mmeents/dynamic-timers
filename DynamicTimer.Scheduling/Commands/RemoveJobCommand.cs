using FluentResults;
using MediatR;

namespace DynamicTimer.Scheduling.Commands;

/// <summary>
/// Command to remove a scheduled job from the scheduler.
/// </summary>
public class RemoveJobCommand : IRequest<Result>
{
    public string JobId { get; set; } = string.Empty;
}
