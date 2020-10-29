// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Cone.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a double-napped cone, which is actually two cones one on top of the other with their points touching.
    /// You can constrain the cone by using <see cref="MinimumY"/> and <see cref="MaximumY"/> (otherwise it's infinite
    /// in the y direction) and add caps to the end to make it solid via (<see cref="IsClosed"/>).
    ///    --------
    ///     \    /
    ///      \  /
    ///       \/
    ///       /\
    ///      /  \
    ///     /    \
    ///    --------
    /// </summary>
    public class Cone : Shape
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Cone(
            double minimumY = double.NegativeInfinity,
            double maximumY = double.PositiveInfinity,
            bool isClosed = false,
            Matrix4x4? transform = null,
            Material? material = null)
            : base(transform, material)
        {
            MinimumY = minimumY;
            MaximumY = maximumY;
            IsClosed = isClosed;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double MinimumY { get; set; }
        public double MaximumY { get; set; }
        public bool IsClosed { get; set; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            // A cone's intersection works almost the same as a cylinder, but a, b, and c are computed differently:
            // a = dx^2 - dy^2 + dz^2
            // b = 2ox_dx - 2oy_dy + 2oz_dz
            // c = ox^2 - oy^2 + oz^2
            double dx = localRay.Direction.X;
            double dy = localRay.Direction.Y;
            double dz = localRay.Direction.Z;

            double ox = localRay.Origin.X;
            double oy = localRay.Origin.Y;
            double oz = localRay.Origin.Z;

            double a = ((dx * dx) - (dy * dy)) + (dz * dz);
            double b = ((2 * ox * dx) - (2 * oy * dy)) + (2 * oz * dz);
            double c = ((ox * ox) - (oy * oy)) + (oz * oz);

            var intersections = new List<Intersection>();

            // If a is zero it means that the ray is parallel to one of the cone's halves, like so:
            //    --------
            //     \    /
            //      \  /
            //       \/
            //     x /\
            //    / /  \
            //   / /    \
            //    --------
            // This still means the ray might intersect the other half of the cone. In this case, the ray will miss when
            // both a and b are zero. If a is zero, but b isn't the following formula is used to find the single point
            // of intersection:
            // t = -c / 2b
            bool aIsZero = a.IsApproximatelyEqual(0);
            if (aIsZero && !b.IsApproximatelyEqual(0))
            {
                double t = -c / (2 * b);
                intersections.Add((t, this));
            }

            if (!aIsZero)
            {
                double discriminant = (b * b) - (4 * a * c);

                // Ray does not intersect the cone if the discriminant is less than zero.
                if (discriminant >= 0)
                {
                    double sqrtDiscriminant = Math.Sqrt(discriminant);
                    double t0 = (-b - sqrtDiscriminant) / (2 * a);
                    double t1 = (-b + sqrtDiscriminant) / (2 * a);

                    if (t0 > t1)
                    {
                        double temp = t0;
                        t0 = t1;
                        t1 = temp;
                    }

                    double y0 = oy + (t0 * dy);
                    if (y0 > MinimumY && y0 < MaximumY)
                    {
                        intersections.Add((t0, this));
                    }

                    double y1 = oy + (t1 * dy);
                    if (y1 > MinimumY && y1 < MaximumY)
                    {
                        intersections.Add((t1, this));
                    }
                }
            }

            IntersectCaps(localRay, intersections);
            return new IntersectionList(intersections);
        }

        protected internal override Vector LocalNormalAt(Point localPoint)
        {
            // Compute the square of the distance from the y axis.
            double distance = (localPoint.X * localPoint.X) + (localPoint.Z * localPoint.Z);
            if (distance < 1 && localPoint.Y >= MaximumY - NumberExtensions.Epsilon)
            {
                return Vector.UnitY;
            }

            if (distance < 1 && localPoint.Y <= MinimumY + NumberExtensions.Epsilon)
            {
                return new Vector(0, -1, 0);
            }

            double y = Math.Sqrt(distance);
            if (localPoint.Y > 0)
            {
                y *= -1;
            }

            return new Vector(localPoint.X, y, localPoint.Z);
        }

        /// <summary>
        /// Checks to see if the intersection at `t` is within the radius of the cylinder from the y axis.
        /// </summary>
        private static bool CheckCapHit(Ray ray, double t, double y)
        {
            double x = ray.Origin.X + (t * ray.Direction.X);
            double z = ray.Origin.Z + (t * ray.Direction.Z);
            return (x * x) + (z * z) <= y * y;
        }

        private void IntersectCaps(Ray ray, List<Intersection> intersections)
        {
            // Caps only matter if the cylinder is closed, and might possibly be intersected by the ray.
            if (!IsClosed || ray.Direction.Y.IsApproximatelyEqual(0))
            {
                return;
            }

            // Check for an intersection with the lower end cap by intersecting the ray with the plane at MinimumY.
            double t = (MinimumY - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCapHit(ray, t, MinimumY))
            {
                intersections.Add((t, this));
            }

            // Check for an intersection with the upper end cap by intersecting the ray with the plane at MaximumY.
            t = (MaximumY - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCapHit(ray, t, MaximumY))
            {
                intersections.Add((t, this));
            }
        }
    }
}
