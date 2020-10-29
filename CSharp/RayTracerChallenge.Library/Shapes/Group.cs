// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Group.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a group of shapes that can be transformed together.
    /// </summary>
    public class Group : Shape
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly List<Shape> _children = new List<Shape>();

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Group(params Shape[] children)
        {
            foreach (Shape child in children)
            {
                AddChild(child);
            }
        }

        public Group(Matrix4x4 transform, params Shape[] children)
            : base(transform)
        {
            foreach (Shape child in children)
            {
                AddChild(child);
            }
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public IReadOnlyList<Shape> Children => _children;

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public void AddChild(Shape shape)
        {
            _children.Add(shape);
            shape.Parent = this;
        }

        public void AddChildren(params Shape[] shapes)
        {
            foreach (Shape shape in shapes)
            {
                AddChild(shape);
            }
        }

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            var intersections = new IntersectionList();

            foreach (Shape child in _children)
            {
                var childIntersections = child.Intersect(localRay);
                intersections.AddRange(childIntersections);
            }

            return intersections;
        }

        protected internal override Vector LocalNormalAt(Point localPoint)
        {
            throw new InvalidOperationException("Groups do not have a local normal");
        }
    }
}
