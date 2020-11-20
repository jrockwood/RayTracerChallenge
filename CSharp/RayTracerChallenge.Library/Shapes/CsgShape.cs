// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CsgShape.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents a Constructive Solid Geometry (CSG) shape, which is two shapes that are combined using either a
    /// union, intersection, or difference operation to create a new shape.
    /// </summary>
    public class CsgShape : Shape
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public CsgShape(
            string name,
            CsgOperation operation,
            Shape left,
            Shape right,
            Matrix4x4? transform = null,
            Material? material = null)
            : this(operation, left, right, name, transform, material)
        {
        }

        public CsgShape(
            CsgOperation operation,
            Shape left,
            Shape right,
            string? name = null,
            Matrix4x4? transform = null,
            Material? material = null)
            : base(name, transform, material)
        {
            Operation = operation;
            Left = left;
            Right = right;

            Left.Parent = this;
            Right.Parent = this;

            BoundingBox = new BoundingBox(Point.Zero, Point.Zero);
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public override BoundingBox BoundingBox { get; }

        public CsgOperation Operation { get; }
        public Shape Left { get; }
        public Shape Right { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            var leftIntersections = Left.Intersect(localRay);
            var rightIntersections = Right.Intersect(localRay);
            var combinedIntersections = new IntersectionList(leftIntersections.Concat(rightIntersections));
            var filtered = FilterIntersections(combinedIntersections);
            return filtered;
        }

        protected internal override Vector LocalNormalAt(Point localPoint, Intersection? hit = null)
        {
            throw new NotSupportedException(
                "Intersections should return the shape that was hit, and therefore the normal");
        }

        internal static bool IsIntersectionAllowed(
            CsgOperation operation,
            bool isLeftHit,
            bool isWithinLeft,
            bool isWithinRight)
        {
            return operation switch
            {
                CsgOperation.Union => (isLeftHit && !isWithinRight) || (!isLeftHit && !isWithinLeft),
                CsgOperation.Intersection => (isLeftHit && isWithinRight) || (!isLeftHit && isWithinLeft),
                CsgOperation.Difference => (isLeftHit && !isWithinRight) || (!isLeftHit && isWithinLeft),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
            };
        }

        internal IntersectionList FilterIntersections(IntersectionList intersections)
        {
            // Begin outside of both children.
            bool isInsideLeft = false;
            bool isInsideRight = false;

            // Prepare a list to receive the filtered intersections.
            var result = new IntersectionList();

            foreach (Intersection intersection in intersections)
            {
                // If the intersection's shape is part of the left child, then isLeftHit is true.
                bool isLeftHit = Left == intersection.Shape ||
                                 (Left is CsgShape csgShape &&
                                  (csgShape.Left == intersection.Shape ||
                                   csgShape.Right == intersection.Shape)) ||
                                 (Left is Group group && group.Children.Contains(intersection.Shape));

                if (IsIntersectionAllowed(Operation, isLeftHit, isInsideLeft, isInsideRight))
                {
                    result.Add(intersection);
                }

                // Depending on which object was hit, toggle either isInsideLeft or isInsideRight
                if (isLeftHit)
                {
                    isInsideLeft = !isInsideLeft;
                }
                else
                {
                    isInsideRight = !isInsideRight;
                }
            }

            return result;
        }
    }

    public enum CsgOperation
    {
        Union,
        Intersection,
        Difference,
    }
}
