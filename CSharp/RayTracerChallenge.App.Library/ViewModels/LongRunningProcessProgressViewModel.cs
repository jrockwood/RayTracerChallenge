// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LongRunningProcessProgressViewModel.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.ViewModels
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using RayTracerChallenge.App.Library.Commands;
    using RayTracerChallenge.App.Library.SystemServices;

    public class LongRunningProcessProgressViewModel : Observable
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly IDispatcherTimer _timer;
        private readonly Func<DateTime> _getNowFunc;

        private DateTime? _startingTime;
        private int _percentComplete;
        private bool _autoStop = true;
        private string _startButtonText = "Start";
        private string _cancelButtonText = "Cancel";

        private CancellationTokenSource? _cancellationTokenSource;
        private CancellationToken? _wrappedCancellationToken;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        /// <summary>
        /// Creates a new <see cref="LongRunningProcessProgressViewModel"/>.
        /// </summary>
        /// <param name="attachedStartCommand">The command to run when the start button is clicked.</param>
        /// <param name="timer">The timer to use. This is mainly for unit tests.</param>
        /// <param name="getNowFunc">A function that returns the current time. This is mainly for unit tests.</param>
        public LongRunningProcessProgressViewModel(
            IViewModelAsyncCommand attachedStartCommand,
            IDispatcherTimer? timer = null,
            Func<DateTime>? getNowFunc = null)
        {
            _timer = timer ?? new WrappedDispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.IsEnabled = false;
            _timer.Tick += OnTimerTick;

            _getNowFunc = getNowFunc ?? (() => DateTime.Now);

            // Create the start command and route events from the wrapped command to the exposed command.
            StartCommand = new RelayAsyncCommand(
                CreateStartCommandFunc(attachedStartCommand),
                attachedStartCommand.IsEnabled);

            attachedStartCommand.CanExecuteChanged +=
                (sender, args) => StartCommand.IsEnabled = attachedStartCommand.IsEnabled;

            // Create the cancel command.
            CancelCommand = new RelayCommand(ExecuteCancelCommand, isEnabled: false);
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="StopTimer"/> should be called automatically when <see
        /// cref="PercentComplete"/> hits 100. Defaults to true.
        /// </summary>
        public bool AutoStop
        {
            get => _autoStop;
            set => SetProperty(ref _autoStop, value);
        }

        public int PercentComplete
        {
            get => _percentComplete;
            internal set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentException("Must be between 0-100, inclusive", nameof(PercentComplete));
                }

                if (!SetProperty(ref _percentComplete, value))
                {
                    return;
                }

                UpdateTimes();

                if (value == 100 && AutoStop)
                {
                    StopTimer();
                }
            }
        }

        public TimeSpan ElapsedTime => _startingTime.HasValue ? _getNowFunc() - _startingTime.Value : TimeSpan.MinValue;

        public string FormattedElapsedTime =>
            _startingTime.HasValue
                ? FormatTimeSpan(ElapsedTime, includeMs: true)
                : FormatTimeSpan(TimeSpan.Zero, includeMs: true);

        public TimeSpan EstimatedTimeRemaining
        {
            get
            {
                if (PercentComplete == 0)
                {
                    return TimeSpan.MaxValue;
                }

                long elapsedTicks = ElapsedTime.Ticks;
                double percentComplete = PercentComplete / 100.0;
                double percentLeft = 1 - percentComplete;
                long totalTicks = (long)((elapsedTicks / percentComplete) * percentLeft);
                return new TimeSpan(totalTicks);
            }
        }

        public string FormattedEstimatedTimeRemaining => _startingTime.HasValue
            ? FormatTimeSpan(EstimatedTimeRemaining, includeMs: false)
            : FormatTimeSpan(TimeSpan.Zero, includeMs: false);

        public bool IsStarted => _timer.IsEnabled;

        public string StartButtonText
        {
            get => _startButtonText;
            set => SetProperty(ref _startButtonText, value);
        }

        public IViewModelAsyncCommand StartCommand { get; }

        public string CancelButtonText
        {
            get => _cancelButtonText;
            set => SetProperty(ref _cancelButtonText, value);
        }

        public IViewModelCommand CancelCommand { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        /// <summary>
        /// Resets the state by cancelling any running commands, setting the percent to zero, and enabling the
        /// appropriate commands.
        /// </summary>
        public void Reset()
        {
            CancelCommand.ExecuteIfEnabled();
            PercentComplete = 0;
            _startingTime = null;
            UpdateTimes();
        }

        /// <summary>
        /// Starts the elapsed time counter.
        /// </summary>
        internal void StartTimer()
        {
            _startingTime = _getNowFunc();
            _timer.IsEnabled = true;
            PercentComplete = 0;
            CancelCommand.IsEnabled = true;
        }

        /// <summary>
        /// Stops the elapsed time counter. Also gets called automatically when the percent reaches 100.
        /// </summary>
        internal void StopTimer()
        {
            _timer.IsEnabled = false;
            CancelCommand.IsEnabled = false;
        }

        private void UpdateTimes()
        {
            OnPropertyChanged(nameof(ElapsedTime));
            OnPropertyChanged(nameof(FormattedElapsedTime));
            OnPropertyChanged(nameof(EstimatedTimeRemaining));
            OnPropertyChanged(nameof(FormattedEstimatedTimeRemaining));
        }

        private static string FormatTimeSpan(TimeSpan timeSpan, bool includeMs)
        {
            if (timeSpan == TimeSpan.MaxValue)
            {
                return "";
            }

            // If there are hours, we don't need to show ms accuracy.
            if (timeSpan.TotalHours >= 1)
            {
                return $"{Math.Floor(timeSpan.TotalHours)}h:{timeSpan.Minutes:00}m:{timeSpan.Seconds:00}s";
            }

            if (includeMs)
            {
                return $"{timeSpan.Minutes}m:{timeSpan.Seconds:00}s:{timeSpan.Milliseconds:000}ms";
            }

            return $"{timeSpan.Minutes}m:{timeSpan.Seconds:00}s";
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            UpdateTimes();

            if (_wrappedCancellationToken.GetValueOrDefault().IsCancellationRequested)
            {
                CancelCommand.Execute();
            }
        }

        private void ExecuteCancelCommand()
        {
            _cancellationTokenSource?.Cancel();
        }

        private ExecuteCommandFunc CreateStartCommandFunc(IViewModelAsyncCommand wrappedStartCommand)
        {
            async Task ExecuteStartCommand(IProgress<int>? wrappedProgress, CancellationToken wrappedToken)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _wrappedCancellationToken = wrappedToken;

                StartTimer();
                try
                {
                    var chainedProgress = new ChainedProgress<int>(
                        percentComplete => PercentComplete = percentComplete,
                        wrappedProgress);

                    await wrappedStartCommand.ExecuteAsync(chainedProgress, _cancellationTokenSource.Token);
                    PercentComplete = 100;
                }
                finally
                {
                    StopTimer();
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                    _wrappedCancellationToken = null;
                }
            }

            return ExecuteStartCommand;
        }
    }
}
