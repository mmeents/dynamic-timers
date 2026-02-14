using DynamicTimer.Scheduling.Models;
using DynamicTimer.Scheduling.Queries;
using DynamicTimer.Scheduling.Services;
using MediatR;

namespace DynamicTimer.Scheduling.Handlers;

public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery, List<ScheduledJobDto>>
{
    private readonly CronSchedulerHostedService _schedulerService;

    public GetAllJobsQueryHandler(CronSchedulerHostedService schedulerService)
    {
        _schedulerService = schedulerService;
    }

    public Task<List<ScheduledJobDto>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = _schedulerService.GetJobs();
        var dtos = jobs.Select(ScheduledJobDto.FromInternal).ToList();
        return Task.FromResult(dtos);
    }
}
