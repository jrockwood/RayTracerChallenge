// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderProgressStep.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    /// <summary>
    /// Contains information about a camera's render progress.
    /// </summary>
    public sealed class RenderProgressStep
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RenderProgressStep(int percentComplete, int pixelX, int pixelY, Color pixelColor)
        {
            PercentComplete = percentComplete;
            PixelX = pixelX;
            PixelY = pixelY;
            PixelColor = pixelColor;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int PercentComplete { get; }
        public int PixelX { get; }
        public int PixelY { get; }
        public Color PixelColor { get; }
    }
}
