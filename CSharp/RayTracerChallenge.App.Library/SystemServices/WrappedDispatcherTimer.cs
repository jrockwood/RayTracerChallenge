// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="WrappedDispatcherTimer.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
//

namespace RayTracerChallenge.App.Library.SystemServices
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// Wraps a <see cref="DispatcherTimer"/> so that it can implement <see cref="IDispatcherTimer"/>.
    /// </summary>
    internal sealed class WrappedDispatcherTimer : IDispatcherTimer
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly DispatcherTimer _wrapped = new DispatcherTimer();

        //// ===========================================================================================================
        //// Events
        //// ===========================================================================================================

        public event EventHandler Tick
        {
            add => _wrapped.Tick += value;
            remove => _wrapped.Tick -= value;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public TimeSpan Interval
        {
            get => _wrapped.Interval;
            set => _wrapped.Interval = value;
        }

        public bool IsEnabled
        {
            get => _wrapped.IsEnabled;
            set => _wrapped.IsEnabled = value;
        }
    }
}
