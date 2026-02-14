using FluentResults;
using MediatR;

namespace DynamicTimer.Scheduling.Commands;

/// <summary>
/// Command to enable a scheduled job.
/// </summary>
public class EnableJobCommand : IRequest<Result>
{
    public string JobId { get; set; } = string.Empty;
}
