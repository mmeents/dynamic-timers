using DynamicTimer.Scheduling.Models;
using MediatR;

namespace DynamicTimer.Scheduling.Queries;

/// <summary>
/// Query to retrieve all scheduled jobs.
/// </summary>
public class GetAllJobsQuery : IRequest<List<ScheduledJobDto>>
{
}
