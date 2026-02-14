using DynamicTimer.Scheduling.Commands;
using DynamicTimer.Scheduling.Models;
using DynamicTimer.Scheduling.Scheduling;
using DynamicTimer.Scheduling.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DynamicTimer.Scheduling.Handlers;

public class AddJobCommandHandler : IRequestHandler<AddJobCommand, Result<string>>
{
    private readonly CronSchedulerHostedService _schedulerService;
    private readonly ILogger<AddJobCommandHandler> _logger;

    public AddJobCommandHandler(
        CronSchedulerHostedService schedulerService,
        ILogger<AddJobCommandHandler> logger)
    {
        _schedulerService = schedulerService;
        _logger = logger;
    }

    public Task<Result<string>> Handle(AddJobCommand request, CancellationToken cancellationToken)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.Name))
            return Task.FromResult(Result.Fail<string>("Job name is required"));

        if (string.IsNullOrWhiteSpace(request.CronExpression))
            return Task.FromResult(Result.Fail<string>("Cron expression is required"));

        if (!CronExpression.TryParse(request.CronExpression, out var cronExpr))
            return Task.FromResult(Result.Fail<string>($"Invalid cron expression: {request.CronExpression}"));

        try
        {
            // Business logic
            var job = new ScheduledJob(request.Name, request.CronExpression)
            {
                IsEnabled = request.IsEnabled
            };

            _schedulerService.AddJob(job);

            _logger.LogInformation("Job added via MediatR: {JobName} ({JobId})", job.Name, job.Id);

            return Task.FromResult(Result.Ok(job.Id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding job: {JobName}", request.Name);
            return Task.FromResult(Result.Fail<string>($"Failed to add job: {ex.Message}"));
        }
    }
}
