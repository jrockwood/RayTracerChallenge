namespace RayTracerChallenge.App.Controls
{
    partial class ProgressControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label _elapsedTimeDescriptionLabel;
            System.Windows.Forms.Label _remainingTimeDescriptionLabel;
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._percentCompleteLabel = new System.Windows.Forms.Label();
            this._elapsedTimeLabel = new System.Windows.Forms.Label();
            this._remainingTimeLabel = new System.Windows.Forms.Label();
            this._timer = new System.Windows.Forms.Timer(this.components);
            _elapsedTimeDescriptionLabel = new System.Windows.Forms.Label();
            _remainingTimeDescriptionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _elapsedTimeDescriptionLabel
            // 
            _elapsedTimeDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            _elapsedTimeDescriptionLabel.AutoSize = true;
            _elapsedTimeDescriptionLabel.Location = new System.Drawing.Point(0, 32);
            _elapsedTimeDescriptionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            _elapsedTimeDescriptionLabel.Name = "_elapsedTimeDescriptionLabel";
            _elapsedTimeDescriptionLabel.Size = new System.Drawing.Size(77, 15);
            _elapsedTimeDescriptionLabel.TabIndex = 2;
            _elapsedTimeDescriptionLabel.Text = "Elapsed time:";
            // 
            // _remainingTimeDescriptionLabel
            // 
            _remainingTimeDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            _remainingTimeDescriptionLabel.AutoSize = true;
            _remainingTimeDescriptionLabel.Location = new System.Drawing.Point(135, 32);
            _remainingTimeDescriptionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            _remainingTimeDescriptionLabel.Name = "_remainingTimeDescriptionLabel";
            _remainingTimeDescriptionLabel.Size = new System.Drawing.Size(146, 15);
            _remainingTimeDescriptionLabel.TabIndex = 4;
            _remainingTimeDescriptionLabel.Text = "Estimated remaining time:";
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBar.Location = new System.Drawing.Point(0, 0);
            this._progressBar.Margin = new System.Windows.Forms.Padding(2, 2, 5, 4);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(607, 28);
            this._progressBar.TabIndex = 0;
            // 
            // _percentCompleteLabel
            // 
            this._percentCompleteLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._percentCompleteLabel.Location = new System.Drawing.Point(614, 0);
            this._percentCompleteLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._percentCompleteLabel.Name = "_percentCompleteLabel";
            this._percentCompleteLabel.Size = new System.Drawing.Size(43, 28);
            this._percentCompleteLabel.TabIndex = 1;
            this._percentCompleteLabel.Text = "100%";
            this._percentCompleteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _elapsedTimeLabel
            // 
            this._elapsedTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._elapsedTimeLabel.AutoSize = true;
            this._elapsedTimeLabel.Location = new System.Drawing.Point(81, 32);
            this._elapsedTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 9, 0);
            this._elapsedTimeLabel.Name = "_elapsedTimeLabel";
            this._elapsedTimeLabel.Size = new System.Drawing.Size(43, 15);
            this._elapsedTimeLabel.TabIndex = 3;
            this._elapsedTimeLabel.Text = "0:00:00";
            // 
            // _remainingTimeLabel
            // 
            this._remainingTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._remainingTimeLabel.AutoSize = true;
            this._remainingTimeLabel.Location = new System.Drawing.Point(285, 32);
            this._remainingTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._remainingTimeLabel.Name = "_remainingTimeLabel";
            this._remainingTimeLabel.Size = new System.Drawing.Size(43, 15);
            this._remainingTimeLabel.TabIndex = 5;
            this._remainingTimeLabel.Text = "0:00:00";
            // 
            // _timer
            // 
            this._timer.Interval = 500;
            this._timer.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // ProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._remainingTimeLabel);
            this.Controls.Add(_remainingTimeDescriptionLabel);
            this.Controls.Add(this._elapsedTimeLabel);
            this.Controls.Add(_elapsedTimeDescriptionLabel);
            this.Controls.Add(this._percentCompleteLabel);
            this.Controls.Add(this._progressBar);
            this.Name = "ProgressControl";
            this.Size = new System.Drawing.Size(657, 47);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Label _percentCompleteLabel;
        private System.Windows.Forms.Label _elapsedTimeLabel;
        private System.Windows.Forms.Label _remainingTimeLabel;
        private System.Windows.Forms.Timer _timer;
    }
}
