// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Cube.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class Cube : Shape
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Cube(Matrix4x4? transform = null, Material? material = null, bool hideShadow = false)
            : base(transform, material, hideShadow)
        {
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Shape WithTransform(Matrix4x4 value)
        {
            return new Cube(value, Material);
        }

        public override Shape WithMaterial(Material value)
        {
            return new Cube(Transform, value);
        }

        [SuppressMessage("ReSharper", "IdentifierTypo")]
        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            (double xtmin, double xtmax) = CheckAxis(localRay.Origin.X, localRay.Direction.X);
            (double ytmin, double ytmax) = CheckAxis(localRay.Origin.Y, localRay.Direction.Y);
            (double ztmin, double ztmax) = CheckAxis(localRay.Origin.Z, localRay.Direction.Z);

            double tmin = Math.Max(xtmin, Math.Max(ytmin, ztmin));
            double tmax = Math.Min(xtmax, Math.Min(ytmax, ztmax));

            if (tmin > tmax)
            {
                return IntersectionList.Empty;
            }

            return IntersectionList.Create(new Intersection(tmin, this), new Intersection(tmax, this));
        }

        protected internal override Vector LocalNormalAt(Point localPoint)
        {
            // ReSharper disable once IdentifierTypo
            double maxc = Math.Max(Math.Abs(localPoint.X), Math.Max(Math.Abs(localPoint.Y), Math.Abs(localPoint.Z)));

            if (Math.Abs(maxc - Math.Abs(localPoint.X)) < double.Epsilon)
            {
                return new Vector(localPoint.X, 0, 0);
            }
            else if (Math.Abs(maxc - Math.Abs(localPoint.Y)) < double.Epsilon)
            {
                return new Vector(0, localPoint.Y, 0);
            }

            return new Vector(0, 0, localPoint.Z);
        }

        [SuppressMessage("ReSharper", "IdentifierTypo")]
        private static (double tmin, double tmax) CheckAxis(double origin, double direction)
        {
            double tminNumerator = -1 - origin;
            double tmaxNumerator = 1 - origin;

            double tmin = tminNumerator / direction;
            double tmax = tmaxNumerator / direction;

            if (tmin > tmax)
            {
                double temp = tmin;
                tmin = tmax;
                tmax = temp;
            }

            return (tmin, tmax);
        }
    }
}
