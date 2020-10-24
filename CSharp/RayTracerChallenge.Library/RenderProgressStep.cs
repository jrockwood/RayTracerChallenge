// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderProgressStep.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System.Collections.Generic;

    /// <summary>
    /// Contains information about a camera's render progress.
    /// </summary>
    public sealed class RenderProgressStep
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RenderProgressStep(int percentComplete, int row, IEnumerable<Color> pixels)
        {
            PercentComplete = percentComplete;
            Row = row;
            Pixels = pixels;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int PercentComplete { get; }
        public int Row { get; }
        public IEnumerable<Color> Pixels { get; }
    }
}
