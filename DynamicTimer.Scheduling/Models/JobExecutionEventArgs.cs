using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicTimer.Scheduling.Models
{
    /// <summary>
    /// Event arguments for job execution events.
    /// </summary>
    public class JobExecutedEventArgs : EventArgs
    {
      public ScheduledJob Job { get; }
      public DateTime ExecutedAt { get; }
      public Exception? Error { get; }
      public JobExecutedEventArgs(ScheduledJob job, DateTime executedAt, Exception? error = null) {
        Job = job;
        ExecutedAt = executedAt;
        Error = error;
      }

    }
}
