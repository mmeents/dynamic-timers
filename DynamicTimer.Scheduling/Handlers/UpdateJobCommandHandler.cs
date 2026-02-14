using DynamicTimer.Scheduling.Commands;
using DynamicTimer.Scheduling.Scheduling;
using DynamicTimer.Scheduling.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DynamicTimer.Scheduling.Handlers;

public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, Result>
{
    private readonly CronSchedulerHostedService _schedulerService;
    private readonly ILogger<UpdateJobCommandHandler> _logger;

    public UpdateJobCommandHandler(
        CronSchedulerHostedService schedulerService,
        ILogger<UpdateJobCommandHandler> logger)
    {
        _schedulerService = schedulerService;
        _logger = logger;
    }

    public Task<Result> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.JobId))
            return Task.FromResult(Result.Fail("Job ID is required"));

        // Validate cron expression if provided
        if (!string.IsNullOrWhiteSpace(request.CronExpression))
        {
            if (!CronExpression.TryParse(request.CronExpression, out _))
                return Task.FromResult(Result.Fail($"Invalid cron expression: {request.CronExpression}"));
        }

        try
        {
            var updated = _schedulerService.UpdateJob(
                request.JobId,
                request.Name,
                request.CronExpression,
                request.IsEnabled);

            if (!updated)
            {
                _logger.LogWarning("Job not found for update: {JobId}", request.JobId);
                return Task.FromResult(Result.Fail($"Job not found: {request.JobId}"));
            }

            _logger.LogInformation("Job updated via MediatR: {JobId}", request.JobId);
            return Task.FromResult(Result.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job: {JobId}", request.JobId);
            return Task.FromResult(Result.Fail($"Failed to update job: {ex.Message}"));
        }
    }
}
