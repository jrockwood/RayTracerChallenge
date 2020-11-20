// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SmoothTriangle.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;

    public class SmoothTriangle : Triangle
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public SmoothTriangle(
            Point p1,
            Point p2,
            Point p3,
            Vector n1,
            Vector n2,
            Vector n3,
            string? name = null,
            Matrix4x4? transform = null,
            Material? material = null)
            : base(p1, p2, p3, name, transform, material)
        {
            N1 = n1;
            N2 = n2;
            N3 = n3;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Vector N1 { get; }
        public Vector N2 { get; }
        public Vector N3 { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override string ToString()
        {
            return $"P1 = {P1}, P2 = {P2}, P3 = {P3}, N1 = {N1}, N2 = {N2}, N3 = {N3}";
        }

        protected internal override Vector LocalNormalAt(Point localPoint, Intersection? hit = null)
        {
            if (hit == null)
            {
                throw new ArgumentNullException(nameof(hit));
            }

            if (hit.U == null || hit.V == null)
            {
                throw new InvalidOperationException($"{nameof(hit)} should have u and v values");
            }

            double u = hit.U.Value;
            double v = hit.V.Value;

            Vector normal = (N2 * u) + (N3 * v) + (N1 * (1 - u - v));
            return normal;
        }
    }
}
