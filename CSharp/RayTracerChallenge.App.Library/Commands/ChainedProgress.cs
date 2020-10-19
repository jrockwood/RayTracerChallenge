// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ChainedProgress.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Commands
{
    using System;

    /// <summary>
    /// Similar to <see cref="Progress{T}"/>, but allows for chaining progress instances together. When <see
    /// cref="IProgress{T}.Report"/> is invoked, the supplied <c>reportAction</c> is invoked, followed by the
    /// chained <see cref="NextProgress"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChainedProgress<T> : Progress<T>
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public ChainedProgress(Action<T> reportAction, IProgress<T>? nextProgress)
            : base(reportAction)
        {
            NextProgress = nextProgress;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        /// <summary>
        /// Gets the <see cref="IProgress{T}"/> that should be invoked after this progress.
        /// </summary>
        public IProgress<T>? NextProgress { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected override void OnReport(T value)
        {
            base.OnReport(value);
            NextProgress?.Report(value);
        }
    }
}
