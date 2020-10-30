// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="BoundingBox.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;
    using System.Linq;

    /// <summary>
    /// Describes a bounding box for a shape.
    /// </summary>
    public sealed class BoundingBox
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public BoundingBox(Point point1, Point point2, params Point[] points)
        {
            double minX = Math.Min(point1.X, point2.X);
            double minY = Math.Min(point1.Y, point2.Y);
            double minZ = Math.Min(point1.Z, point2.Z);

            double maxX = Math.Max(point1.X, point2.X);
            double maxY = Math.Max(point1.Y, point2.Y);
            double maxZ = Math.Max(point1.Z, point2.Z);

            if (points.Length > 0)
            {
                minX = Math.Min(minX, points.Min(p => p.X));
                minY = Math.Min(minY, points.Min(p => p.Y));
                minZ = Math.Min(minZ, points.Min(p => p.Z));

                maxX = Math.Max(maxX, points.Max(p => p.X));
                maxY = Math.Max(maxY, points.Max(p => p.Y));
                maxZ = Math.Max(maxZ, points.Max(p => p.Z));
            }

            MinPoint = new Point(minX, minY, minZ);
            MaxPoint = new Point(maxX, maxY, maxZ);
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Point MinPoint { get; }
        public Point MaxPoint { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public static bool TryLocalIntersect(
            Ray localRay,
            Point minPoint,
            Point maxPoint,
            out double tMin,
            out double tMax)
        {
            (double xtMin, double xtMax) = CheckAxis(localRay.Origin.X, localRay.Direction.X, minPoint.X, maxPoint.X);
            (double ytMin, double ytMax) = CheckAxis(localRay.Origin.Y, localRay.Direction.Y, minPoint.Y, maxPoint.Y);
            if (xtMin > ytMax || ytMin > xtMax)
            {
                tMin = double.NegativeInfinity;
                tMax = double.PositiveInfinity;
                return false;
            }

            (double ztMin, double ztMax) = CheckAxis(localRay.Origin.Z, localRay.Direction.Z, minPoint.Z, maxPoint.Z);

            tMin = Math.Max(xtMin, Math.Max(ytMin, ztMin));
            tMax = Math.Min(xtMax, Math.Min(ytMax, ztMax));

            return tMin <= tMax;
        }

        public bool TryLocalIntersect(Ray localRay)
        {
            return TryLocalIntersect(localRay, MinPoint, MaxPoint, out _, out _);
        }

        /// <summary>
        /// Checks to see if a component of a ray intersects the axis plane and returns the tMin and tMax values.
        /// </summary>
        /// <param name="origin">An x/y/z component of the ray's origin.</param>
        /// <param name="direction">An x/y/z component of the ray's direction.</param>
        /// <param name="min">The minimum bounds for the axis-aligned bounding box.</param>
        /// <param name="max">The maximum bounds for the axis-aligned bounding box.</param>
        /// <returns>A tuple containing the tMin and tMax intersection values.</returns>
        private static (double tMin, double tMax) CheckAxis(double origin, double direction, double min, double max)
        {
            if (min > max)
            {
                throw new ArgumentException($"Min '{min}' must be less than max '{max}'.", nameof(min));
            }

            double tMinNumerator = min - origin;
            double tMaxNumerator = max - origin;

            double tMin = tMinNumerator / direction;
            double tMax = tMaxNumerator / direction;

            if (tMin > tMax)
            {
                double temp = tMin;
                tMin = tMax;
                tMax = temp;
            }

            return (tMin, tMax);
        }
    }
}
