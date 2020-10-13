// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IntersectionState.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using RayTracerChallenge.Library.Shapes;

    /// <summary>
    /// Immutable data structure containing precomputed values pertaining to an intersection and a ray.
    /// </summary>
    public sealed class IntersectionState
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        private IntersectionState(float t, Shape shape, Point point, Vector eye, Vector normal, bool isInside)
        {
            T = t;
            Shape = shape;
            Point = point;
            Eye = eye;
            Normal = normal;
            IsInside = isInside;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public float T { get; }
        public Shape Shape { get; }
        public Point Point { get; }
        public Vector Eye { get; }
        public Vector Normal { get; }
        public bool IsInside { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public static IntersectionState Create(Intersection intersection, Ray ray)
        {
            // Copy the intersection's properties for convenience.
            float t = intersection.T;
            Shape shape = intersection.Shape;

            // Precompute some useful values.
            Point point = ray.PositionAt(intersection.T);
            Vector eye = -ray.Direction;
            Vector normal = shape.NormalAt(point);
            bool isInside = false;

            if (normal.Dot(eye) < 0)
            {
                isInside = true;
                normal = -normal;
            }

            return new IntersectionState(t, shape, point, eye, normal, isInside);
        }
    }
}
