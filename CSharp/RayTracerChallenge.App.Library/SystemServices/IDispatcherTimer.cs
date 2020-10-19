// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IDispatcherTimer.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.SystemServices
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// Interface for a <see cref="DispatcherTimer"/> so that unit tests can easily mock a timer.
    /// </summary>
    public interface IDispatcherTimer
    {
        //// ===========================================================================================================
        //// Events
        //// ===========================================================================================================

        public event EventHandler Tick;

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public TimeSpan Interval { get; set; }

        public bool IsEnabled { get; set; }
    }
}
