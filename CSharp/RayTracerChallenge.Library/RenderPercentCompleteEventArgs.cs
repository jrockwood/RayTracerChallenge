// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderPercentCompleteEventArgs.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;

    /// <summary>
    /// Event args for the <see cref="Camera.RenderPercentCompleteChanged"/> event.
    /// </summary>
    public sealed class RenderPercentCompleteEventArgs : EventArgs
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RenderPercentCompleteEventArgs(int percentComplete, Canvas canvas)
        {
            PercentComplete = percentComplete;
            Canvas = canvas;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int PercentComplete { get; }

        public string PercentCompleteAsString => PercentComplete + "%";

        public Canvas Canvas { get; }
    }
}
