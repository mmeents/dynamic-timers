using DynamicTimer.Scheduling.Commands;
using DynamicTimer.Scheduling.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DynamicTimer.Scheduling.Handlers;

public class StopSchedulerCommandHandler : IRequestHandler<StopSchedulerCommand, Result>
{
    private readonly CronSchedulerHostedService _schedulerService;
    private readonly ILogger<StopSchedulerCommandHandler> _logger;

    public StopSchedulerCommandHandler(
        CronSchedulerHostedService schedulerService,
        ILogger<StopSchedulerCommandHandler> logger)
    {
        _schedulerService = schedulerService;
        _logger = logger;
    }

    public Task<Result> Handle(StopSchedulerCommand request, CancellationToken cancellationToken)
    {
        if (!_schedulerService.IsRunning)
        {
            _logger.LogWarning("Scheduler is not running");
            return Task.FromResult(Result.Fail("Scheduler is not running"));
        }

        _schedulerService.Stop();
        _logger.LogInformation("Scheduler stopped via MediatR");
        return Task.FromResult(Result.Ok());
    }
}
