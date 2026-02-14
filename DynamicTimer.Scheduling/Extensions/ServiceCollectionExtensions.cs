using DynamicTimer.Scheduling.Scheduling;
using DynamicTimer.Scheduling.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicTimer.Scheduling.Extensions;

/// <summary>
/// Extension methods for registering cron scheduling services with dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all cron scheduling services including MediatR handlers, the scheduler, and hosted service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCronScheduling(this IServiceCollection services)
    {
        // Register hosted service (which wraps the scheduler)
        services.AddSingleton<CronSchedulerHostedService>();
        services.AddHostedService(provider => provider.GetRequiredService<CronSchedulerHostedService>());

        // Register MediatR and scan this assembly for handlers
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        return services;
    }
}
