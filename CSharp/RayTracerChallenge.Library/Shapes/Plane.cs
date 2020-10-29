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
        //// Constructors
        //// ===========================================================================================================

        public Plane(Matrix4x4? transform = null, Material? material = null)
            : base(transform, material)
        {
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            // If the ray is parallel to the plane, there are no intersections.
            if (Math.Abs(localRay.Direction.Y) < NumberExtensions.Epsilon)
            {
                return IntersectionList.Empty;
            }

            double t = -localRay.Origin.Y / localRay.Direction.Y;
            return IntersectionList.Create((t, this));
        }

        protected internal override Vector LocalNormalAt(Point localPoint)
        {
            return Vector.UnitY;
        }
    }
}
