using DynamicTimer.Scheduling.Models;
using DynamicTimer.Scheduling.Queries;
using DynamicTimer.Scheduling.Services;
using MediatR;

namespace DynamicTimer.Scheduling.Handlers;

public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, ScheduledJobDto?>
{
    private readonly CronSchedulerHostedService _schedulerService;

    public GetJobByIdQueryHandler(CronSchedulerHostedService schedulerService)
    {
        _schedulerService = schedulerService;
    }

    public Task<ScheduledJobDto?> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.JobId))
            return Task.FromResult<ScheduledJobDto?>(null);

        var job = _schedulerService.GetJobById(request.JobId);
        var dto = job != null ? ScheduledJobDto.FromInternal(job) : null;
        return Task.FromResult(dto);
    }
}
