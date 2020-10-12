// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Light.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Lights
{
    /// <summary>
    /// Abstract base class for all lights.
    /// </summary>
    public abstract class Light
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected Light(Point position, Color intensity)
        {
            Position = position;
            Intensity = intensity;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Point Position { get; }

        public Color Intensity { get; }
    }
}
