namespace DynamicTimer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblJobName = new System.Windows.Forms.Label();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.lblCronExpression = new System.Windows.Forms.Label();
            this.txtCronExpression = new System.Windows.Forms.TextBox();
            this.btnAddJob = new System.Windows.Forms.Button();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.dgvJobs = new System.Windows.Forms.DataGridView();
            this.btnRemoveJob = new System.Windows.Forms.Button();
            this.lblExecutionLog = new System.Windows.Forms.Label();
            this.rtbExecutionLog = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).BeginInit();
            this.SuspendLayout();
            //
            // lblJobName
            //
            this.lblJobName.AutoSize = true;
            this.lblJobName.Location = new System.Drawing.Point(12, 15);
            this.lblJobName.Name = "lblJobName";
            this.lblJobName.Size = new System.Drawing.Size(70, 15);
            this.lblJobName.TabIndex = 0;
            this.lblJobName.Text = "Job Name:";
            //
            // txtJobName
            //
            this.txtJobName.Location = new System.Drawing.Point(100, 12);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(250, 23);
            this.txtJobName.TabIndex = 1;
            //
            // lblCronExpression
            //
            this.lblCronExpression.AutoSize = true;
            this.lblCronExpression.Location = new System.Drawing.Point(12, 44);
            this.lblCronExpression.Name = "lblCronExpression";
            this.lblCronExpression.Size = new System.Drawing.Size(181, 15);
            this.lblCronExpression.TabIndex = 2;
            this.lblCronExpression.Text = "Cron Expression (e.g., */5 * * * *):";
            //
            // txtCronExpression
            //
            this.txtCronExpression.Location = new System.Drawing.Point(200, 41);
            this.txtCronExpression.Name = "txtCronExpression";
            this.txtCronExpression.Size = new System.Drawing.Size(150, 23);
            this.txtCronExpression.TabIndex = 3;
            //
            // btnAddJob
            //
            this.btnAddJob.Location = new System.Drawing.Point(356, 12);
            this.btnAddJob.Name = "btnAddJob";
            this.btnAddJob.Size = new System.Drawing.Size(100, 52);
            this.btnAddJob.TabIndex = 4;
            this.btnAddJob.Text = "Add Job";
            this.btnAddJob.UseVisualStyleBackColor = true;
            this.btnAddJob.Click += new System.EventHandler(this.btnAddJob_Click);
            //
            // btnStartStop
            //
            this.btnStartStop.Location = new System.Drawing.Point(462, 12);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(120, 52);
            this.btnStartStop.TabIndex = 5;
            this.btnStartStop.Text = "Start Scheduler";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            //
            // dgvJobs
            //
            this.dgvJobs.AllowUserToAddRows = false;
            this.dgvJobs.AllowUserToDeleteRows = false;
            this.dgvJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvJobs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJobs.Location = new System.Drawing.Point(12, 70);
            this.dgvJobs.Name = "dgvJobs";
            this.dgvJobs.ReadOnly = true;
            this.dgvJobs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvJobs.Size = new System.Drawing.Size(776, 200);
            this.dgvJobs.TabIndex = 6;
            //
            // btnRemoveJob
            //
            this.btnRemoveJob.Location = new System.Drawing.Point(12, 276);
            this.btnRemoveJob.Name = "btnRemoveJob";
            this.btnRemoveJob.Size = new System.Drawing.Size(150, 30);
            this.btnRemoveJob.TabIndex = 7;
            this.btnRemoveJob.Text = "Remove Selected Job";
            this.btnRemoveJob.UseVisualStyleBackColor = true;
            this.btnRemoveJob.Click += new System.EventHandler(this.btnRemoveJob_Click);
            //
            // lblExecutionLog
            //
            this.lblExecutionLog.AutoSize = true;
            this.lblExecutionLog.Location = new System.Drawing.Point(12, 315);
            this.lblExecutionLog.Name = "lblExecutionLog";
            this.lblExecutionLog.Size = new System.Drawing.Size(88, 15);
            this.lblExecutionLog.TabIndex = 8;
            this.lblExecutionLog.Text = "Execution Log:";
            //
            // rtbExecutionLog
            //
            this.rtbExecutionLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbExecutionLog.Location = new System.Drawing.Point(12, 333);
            this.rtbExecutionLog.Name = "rtbExecutionLog";
            this.rtbExecutionLog.ReadOnly = true;
            this.rtbExecutionLog.Size = new System.Drawing.Size(776, 105);
            this.rtbExecutionLog.TabIndex = 9;
            this.rtbExecutionLog.Text = "";
            //
            // Form1
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rtbExecutionLog);
            this.Controls.Add(this.lblExecutionLog);
            this.Controls.Add(this.btnRemoveJob);
            this.Controls.Add(this.dgvJobs);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.btnAddJob);
            this.Controls.Add(this.txtCronExpression);
            this.Controls.Add(this.lblCronExpression);
            this.Controls.Add(this.txtJobName);
            this.Controls.Add(this.lblJobName);
            this.Name = "Form1";
            this.Text = "Cron Scheduler";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblJobName;
        private System.Windows.Forms.TextBox txtJobName;
        private System.Windows.Forms.Label lblCronExpression;
        private System.Windows.Forms.TextBox txtCronExpression;
        private System.Windows.Forms.Button btnAddJob;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.DataGridView dgvJobs;
        private System.Windows.Forms.Button btnRemoveJob;
        private System.Windows.Forms.Label lblExecutionLog;
        private System.Windows.Forms.RichTextBox rtbExecutionLog;
    }
}
