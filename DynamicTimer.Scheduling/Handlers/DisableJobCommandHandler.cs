using DynamicTimer.Scheduling.Commands;
using DynamicTimer.Scheduling.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DynamicTimer.Scheduling.Handlers;

public class DisableJobCommandHandler : IRequestHandler<DisableJobCommand, Result>
{
    private readonly CronSchedulerHostedService _schedulerService;
    private readonly ILogger<DisableJobCommandHandler> _logger;

    public DisableJobCommandHandler(
        CronSchedulerHostedService schedulerService,
        ILogger<DisableJobCommandHandler> logger)
    {
        _schedulerService = schedulerService;
        _logger = logger;
    }

    public Task<Result> Handle(DisableJobCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.JobId))
            return Task.FromResult(Result.Fail("Job ID is required"));

        var updated = _schedulerService.UpdateJob(request.JobId, isEnabled: false);

        if (!updated)
        {
            _logger.LogWarning("Job not found for disable: {JobId}", request.JobId);
            return Task.FromResult(Result.Fail($"Job not found: {request.JobId}"));
        }

        _logger.LogInformation("Job disabled via MediatR: {JobId}", request.JobId);
        return Task.FromResult(Result.Ok());
    }
}
