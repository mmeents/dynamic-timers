using DynamicTimer.Scheduling.Models;
using MediatR;

namespace DynamicTimer.Scheduling.Queries;

/// <summary>
/// Query to retrieve a specific scheduled job by its ID.
/// </summary>
public class GetJobByIdQuery : IRequest<ScheduledJobDto?>
{
    public string JobId { get; set; } = string.Empty;
}
