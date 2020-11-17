// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraRenderOptions.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Threading;

    /// <summary>
    /// Controls how a <see cref="Camera"/> renders a scene to a <see cref="Canvas"/>.
    /// </summary>
    public class CameraRenderOptions
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public CameraRenderOptions(
            int maxDegreeOfParallelism = -1,
            IProgress<RenderProgressStep>? progress = null,
            CancellationToken cancellationToken = default)
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism;
            Progress = progress;
            CancellationToken = cancellationToken;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        /// <summary>
        /// Provides a way to communicate progress during rendering.
        /// </summary>
        public IProgress<RenderProgressStep>? Progress { get; }

        /// <summary>
        /// The <see cref="CancellationToken"/> to use within the rendering loop to see if the rendering should be canceled.
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Gets the maximum number of threads to use for the rendering loop. Set to -1 for no limit on the number of
        /// concurrently running operations.
        /// </summary>
        public int MaxDegreeOfParallelism { get; }
    }
}
