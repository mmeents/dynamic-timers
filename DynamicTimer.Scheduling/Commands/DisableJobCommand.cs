using FluentResults;
using MediatR;

namespace DynamicTimer.Scheduling.Commands;

/// <summary>
/// Command to disable a scheduled job.
/// </summary>
public class DisableJobCommand : IRequest<Result>
{
    public string JobId { get; set; } = string.Empty;
}
