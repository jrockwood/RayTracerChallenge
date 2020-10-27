// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IntersectionState.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Collections.Generic;
    using RayTracerChallenge.Library.Shapes;

    /// <summary>
    /// Immutable data structure containing precomputed values pertaining to an intersection and a ray.
    /// </summary>
    public sealed class IntersectionState
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        private IntersectionState(
            double t,
            Shape shape,
            Point point,
            Point overPoint,
            Point underPoint,
            Vector eye,
            Vector normal,
            Vector reflection,
            double n1,
            double n2,
            bool isInside)
        {
            T = t;
            Shape = shape;
            Point = point;
            OverPoint = overPoint;
            UnderPoint = underPoint;
            Eye = eye;
            Normal = normal;
            Reflection = reflection;
            N1 = n1;
            N2 = n2;
            IsInside = isInside;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double T { get; }
        public Shape Shape { get; }

        public Point Point { get; }
        public Point OverPoint { get; }
        public Point UnderPoint { get; }

        public Vector Eye { get; }
        public Vector Normal { get; }
        public Vector Reflection { get; }

        public double N1 { get; }
        public double N2 { get; }

        public bool IsInside { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public static IntersectionState Create(Intersection hit, Ray ray, IntersectionList intersections)
        {
            // Copy the intersection's properties for convenience.
            double t = hit.T;
            Shape shape = hit.Shape;

            // Precompute some useful values.
            Point point = ray.PositionAt(hit.T);
            Vector eye = -ray.Direction;
            Vector normal = shape.NormalAt(point);
            bool isInside = false;

            if (normal.Dot(eye) < 0)
            {
                isInside = true;
                normal = -normal;
            }

            Point overPoint = point + (normal * NumberExtensions.Epsilon);
            Point underPoint = point - (normal * NumberExtensions.Epsilon);

            Vector reflection = ray.Direction.Reflect(normal);

            // Calculate the refraction values.
            var containers = new List<Shape>();
            double n1 = 1.0;
            double n2 = 1.0;

            foreach (Intersection intersection in intersections)
            {
                if (intersection == hit)
                {
                    n1 = containers.Count == 0 ? 1.0 : containers[^1].Material.RefractiveIndex;
                }

                int shapeIndex = containers.IndexOf(intersection.Shape);
                if (shapeIndex >= 0)
                {
                    containers.RemoveAt(shapeIndex);
                }
                else
                {
                    containers.Add(intersection.Shape);
                }

                if (intersection == hit)
                {
                    n2 = containers.Count == 0 ? 1.0 : containers[^1].Material.RefractiveIndex;
                    break;
                }
            }

            return new IntersectionState(
                t,
                shape,
                point,
                overPoint,
                underPoint,
                eye,
                normal,
                reflection,
                n1,
                n2,
                isInside);
        }

        public double Schlick()
        {
            // Find the cosine of the angle between the eye and the normal vectors.
            double cos = Eye.Dot(Normal);

            // Total internal reflection can only occur if n1 > n2.
            if (N1 > N2)
            {
                double n = N1 / N2;
                double sin2T = n * n * (1.0 - (cos * cos));
                if (sin2T > 1.0)
                {
                    return 1.0;
                }

                // Compute cosine of theta_t using trigonometric identity.
                double cosT = Math.Sqrt(1.0 - sin2T);

                // When N1 > N2, use cos(theta_t) instead.
                cos = cosT;
            }

            double r0 = Math.Pow((N1 - N2) / (N1 + N2), 2);
            double reflectance = r0 + ((1 - r0) * Math.Pow(1 - cos, 5));
            return reflectance;
        }
    }
}
