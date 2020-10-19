// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeAsyncCommand.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Tests.Fakes
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using RayTracerChallenge.App.Library.Commands;

    internal enum FakeAsyncCommandBehavior
    {
        ExecuteReturnsCompletedTask,
        ExecuteReturnsAsyncNoopTask,
        ExecuteWaitsForCancel
    }

    internal class FakeAsyncCommand : BaseAsyncCommand
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public FakeAsyncCommand(
            FakeAsyncCommandBehavior behavior = FakeAsyncCommandBehavior.ExecuteReturnsCompletedTask)
        {
            Behavior = behavior;
            FirstProgressPercent = 48;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public FakeAsyncCommandBehavior Behavior { get; }

        public int ExecuteInvokeCount { get; private set; }
        public bool ExecuteWasInvoked => ExecuteInvokeCount > 0;

        public int FirstProgressPercent { get; set; }

        public bool WasCancelled { get; private set; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected override Task ExecuteAsyncImpl(
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default)
        {
            ExecuteInvokeCount++;

            return Behavior switch
            {
                FakeAsyncCommandBehavior.ExecuteReturnsCompletedTask => Task.CompletedTask,
                FakeAsyncCommandBehavior.ExecuteReturnsAsyncNoopTask => Task.Run(
                    () =>
                    {
                        progress?.Report(FirstProgressPercent);
                        progress?.Report(100);
                    },
                    cancellationToken),
                FakeAsyncCommandBehavior.ExecuteWaitsForCancel => Task.Run(
                    () =>
                    {
                        progress?.Report(FirstProgressPercent);
                        WasCancelled = cancellationToken.WaitHandle.WaitOne(5000);
                    },
                    cancellationToken),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
