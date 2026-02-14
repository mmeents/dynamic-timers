using DynamicTimer.Scheduling.Models;
using MediatR;

namespace DynamicTimer.Scheduling.Queries;

/// <summary>
/// Query to retrieve the current status of the scheduler.
/// </summary>
public class GetSchedulerStatusQuery : IRequest<SchedulerStatusDto>
{
}
