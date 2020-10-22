// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ICanvas.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    /// <summary>
    /// Represents a canvas that contains a color for every pixel.
    /// </summary>
    public interface ICanvas
    {
        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        int Width { get; }
        int Height { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        Color GetPixel(int x, int y);
    }
}
