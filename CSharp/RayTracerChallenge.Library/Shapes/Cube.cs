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
        public static readonly BoundingBox CubeBox = new BoundingBox(s_minPoint, s_maxPoint);

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Cube(string? name = null, Matrix4x4? transform = null, Material? material = null)
            : base(name, transform, material)
        {
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public override BoundingBox BoundingBox => CubeBox;

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

        protected internal override Vector LocalNormalAt(Point localPoint, Intersection? hit = null)
        {
            double absX = Math.Abs(localPoint.X);
            double absY = Math.Abs(localPoint.Y);
            double absZ = Math.Abs(localPoint.Z);

            double maxC = Math.Max(absX, Math.Max(absY, absZ));

            if (Math.Abs(maxC - absX) < double.Epsilon)
            {
                return new Vector(localPoint.X, 0, 0);
            }

            if (Math.Abs(maxC - absY) < double.Epsilon)
            {
                return new Vector(0, localPoint.Y, 0);
            }

            return new Vector(0, 0, localPoint.Z);
        }
    }
}
