// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeDispatcherTimer.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Tests.Fakes
{
    using System;
    using System.Threading.Tasks;
    using RayTracerChallenge.App.Library.SystemServices;

    public class FakeDispatcherTimer : IDispatcherTimer
    {
        public event EventHandler? Tick;

        public TimeSpan Interval { get; set; }
        public bool IsEnabled { get; set; }

        public Task RaiseTickEventAsync()
        {
            return Task.Run(() => Tick?.Invoke(this, EventArgs.Empty));
        }
    }
}
