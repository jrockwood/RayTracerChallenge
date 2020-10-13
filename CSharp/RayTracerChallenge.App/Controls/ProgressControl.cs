// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressControl.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Controls
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;

    /// <summary>
    /// Contains a progress bar and labels showing the percent complete, elapsed time, and estimated time remaining.
    /// </summary>
    [SuppressMessage("ReSharper", "LocalizableElement")]
    public partial class ProgressControl : UserControl
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private DateTime _startingTime;
        private int _percentComplete;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public ProgressControl()
        {
            InitializeComponent();
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int PercentComplete
        {
            get => _percentComplete;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentException("Must be between 0-100, inclusive", nameof(PercentComplete));
                }

                if (_percentComplete == value)
                {
                    return;
                }

                _percentComplete = value;
                UpdateProgress(value);
            }
        }

        public TimeSpan ElapsedTime => DateTime.Now - _startingTime;

        public string FormattedElapsedTime => FormatTimeSpan(ElapsedTime);

        public TimeSpan EstimatedTimeRemaining
        {
            get
            {
                if (PercentComplete == 0)
                {
                    return TimeSpan.MaxValue;
                }

                double percentComplete = PercentComplete * 0.01;
                double percentLeft = 1 - percentComplete;
                long totalTicks = (long)((ElapsedTime.Ticks / percentComplete) * percentLeft);
                return new TimeSpan(totalTicks);
            }
        }

        public string FormattedEstimatedTimeRemaining => FormatTimeSpan(EstimatedTimeRemaining);

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        /// <summary>
        /// Starts the elapsed time counter.
        /// </summary>
        public void Start()
        {
            _startingTime = DateTime.Now;
            _timer.Enabled = true;
            UpdateProgress(0);
        }

        /// <summary>
        /// Stops the elapsed time counter. Also gets called automatically when the percent reaches 100.
        /// </summary>
        public void Stop()
        {
            _timer.Enabled = false;
        }

        private void UpdateProgress(int percentComplete)
        {
            _progressBar.Value = percentComplete;
            _percentCompleteLabel.Text = $"{percentComplete}%";
            UpdateTimes();

            if (percentComplete == 100)
            {
                Stop();
            }
        }

        private void UpdateTimes()
        {
            _elapsedTimeLabel.Text = FormattedElapsedTime;
            _remainingTimeLabel.Text = FormattedEstimatedTimeRemaining;
        }

        private static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return timeSpan == TimeSpan.MaxValue
                ? ""
                : $"{timeSpan.Hours}:{timeSpan.Minutes:#0}:{timeSpan.Seconds:#0}";
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            UpdateTimes();
        }
    }
}
