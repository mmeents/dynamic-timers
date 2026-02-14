using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DynamicTimer.Scheduling.Extensions;

namespace DynamicTimer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Create and configure the host
            var host = CreateHostBuilder().Build();

            // Start hosted services in background
            _ = host.RunAsync();

            // Get Form1 from DI container
            var form = host.Services.GetRequiredService<Form1>();

            // Run the application with the form
            Application.Run(form);

            // Shutdown host when form closes
            host.StopAsync().Wait();
            host.Dispose();
        }

        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {                   
                    // Register cron scheduling services
                    services.AddCronScheduling();                
                    // Register Form1 as singleton (so notification handlers work)
                    services.AddSingleton<Form1>();                    
                });
    }
}