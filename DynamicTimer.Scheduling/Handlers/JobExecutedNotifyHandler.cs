using DynamicTimer.Scheduling.Notifications;
using DynamicTimer.Scheduling.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicTimer.Scheduling.Handlers
{
    public class JobExecutedNotifyHandler( CronSchedulerHostedService cronService ) : INotificationHandler<JobExecutedNotification> {
        private readonly CronSchedulerHostedService _cronService = cronService;
        Task INotificationHandler<JobExecutedNotification>.Handle(JobExecutedNotification notification, CancellationToken cancellationToken)
        {
           _cronService.DoJobExecutedNotify(notification);
           return Task.CompletedTask;
        }
    }
}
