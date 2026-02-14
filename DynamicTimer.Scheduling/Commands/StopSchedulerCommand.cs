using FluentResults;
using MediatR;

namespace DynamicTimer.Scheduling.Commands;

/// <summary>
/// Command to stop the cron scheduler.
/// </summary>
public class StopSchedulerCommand : IRequest<Result>
{
}
