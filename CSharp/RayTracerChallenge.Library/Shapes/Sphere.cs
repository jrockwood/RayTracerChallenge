// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Sphere.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;

    public class Sphere : Shape
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Sphere(Matrix4x4? transform = null, Material? material = null, bool hideShadow = false)
            : base(transform, material, hideShadow)
        {
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public static Sphere CreateGlassSphere()
        {
            return new Sphere(material: new Material(transparency: 1, refractiveIndex: 1.5));
        }

        public override Shape WithTransform(Matrix4x4 value)
        {
            return new Sphere(value, Material);
        }

        public override Shape WithMaterial(Material value)
        {
            return new Sphere(Transform, value);
        }

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            Vector sphereToRay = localRay.Origin - Point.Zero;

            // Calculate the discriminant using this formula: b^2 - 4ac
            double a = localRay.Direction.Dot(localRay.Direction);
            double b = 2 * localRay.Direction.Dot(sphereToRay);
            double c = sphereToRay.Dot(sphereToRay) - 1;
            double discriminant = (b * b) - (4 * a * c);

            if (discriminant < 0)
            {
                return IntersectionList.Empty;
            }

            double sqrtOfDiscriminant = Math.Sqrt(discriminant);
            double t1 = (-b - sqrtOfDiscriminant) / (2 * a);
            double t2 = (-b + sqrtOfDiscriminant) / (2 * a);

            return IntersectionList.Create((t1, this), (t2, this));
        }

        protected internal override Vector LocalNormalAt(Point localPoint)
        {
            Vector localNormal = localPoint - Point.Zero;
            return localNormal;
        }
    }
}
