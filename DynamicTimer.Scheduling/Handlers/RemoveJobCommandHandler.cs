using DynamicTimer.Scheduling.Commands;
using DynamicTimer.Scheduling.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DynamicTimer.Scheduling.Handlers;

public class RemoveJobCommandHandler : IRequestHandler<RemoveJobCommand, Result>
{
    private readonly CronSchedulerHostedService _schedulerService;
    private readonly ILogger<RemoveJobCommandHandler> _logger;

    public RemoveJobCommandHandler(
        CronSchedulerHostedService schedulerService,
        ILogger<RemoveJobCommandHandler> logger)
    {
        _schedulerService = schedulerService;
        _logger = logger;
    }

    public Task<Result> Handle(RemoveJobCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.JobId))
            return Task.FromResult(Result.Fail("Job ID is required"));

        var removed = _schedulerService.RemoveJob(request.JobId);

        if (!removed)
        {
            _logger.LogWarning("Job not found for removal: {JobId}", request.JobId);
            return Task.FromResult(Result.Fail($"Job not found: {request.JobId}"));
        }

        _logger.LogInformation("Job removed via MediatR: {JobId}", request.JobId);
        return Task.FromResult(Result.Ok());
    }
}
