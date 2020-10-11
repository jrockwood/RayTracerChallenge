﻿// ---------------------------------------------------------------------------------------------------------------------
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
        protected override IntersectionList LocalIntersect(Ray localRay)
        {
            Vector sphereToRay = localRay.Origin - Point.Zero;

            // Calculate the discriminant using this formula: b^2 - 4ac
            float a = localRay.Direction.Dot(localRay.Direction);
            float b = 2 * localRay.Direction.Dot(sphereToRay);
            float c = sphereToRay.Dot(sphereToRay) - 1;
            float discriminant = (b * b) - (4 * a * c);

            if (discriminant < 0)
            {
                return new IntersectionList();
            }

            float sqrtOfDiscriminant = MathF.Sqrt(discriminant);
            float t1 = (-b - sqrtOfDiscriminant) / (2 * a);
            float t2 = (-b + sqrtOfDiscriminant) / (2 * a);

            return new IntersectionList(new Intersection(t1, this), new Intersection(t2, this));
        }
    }
}
