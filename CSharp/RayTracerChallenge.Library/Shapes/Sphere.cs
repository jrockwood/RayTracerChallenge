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
        //// Member Variables
        //// ===========================================================================================================

        private static readonly BoundingBox s_sphereBox = new BoundingBox(new Point(-1, -1, -1), new Point(1, 1, 1));

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Sphere(Matrix4x4? transform = null, Material? material = null)
            : base(transform, material)
        {
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public override BoundingBox BoundingBox => s_sphereBox;

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public static Sphere CreateGlassSphere()
        {
            return new Sphere(material: new Material(transparency: 1, refractiveIndex: 1.5));
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

            return new IntersectionList((t1, this), (t2, this));
        }

        protected internal override Vector LocalNormalAt(Point localPoint, Intersection? hit = null)
        {
            Vector localNormal = localPoint - Point.Zero;
            return localNormal;
        }
    }
}
