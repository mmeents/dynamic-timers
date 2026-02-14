using DynamicTimer.Scheduling.Commands;
using DynamicTimer.Scheduling.Queries;
using DynamicTimer.Scheduling.Notifications;
using DynamicTimer.Scheduling.Models;
using MediatR;
using DynamicTimer.Scheduling.Services;
using DynamicTimer.Scheduling.Notifications;

namespace DynamicTimer
{
    public partial class Form1 : Form
    {
        private readonly IMediator _mediator;
        private readonly CronSchedulerHostedService _cronService;

        public Form1(IMediator mediator, CronSchedulerHostedService cronService)
        {
            InitializeComponent();
            _mediator = mediator;
            _cronService = cronService;
            _cronService.JobExecuted += UpdateExecutionLog;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set up DataGridView columns
            dgvJobs.AutoGenerateColumns = false;
            dgvJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                Name = "colName"
            });
            dgvJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CronExpression",
                HeaderText = "Cron Expression",
                Name = "colCronExpression"
            });
            dgvJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NextRun",
                HeaderText = "Next Run",
                Name = "colNextRun",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "yyyy-MM-dd HH:mm:ss"
                }
            });
            dgvJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LastRun",
                HeaderText = "Last Run",
                Name = "colLastRun",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "yyyy-MM-dd HH:mm:ss"
                }
            });
            dgvJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IsEnabled",
                HeaderText = "Status",
                Name = "colStatus"
            });

            // Add sample test jobs
            AddSampleJobs();

            // Refresh the grid
            _ = RefreshJobsGrid();

            LogMessage("Application started. Click 'Start Scheduler' to begin.");
        }

        private async void AddSampleJobs()
        {
            try
            {
                await _mediator.Send(new AddJobCommand { Name = "Every Minute Test", CronExpression = "* * * * *" });
                await _mediator.Send(new AddJobCommand { Name = "Every 5 Minutes", CronExpression = "*/5 * * * *" });
                await _mediator.Send(new AddJobCommand { Name = "Every Hour at :30", CronExpression = "30 * * * *" });
            }
            catch (Exception ex)
            {
                LogMessage($"Error adding sample jobs: {ex.Message}");
            }
        }

        private async void btnAddJob_Click(object sender, EventArgs e)
        {
            var name = txtJobName.Text.Trim();
            var cronExpression = txtCronExpression.Text.Trim();

            // Validate inputs
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a job name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cronExpression))
            {
                MessageBox.Show("Please enter a cron expression.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var command = new AddJobCommand
                {
                    Name = name,
                    CronExpression = cronExpression,
                    IsEnabled = true
                };

                var result = await _mediator.Send(command);

                if (result.IsSuccess)
                {
                    LogMessage($"Job added: {name} with cron expression {cronExpression}");
                    await RefreshJobsGrid();

                    // Clear inputs
                    txtJobName.Clear();
                    txtCronExpression.Clear();
                    txtJobName.Focus();
                }
                else
                {
                    MessageBox.Show($"Failed to add job: {string.Join(", ", result.Errors.Select(e => e.Message))}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding job: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnStartStop_Click(object sender, EventArgs e)
        {
            var status = await _mediator.Send(new GetSchedulerStatusQuery());

            if (status.IsRunning)
            {
                var result = await _mediator.Send(new StopSchedulerCommand());
                if (result.IsSuccess)
                {
                    btnStartStop.Text = "Start Scheduler";
                    LogMessage("Scheduler stopped.");
                }
                else
                {
                    MessageBox.Show($"Failed to stop scheduler: {string.Join(", ", result.Errors.Select(e => e.Message))}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var result = await _mediator.Send(new StartSchedulerCommand());
                if (result.IsSuccess)
                {
                    btnStartStop.Text = "Stop Scheduler";
                    LogMessage("Scheduler started.");
                }
                else
                {
                    MessageBox.Show($"Failed to start scheduler: {string.Join(", ", result.Errors.Select(e => e.Message))}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnRemoveJob_Click(object sender, EventArgs e)
        {
            if (dgvJobs.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a job to remove.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = dgvJobs.SelectedRows[0];
            var job = selectedRow.DataBoundItem as ScheduledJobDto;

            if (job != null)
            {
                var confirmResult = MessageBox.Show($"Are you sure you want to remove job '{job.Name}'?",
                    "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    var result = await _mediator.Send(new RemoveJobCommand { JobId = job.Id });

                    if (result.IsSuccess)
                    {
                        LogMessage($"Job removed: {job.Name}");
                        await RefreshJobsGrid();
                    }
                    else
                    {
                        MessageBox.Show($"Failed to remove job: {string.Join(", ", result.Errors.Select(e => e.Message))}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }       
       

        private async void UpdateExecutionLog(object? sender, JobExecutedNotification notification)
        {
            if (notification.Error != null)
            {
                LogMessage($"[ERROR] Job '{notification.JobName}' failed: {notification.Error}");
            }
            else
            {
                LogMessage($"Job '{notification.JobName}' executed at {notification.ExecutedAt:yyyy-MM-dd HH:mm:ss}");
            }

            await RefreshJobsGrid();
        }

        private async Task RefreshJobsGrid(){

            if (dgvJobs.InvokeRequired)
            {
                dgvJobs.Invoke(new Action(async () => await RefreshJobsGrid()));
                return;
            }
            var jobs = await _mediator.Send(new GetAllJobsQuery());
            dgvJobs.DataSource = null;
            dgvJobs.DataSource = jobs;
        }

        private void LogMessage(string message)
        {
            if (rtbExecutionLog.InvokeRequired)
            {
                rtbExecutionLog.Invoke(new Action(() => LogMessage(message)));
                return;
            }
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            rtbExecutionLog.AppendText($"[{timestamp}] {message}\n");
            rtbExecutionLog.ScrollToCaret();
        }
    }
}
