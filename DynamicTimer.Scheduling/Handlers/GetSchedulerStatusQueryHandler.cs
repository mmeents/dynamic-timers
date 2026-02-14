using DynamicTimer.Scheduling.Models;
using DynamicTimer.Scheduling.Queries;
using DynamicTimer.Scheduling.Services;
using MediatR;

namespace DynamicTimer.Scheduling.Handlers;

public class GetSchedulerStatusQueryHandler : IRequestHandler<GetSchedulerStatusQuery, SchedulerStatusDto>
{
    private readonly CronSchedulerHostedService _schedulerService;

    public GetSchedulerStatusQueryHandler(CronSchedulerHostedService schedulerService)
    {
        _schedulerService = schedulerService;
    }

    public Task<SchedulerStatusDto> Handle(GetSchedulerStatusQuery request, CancellationToken cancellationToken)
    {
        var jobs = _schedulerService.GetJobs();

        var status = new SchedulerStatusDto
        {
            IsRunning = _schedulerService.IsRunning,
            JobCount = jobs.Count,
            EnabledJobCount = jobs.Count(j => j.IsEnabled),
            DisabledJobCount = jobs.Count(j => !j.IsEnabled),
            NextExecutionTime = jobs
                .Where(j => j.IsEnabled && j.NextRun.HasValue)
                .OrderBy(j => j.NextRun)
                .FirstOrDefault()?.NextRun
        };

        return Task.FromResult(status);
    }
}
