﻿// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Ray.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    public sealed class Ray
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Ray(Point origin, Vector direction)
        {
            Origin = origin;
            Direction = direction;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Point Origin { get; }
        public Vector Direction { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public Point PositionAt(double t)
        {
            return Origin + (Direction * t);
        }

        public Ray Transform(Matrix4x4 transform)
        {
            Point newOrigin = transform * Origin;
            Vector newDirection = transform * Direction;
            return new Ray(newOrigin, newDirection);
        }
    }
}
