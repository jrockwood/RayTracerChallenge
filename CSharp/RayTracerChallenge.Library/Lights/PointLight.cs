// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PointLight.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Lights
{
    /// <summary>
    /// Represents a light with no size, existing at a single point in space, with an intensity describing how bright it
    /// is and its color.
    /// </summary>
    public class PointLight : Light
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public PointLight(Point position, Color intensity)
            : base(position, intensity)
        {
        }
    }
}
