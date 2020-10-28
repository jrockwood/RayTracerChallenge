// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Cylinder.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;

    public class Cylinder : Shape
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Cylinder(
            double minimumY,
            double maximumY,
            bool isClosed = false,
            Matrix4x4? transform = null,
            Material? material = null,
            bool hideShadow = false)
            : base(transform, material, hideShadow)
        {
            MinimumY = minimumY;
            MaximumY = maximumY;
            IsClosed = isClosed;
        }

        public Cylinder(Matrix4x4? transform = null, Material? material = null, bool hideShadow = false)
            : this(double.NegativeInfinity, double.PositiveInfinity, false, transform, material, hideShadow)
        {
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double MinimumY { get; }
        public double MaximumY { get; }

        public bool IsClosed { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Shape WithTransform(Matrix4x4 value)
        {
            return new Cylinder(value, Material, HideShadow);
        }

        public override Shape WithMaterial(Material value)
        {
            return new Cylinder(Transform, value, HideShadow);
        }

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            double a = (localRay.Direction.X * localRay.Direction.X) + (localRay.Direction.Z * localRay.Direction.Z);

            // Ray is parallel to the y axis.
            if (a.IsApproximatelyEqual(0))
            {
                return IntersectCaps(localRay, IntersectionList.Empty);
            }

            double b = (2 * localRay.Origin.X * localRay.Direction.X) + (2 * localRay.Origin.Z * localRay.Direction.Z);
            double c = ((localRay.Origin.X * localRay.Origin.X) + (localRay.Origin.Z * localRay.Origin.Z)) - 1;
            double discriminant = (b * b) - (4 * a * c);

            // Ray does not intersect the cylinder.
            if (discriminant < 0)
            {
                return IntersectionList.Empty;
            }

            double sqrtDiscriminant = Math.Sqrt(discriminant);
            double t0 = (-b - sqrtDiscriminant) / (2 * a);
            double t1 = (-b + sqrtDiscriminant) / (2 * a);

            if (t0 > t1)
            {
                double temp = t0;
                t0 = t1;
                t1 = temp;
            }

            var intersections = IntersectionList.Empty;

            double y0 = localRay.Origin.Y + (t0 * localRay.Direction.Y);
            double y1 = localRay.Origin.Y + (t1 * localRay.Direction.Y);

            if (y0 > MinimumY && y0 < MaximumY)
            {
                intersections = intersections.Add((t0, this));
            }

            if (y1 > MinimumY && y1 < MaximumY)
            {
                intersections = intersections.Add((t1, this));
            }

            intersections = IntersectCaps(localRay, intersections);

            return intersections;
        }

        protected internal override Vector LocalNormalAt(Point localPoint)
        {
            // Compute the square of the distance from the y axis.
            double distance = (localPoint.X * localPoint.X) + (localPoint.Z * localPoint.Z);
            if (distance < 1 && localPoint.Y >= MaximumY - NumberExtensions.Epsilon)
            {
                return Vector.UnitY;
            }
            else if (distance < 1 && localPoint.Y <= MinimumY + NumberExtensions.Epsilon)
            {
                return new Vector(0, -1, 0);
            }

            return new Vector(localPoint.X, 0, localPoint.Z);
        }

        /// <summary>
        /// Checks to see if the intersection at `t` is within a radius of 1 (the radius of the cylinder) from the y axis.
        /// </summary>
        private bool CheckCapHit(Ray ray, double t)
        {
            double x = ray.Origin.X + (t * ray.Direction.X);
            double z = ray.Origin.Z + (t * ray.Direction.Z);
            return (x * x) + (z * z) <= 1;
        }

        private IntersectionList IntersectCaps(Ray ray, IntersectionList intersections)
        {
            // Caps only matter if the cylinder is closed, and might possibly be intersected by the ray.
            if (!IsClosed || ray.Direction.Y.IsApproximatelyEqual(0))
            {
                return intersections;
            }

            // Check for an intersection with the lower end cap by intersecting the ray with the plane at MinimumY.
            double t = (MinimumY - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCapHit(ray, t))
            {
                intersections = intersections.Add((t, this));
            }

            // Check for an intersection with the upper end cap by intersecting the ray with the plane at MaximumY.
            t = (MaximumY - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCapHit(ray, t))
            {
                intersections = intersections.Add((t, this));
            }

            return intersections;
        }
    }
}
