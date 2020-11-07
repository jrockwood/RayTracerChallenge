// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Triangle.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    public class Triangle : Shape
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Triangle(Point p1, Point p2, Point p3, Matrix4x4? transform = null, Material? material = null)
            : base(transform, material)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;

            E1 = p2 - p1;
            E2 = p3 - p1;
            Normal = E2.Cross(E1).Normalize();

            BoundingBox = new BoundingBox(Point.Zero, Point.Zero);
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Point P1 { get; }
        public Point P2 { get; }
        public Point P3 { get; }

        public Vector E1 { get; }
        public Vector E2 { get; }

        public Vector Normal { get; }

        public override BoundingBox BoundingBox { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override string ToString()
        {
            return $"P1 = {P1}, P2 = {P2}, P3 = {P3}";
        }

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            Vector directionCrossE2 = localRay.Direction.Cross(E2);
            double determinant = E1.Dot(directionCrossE2);

            if (determinant.IsApproximatelyEqual(0))
            {
                return IntersectionList.Empty;
            }

            double f = 1.0 / determinant;
            Vector p1ToOrigin = localRay.Origin - P1;
            double u = f * p1ToOrigin.Dot(directionCrossE2);

            if (u < 0 || u > 1)
            {
                return IntersectionList.Empty;
            }

            Vector originCrossE1 = p1ToOrigin.Cross(E1);
            double v = f * localRay.Direction.Dot(originCrossE1);

            if (v < 0 || u + v > 1)
            {
                return IntersectionList.Empty;
            }

            double t = f * E2.Dot(originCrossE1);
            return new IntersectionList(new Intersection(t, this));
        }

        protected internal override Vector LocalNormalAt(Point localPoint)
        {
            return Normal;
        }
    }
}
