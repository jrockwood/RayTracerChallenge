// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Intersection.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using RayTracerChallenge.Library.Shapes;

    /// <summary>
    /// Represents an immutable intersection between a ray and a <see cref="Shape"/>.
    /// </summary>
    public sealed class Intersection
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Intersection(double t, Shape shape)
        {
            T = t;
            Shape = shape;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double T { get; }
        public Shape Shape { get; }
    }
}
