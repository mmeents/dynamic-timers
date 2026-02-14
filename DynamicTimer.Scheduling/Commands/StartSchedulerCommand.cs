using FluentResults;
using MediatR;

namespace DynamicTimer.Scheduling.Commands;

/// <summary>
/// Command to start the cron scheduler.
/// </summary>
public class StartSchedulerCommand : IRequest<Result>
{
}
