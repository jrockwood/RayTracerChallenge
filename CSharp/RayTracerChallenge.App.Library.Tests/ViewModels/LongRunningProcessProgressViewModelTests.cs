// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LongRunningProcessProgressViewModelTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Tests.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.App.Library.Tests.Fakes;
    using RayTracerChallenge.App.Library.ViewModels;

    public class LongRunningProcessProgressViewModelTests
    {
        [Test]
        public void Ctor_should_use_default_values()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            vm.AutoStop.Should().BeTrue();
            vm.PercentComplete.Should().Be(0);

            vm.ElapsedTime.Should().Be(TimeSpan.MinValue);
            vm.FormattedElapsedTime.Should().Be("0m:00s:000ms");

            vm.EstimatedTimeRemaining.Should().Be(TimeSpan.MaxValue);
            vm.FormattedEstimatedTimeRemaining.Should().Be("0m:00s");

            vm.IsStarted.Should().BeFalse();

            vm.StartButtonText.Should().Be("Start");
            vm.CancelButtonText.Should().Be("Cancel");
        }

        [Test]
        public void Ctor_should_set_timer_values()
        {
            var timer = new FakeDispatcherTimer();
            _ = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), timer);
            timer.Interval.Should().Be(TimeSpan.FromMilliseconds(500));
            timer.IsEnabled.Should().BeFalse();
        }

        //// ===========================================================================================================
        //// Property Setter Tests
        //// ===========================================================================================================

        [Test]
        public void AutoStop_should_raise_PropertyChanged_correctly()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            using var monitoredVm = vm.Monitor();
            vm.AutoStop = false;
            monitoredVm.Should().RaisePropertyChangeFor(x => x.AutoStop);

            monitoredVm.Clear();
            vm.AutoStop = false;
            monitoredVm.Should().NotRaisePropertyChangeFor(x => x.AutoStop);
        }

        [Test]
        public void StartButtonText_should_raise_PropertyChanged_correctly()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            using var monitoredVm = vm.Monitor();
            vm.StartButtonText = "Start me";
            monitoredVm.Should().RaisePropertyChangeFor(x => x.StartButtonText);

            monitoredVm.Clear();
            vm.StartButtonText = "Start me";
            monitoredVm.Should().NotRaisePropertyChangeFor(x => x.StartButtonText);
        }

        [Test]
        public void CancelButtonText_should_raise_PropertyChanged_correctly()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            using var monitoredVm = vm.Monitor();
            vm.CancelButtonText = "Stop me";
            monitoredVm.Should().RaisePropertyChangeFor(x => x.CancelButtonText);

            monitoredVm.Clear();
            vm.CancelButtonText = "Stop me";
            monitoredVm.Should().NotRaisePropertyChangeFor(x => x.CancelButtonText);
        }

        [Test]
        public void PercentComplete_should_throw_on_invalid_values()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            Action action = () => vm.PercentComplete = -1;
            action.Should()
                .ThrowExactly<ArgumentException>()
                .And.ParamName.Should()
                .Be(nameof(LongRunningProcessProgressViewModel.PercentComplete));

            action = () => vm.PercentComplete = 101;
            action.Should()
                .ThrowExactly<ArgumentException>()
                .And.ParamName.Should()
                .Be(nameof(LongRunningProcessProgressViewModel.PercentComplete));
        }

        [Test]
        public void PercentComplete_should_raise_PropertyChanged_correctly()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            using var monitoredVm = vm.Monitor();
            vm.PercentComplete = 45;
            monitoredVm.Should().RaisePropertyChangeFor(x => x.PercentComplete);

            monitoredVm.Clear();
            vm.PercentComplete = 45;
            monitoredVm.Should().NotRaisePropertyChangeFor(x => x.PercentComplete);
        }

        [Test]
        public void PercentComplete_should_also_raise_PropertyChanged_for_the_time_properties()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            using var monitoredVm = vm.Monitor();
            vm.PercentComplete = 45;
            monitoredVm.Should().RaisePropertyChangeFor(x => x.ElapsedTime);
            monitoredVm.Should().RaisePropertyChangeFor(x => x.FormattedElapsedTime);
            monitoredVm.Should().RaisePropertyChangeFor(x => x.EstimatedTimeRemaining);
            monitoredVm.Should().RaisePropertyChangeFor(x => x.FormattedEstimatedTimeRemaining);
        }

        //// ===========================================================================================================
        //// Start/Stop Tests
        //// ===========================================================================================================

        [Test]
        public void Start_should_start_the_timer()
        {
            var timer = new FakeDispatcherTimer();
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), timer);
            vm.StartTimer();
            timer.IsEnabled.Should().BeTrue();
        }

        [Test]
        public void Stop_should_stop_the_timer()
        {
            var timer = new FakeDispatcherTimer();
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), timer);
            vm.StartTimer();
            vm.StopTimer();
            timer.IsEnabled.Should().BeFalse();
        }

        [Test]
        public void Start_should_set_PercentComplete_to_0()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer())
            {
                PercentComplete = 50
            };

            vm.StartTimer();
            vm.PercentComplete.Should().Be(0);
        }

        [Test]
        public void PercentComplete_should_automatically_stop_if_AutoStop_is_true_and_the_percent_is_100()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            vm.StartTimer();
            vm.PercentComplete = 100;
            vm.IsStarted.Should().BeFalse();

            vm.PercentComplete = 0;
            vm.AutoStop = false;
            vm.StartTimer();
            vm.PercentComplete = 100;
            vm.IsStarted.Should().BeTrue();
        }

        //// ===========================================================================================================
        //// Timer Tick Tests
        //// ===========================================================================================================

        [Test]
        public async Task When_the_timer_ticks_the_time_properties_should_raise_PropertyChanged_events()
        {
            var timer = new FakeDispatcherTimer();
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), timer);
            using var monitoredVm = vm.Monitor();

            await timer.RaiseTickEventAsync();
            monitoredVm.Should().RaisePropertyChangeFor(x => x.ElapsedTime);
            monitoredVm.Should().RaisePropertyChangeFor(x => x.FormattedElapsedTime);
            monitoredVm.Should().RaisePropertyChangeFor(x => x.EstimatedTimeRemaining);
            monitoredVm.Should().RaisePropertyChangeFor(x => x.FormattedEstimatedTimeRemaining);
        }

        //// ===========================================================================================================
        //// ElapsedTime Tests
        //// ===========================================================================================================

        [Test]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public void ElapsedTime_should_track_the_time_since_Start_was_called()
        {
            int callCount = 0;
            var baseDate = new DateTime(2020, 1, 1);

            DateTime GetNow() =>
                callCount switch
                {
                    0 => baseDate,
                    1 => baseDate.AddMilliseconds(500),
                    2 => baseDate.AddSeconds(5),
                    3 => baseDate.AddMinutes(10),
                    4 => baseDate.AddHours(20),
                    5 => baseDate.AddDays(2),
                    6 => baseDate.AddHours(14).AddMinutes(13).AddSeconds(12),
                    _ => throw new InvalidOperationException(),
                };

            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer(), GetNow);
            vm.StartTimer();

            vm.ElapsedTime.TotalMilliseconds.Should().Be(0);
            vm.FormattedElapsedTime.Should().Be("0m:00s:000ms");

            callCount++;
            vm.ElapsedTime.TotalMilliseconds.Should().Be(500);
            vm.FormattedElapsedTime.Should().Be("0m:00s:500ms");

            callCount++;
            vm.ElapsedTime.TotalSeconds.Should().Be(5);
            vm.FormattedElapsedTime.Should().Be("0m:05s:000ms");

            callCount++;
            vm.ElapsedTime.TotalMinutes.Should().Be(10);
            vm.FormattedElapsedTime.Should().Be("10m:00s:000ms");

            callCount++;
            vm.ElapsedTime.TotalHours.Should().Be(20);
            vm.FormattedElapsedTime.Should().Be("20h:00m:00s");

            callCount++;
            vm.ElapsedTime.TotalDays.Should().Be(2);
            vm.FormattedElapsedTime.Should().Be("48h:00m:00s");

            callCount++;
            vm.ElapsedTime.TotalHours.Should().Be(14.22);
            vm.FormattedElapsedTime.Should().Be("14h:13m:12s");
        }

        //// ===========================================================================================================
        //// EstimatedTimeRemaining Tests
        //// ===========================================================================================================

        [Test]
        public void EstimatedTimeRemaining_should_return_the_max_value_if_the_percent_is_zero()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            vm.StartTimer();
            vm.EstimatedTimeRemaining.Should().Be(TimeSpan.MaxValue);
        }

        [Test]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public void EstimatedTimeRemaining_should_calculate_correctly()
        {
            // Assume the total time is 10 minutes, make sure that the estimated time is correct for different percentages.
            int callCount = 0;
            var baseDate = new DateTime(2020, 1, 1);

            DateTime GetNow() =>
                callCount switch
                {
                    /* 0% */
                    0 => baseDate,
                    /* 10% */
                    1 => baseDate.AddMinutes(1),
                    /* 25% */
                    2 => baseDate.AddMinutes(2.5),
                    /* 50% */
                    3 => baseDate.AddMinutes(5),
                    /* 75% */
                    4 => baseDate.AddMinutes(7.5),
                    /* 80% */
                    5 => baseDate.AddMinutes(8),
                    /* 90% */
                    6 => baseDate.AddMinutes(9),
                    /* 99% */
                    7 => baseDate.AddMinutes(9).AddSeconds(45),
                    /* 100% */
                    8 => baseDate.AddMinutes(10),
                    _ => throw new InvalidOperationException(),
                };

            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer(), GetNow);
            vm.StartTimer();

            vm.EstimatedTimeRemaining.Should().Be(TimeSpan.MaxValue);
            vm.FormattedEstimatedTimeRemaining.Should().Be("");

            callCount++;
            vm.PercentComplete = 10;
            vm.EstimatedTimeRemaining.Should().Be(TimeSpan.FromMinutes(9));
            vm.FormattedEstimatedTimeRemaining.Should().Be("9m:00s");

            callCount++;
            vm.PercentComplete = 25;
            vm.EstimatedTimeRemaining.Should().Be(TimeSpan.FromMinutes(7.5));
            vm.FormattedEstimatedTimeRemaining.Should().Be("7m:30s");

            callCount++;
            vm.PercentComplete = 50;
            vm.EstimatedTimeRemaining.Should().Be(TimeSpan.FromMinutes(5));
            vm.FormattedEstimatedTimeRemaining.Should().Be("5m:00s");

            callCount++;
            vm.PercentComplete = 75;
            vm.EstimatedTimeRemaining.Should().Be(TimeSpan.FromMinutes(2.5));
            vm.FormattedEstimatedTimeRemaining.Should().Be("2m:30s");

            callCount++;
            vm.PercentComplete = 80;
            vm.EstimatedTimeRemaining.Should().BeCloseTo(TimeSpan.FromMinutes(2));
            vm.FormattedEstimatedTimeRemaining.Should().Be("1m:59s");

            callCount++;
            vm.PercentComplete = 90;
            vm.EstimatedTimeRemaining.Should().BeCloseTo(TimeSpan.FromMinutes(1));
            vm.FormattedEstimatedTimeRemaining.Should().Be("0m:59s");

            callCount++;
            vm.PercentComplete = 99;
            vm.EstimatedTimeRemaining.Should().BeCloseTo(TimeSpan.FromSeconds(5.9));
            vm.FormattedEstimatedTimeRemaining.Should().Be("0m:05s");

            callCount++;
            vm.PercentComplete = 100;
            vm.EstimatedTimeRemaining.Should().BeCloseTo(TimeSpan.FromSeconds(0));
            vm.FormattedEstimatedTimeRemaining.Should().Be("0m:00s");
        }

        //// ===========================================================================================================
        //// Start Command Tests
        //// ===========================================================================================================

        [Test]
        public void StartCommand_should_be_enabled_by_default()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            vm.StartCommand.IsEnabled.Should().BeTrue();
        }

        [Test]
        public Task StartCommand_should_start_the_timer()
        {
            var timer = new FakeDispatcherTimer();
            var vm = new LongRunningProcessProgressViewModel(
                new FakeAsyncCommand(FakeAsyncCommandBehavior.ExecuteReturnsAsyncNoopTask),
                timer);

            var task = vm.StartCommand.ExecuteAsync();
            timer.IsEnabled.Should().BeTrue();
            return task;
        }

        [Test]
        public async Task StartCommand_should_stop_the_timer_when_completed()
        {
            var timer = new FakeDispatcherTimer();
            var vm = new LongRunningProcessProgressViewModel(
                new FakeAsyncCommand(FakeAsyncCommandBehavior.ExecuteReturnsAsyncNoopTask),
                timer);

            await vm.StartCommand.ExecuteAsync();
            timer.IsEnabled.Should().BeFalse();
        }

        [Test]
        public async Task StartCommand_should_invoke_the_original_command()
        {
            var command = new FakeAsyncCommand(FakeAsyncCommandBehavior.ExecuteReturnsAsyncNoopTask);
            var vm = new LongRunningProcessProgressViewModel(command, new FakeDispatcherTimer());

            await vm.StartCommand.ExecuteAsync();
            command.ExecuteWasInvoked.Should().BeTrue();
        }

        [Test]
        public async Task StartCommand_should_update_the_progress()
        {
            var command = new FakeAsyncCommand(FakeAsyncCommandBehavior.ExecuteReturnsAsyncNoopTask);
            var vm = new LongRunningProcessProgressViewModel(command, new FakeDispatcherTimer());
            using var monitoredVm = vm.Monitor();
            var progress = new FakeProgress();

            await vm.StartCommand.ExecuteAsync(progress);
            monitoredVm.Should().RaisePropertyChangeFor(x => x.PercentComplete);
        }

        [Test]
        public async Task StartCommand_should_invoke_the_supplied_progress()
        {
            var command = new FakeAsyncCommand(FakeAsyncCommandBehavior.ExecuteReturnsAsyncNoopTask);
            var vm = new LongRunningProcessProgressViewModel(command, new FakeDispatcherTimer());
            var progress = new FakeProgress();

            await vm.StartCommand.ExecuteAsync(progress);
            progress.ReportedValues.Should().HaveCount(2).And.ContainInOrder(48, 100);
        }

        [Test]
        public void StartCommand_enabled_status_should_match_the_original_command()
        {
            var command = new FakeAsyncCommand { IsEnabled = false };
            var vm = new LongRunningProcessProgressViewModel(command, new FakeDispatcherTimer());
            vm.StartCommand.IsEnabled.Should().BeFalse();
        }

        [Test]
        public void StartCommand_should_become_enabled_when_the_original_command_becomes_enabled()
        {
            var command = new FakeAsyncCommand { IsEnabled = false };
            var vm = new LongRunningProcessProgressViewModel(command, new FakeDispatcherTimer());
            vm.StartCommand.IsEnabled.Should().BeFalse();

            command.IsEnabled = true;
            vm.StartCommand.IsEnabled.Should().BeTrue();
        }

        //// ===========================================================================================================
        //// CancelCommand Tests
        //// ===========================================================================================================

        [Test]
        public void CancelCommand_should_be_disabled_by_default()
        {
            var vm = new LongRunningProcessProgressViewModel(new FakeAsyncCommand(), new FakeDispatcherTimer());
            vm.CancelCommand.IsEnabled.Should().BeFalse();
        }

        [Test]
        public async Task CancelCommand_should_be_enabled_when_start_is_invoked_and_disabled_after_completion()
        {
            var command = new FakeAsyncCommand(FakeAsyncCommandBehavior.ExecuteReturnsAsyncNoopTask);
            var vm = new LongRunningProcessProgressViewModel(command, new FakeDispatcherTimer());
            var task = vm.StartCommand.ExecuteAsync();
            vm.CancelCommand.IsEnabled.Should().BeTrue();

            await task;
            vm.CancelCommand.IsEnabled.Should().BeFalse();
        }

        [Test]
        public async Task CancelCommand_should_cancel_the_running_start_command()
        {
            var command = new FakeAsyncCommand(FakeAsyncCommandBehavior.ExecuteWaitsForCancel);
            var vm = new LongRunningProcessProgressViewModel(command, new FakeDispatcherTimer());

            var task = vm.StartCommand.ExecuteAsync();
            vm.CancelCommand.Execute();
            await task;

            command.WasCancelled.Should().BeTrue();
        }

        [Test]
        public async Task
            Cancellation_should_occur_when_the_start_command_CancellationToken_is_signalled_on_the_next_timer_tick()
        {
            var command = new FakeAsyncCommand(FakeAsyncCommandBehavior.ExecuteWaitsForCancel);
            var timer = new FakeDispatcherTimer();
            var vm = new LongRunningProcessProgressViewModel(command, timer);

            using var cancellationTokenSource = new CancellationTokenSource();
            var task = vm.StartCommand.ExecuteAsync(cancellationToken: cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();
            await timer.RaiseTickEventAsync();

            await task;

            command.WasCancelled.Should().BeTrue();
        }
    }
}
