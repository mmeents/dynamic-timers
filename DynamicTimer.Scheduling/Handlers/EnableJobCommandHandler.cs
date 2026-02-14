using DynamicTimer.Scheduling.Commands;
using DynamicTimer.Scheduling.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DynamicTimer.Scheduling.Handlers;

public class EnableJobCommandHandler : IRequestHandler<EnableJobCommand, Result>
{
    private readonly CronSchedulerHostedService _schedulerService;
    private readonly ILogger<EnableJobCommandHandler> _logger;

    public EnableJobCommandHandler(
        CronSchedulerHostedService schedulerService,
        ILogger<EnableJobCommandHandler> logger)
    {
        _schedulerService = schedulerService;
        _logger = logger;
    }

    public Task<Result> Handle(EnableJobCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.JobId))
            return Task.FromResult(Result.Fail("Job ID is required"));

        var updated = _schedulerService.UpdateJob(request.JobId, isEnabled: true);

        if (!updated)
        {
            _logger.LogWarning("Job not found for enable: {JobId}", request.JobId);
            return Task.FromResult(Result.Fail($"Job not found: {request.JobId}"));
        }

        _logger.LogInformation("Job enabled via MediatR: {JobId}", request.JobId);
        return Task.FromResult(Result.Ok());
    }
}
