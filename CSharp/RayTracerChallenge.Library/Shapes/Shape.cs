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
        //// Constructors
        //// ===========================================================================================================

        protected Shape(Matrix4x4? transform = null)
        {
            Transform = transform ?? Matrix4x4.Identity;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Matrix4x4 Transform { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public abstract Shape WithTransform(Matrix4x4 value);

        public IntersectionList Intersect(Ray ray)
        {
            var localRay = ray.Transform(Transform.Invert());
            return LocalIntersect(localRay);
        }

        protected abstract IntersectionList LocalIntersect(Ray localRay);

        public Vector NormalAt(Point worldPoint)
        {
            Point localPoint = Transform.Invert() * worldPoint;
            Vector localNormal = LocalNormalAt(localPoint);
            Vector worldNormal = Transform.Invert().Transpose() * localNormal;
            return worldNormal.Normalize();
        }

        protected abstract Vector LocalNormalAt(Point localPoint);
    }
}
