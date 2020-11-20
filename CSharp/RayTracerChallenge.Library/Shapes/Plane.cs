// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Plane.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;

    public class Plane : Shape
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private static readonly BoundingBox s_planeBox = new BoundingBox(
            new Point(double.NegativeInfinity, 0, double.NegativeInfinity),
            new Point(double.PositiveInfinity, 0, double.PositiveInfinity));

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Plane(string? name = null, Matrix4x4? transform = null, Material? material = null)
            : base(name, transform, material)
        {
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public override BoundingBox BoundingBox => s_planeBox;

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            // If the ray is parallel to the plane, there are no intersections.
            if (Math.Abs(localRay.Direction.Y) < NumberExtensions.Epsilon)
            {
                return IntersectionList.Empty;
            }

            double t = -localRay.Origin.Y / localRay.Direction.Y;
            return new IntersectionList((t, this));
        }

        protected internal override Vector LocalNormalAt(Point localPoint, Intersection? hit = null)
        {
            return Vector.UnitY;
        }
    }
}
