using FluentResults;
using MediatR;

namespace DynamicTimer.Scheduling.Commands;

/// <summary>
/// Command to update an existing scheduled job.
/// </summary>
public class UpdateJobCommand : IRequest<Result>
{
    public string JobId { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? CronExpression { get; set; }
    public bool? IsEnabled { get; set; }
}
