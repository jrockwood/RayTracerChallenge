// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Shape.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    public abstract class Shape
    {
        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public IntersectionList Intersect(Ray ray)
        {
            return LocalIntersect(ray);
        }

        protected abstract IntersectionList LocalIntersect(Ray localRay);
    }
}
