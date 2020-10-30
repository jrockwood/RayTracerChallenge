// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Cube.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;

    public class Cube : Shape
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private static readonly Point s_minPoint = new Point(-1, -1, -1);
        private static readonly Point s_maxPoint = new Point(1, 1, 1);

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Cube(Matrix4x4? transform = null, Material? material = null)
            : base(transform, material)
        {
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            if (BoundingBox.TryLocalIntersect(localRay, s_minPoint, s_maxPoint, out double tMin, out double tMax))
            {
                return new IntersectionList((tMin, this), (tMax, this));
            }

            return IntersectionList.Empty;
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
    }
}
