using DynamicTimer.Scheduling.Commands;
using DynamicTimer.Scheduling.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DynamicTimer.Scheduling.Handlers;

public class StartSchedulerCommandHandler : IRequestHandler<StartSchedulerCommand, Result>
{
    private readonly CronSchedulerHostedService _schedulerService;
    private readonly ILogger<StartSchedulerCommandHandler> _logger;

    public StartSchedulerCommandHandler(
        CronSchedulerHostedService schedulerService,
        ILogger<StartSchedulerCommandHandler> logger)
    {
        _schedulerService = schedulerService;
        _logger = logger;
    }

    public Task<Result> Handle(StartSchedulerCommand request, CancellationToken cancellationToken)
    {
        if (_schedulerService.IsRunning)
        {
            _logger.LogWarning("Scheduler is already running");
            return Task.FromResult(Result.Fail("Scheduler is already running"));
        }

        _schedulerService.Start();
        _logger.LogInformation("Scheduler started via MediatR");
        return Task.FromResult(Result.Ok());
    }
}
