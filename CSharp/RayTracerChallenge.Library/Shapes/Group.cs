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
    using System.Linq;

    /// <summary>
    /// Represents a group of shapes that can be transformed together.
    /// </summary>
    public class Group : Shape
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly List<Shape> _children = new List<Shape>();
        private BoundingBox? _boundingBox;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Group(params Shape[] children)
            : this(null, null, null, children)
        {
        }

        public Group(string name, params Shape[] children)
            : this(name, null, null, children)
        {
        }

        public Group(string? name = null, Matrix4x4? transform = null, Material? material = null, params Shape[] children)
            : base(transform, material)
        {
            Name = name;

            foreach (Shape child in children)
            {
                AddChild(child);
            }
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public override BoundingBox BoundingBox => _boundingBox ??= CalculateBoundingBox();

        public IReadOnlyList<Shape> Children => _children;

        public string? Name { get; set; }

        public override Material Material
        {
            get => base.Material;
            set
            {
                base.Material = value;
                ApplyMaterialToChildren();
            }
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public void AddChild(Shape shape)
        {
            _boundingBox = null;
            _children.Add(shape);
            shape.Parent = this;
            shape.Material = Material;
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
            // Try intersecting with the bounding box first before asking each shape in the group.
            if (!BoundingBox.TryLocalIntersect(localRay))
            {
                return IntersectionList.Empty;
            }

            var intersections = new IntersectionList();
            foreach (Shape child in _children)
            {
                var childIntersections = child.Intersect(localRay);
                intersections.AddRange(childIntersections);
            }

            return intersections;
        }

        protected internal override Vector LocalNormalAt(Point localPoint, Intersection? hit = null)
        {
            throw new InvalidOperationException("Groups do not have a local normal.");
        }

        private void ApplyMaterialToChildren()
        {
            foreach (Shape child in _children)
            {
                child.Material = Material;
                if (child is Group g)
                {
                    g.ApplyMaterialToChildren();
                }
            }
        }

        private BoundingBox CalculateBoundingBox()
        {
            double minX = double.PositiveInfinity;
            double minY = double.PositiveInfinity;
            double minZ = double.PositiveInfinity;

            double maxX = double.NegativeInfinity;
            double maxY = double.NegativeInfinity;
            double maxZ = double.NegativeInfinity;

            foreach (Shape child in _children)
            {
                BoundingBox box = child.BoundingBox;
                var p1 = box.MinPoint;
                var p2 = new Point(box.MinPoint.X, box.MinPoint.Y, box.MaxPoint.Z);
                var p3 = new Point(box.MinPoint.X, box.MaxPoint.Y, box.MinPoint.Z);
                var p4 = new Point(box.MinPoint.X, box.MaxPoint.Y, box.MaxPoint.Z);
                var p5 = new Point(box.MaxPoint.X, box.MinPoint.Y, box.MinPoint.Z);
                var p6 = new Point(box.MaxPoint.X, box.MinPoint.Y, box.MaxPoint.Z);
                var p7 = new Point(box.MaxPoint.X, box.MaxPoint.Y, box.MinPoint.Z);
                var p8 = box.MaxPoint;

                Matrix4x4 transform = child.Transform;
                Point tp1 = transform * p1;
                Point tp2 = transform * p2;
                Point tp3 = transform * p3;
                Point tp4 = transform * p4;
                Point tp5 = transform * p5;
                Point tp6 = transform * p6;
                Point tp7 = transform * p7;
                Point tp8 = transform * p8;
                var points = new[] { tp1, tp2, tp3, tp4, tp5, tp6, tp7, tp8 };

                minX = Math.Min(minX, points.Min(p => p.X));
                minY = Math.Min(minY, points.Min(p => p.Y));
                minZ = Math.Min(minZ, points.Min(p => p.Z));

                maxX = Math.Max(maxX, points.Max(p => p.X));
                maxY = Math.Max(maxY, points.Max(p => p.Y));
                maxZ = Math.Max(maxZ, points.Max(p => p.Z));
            }

            return new BoundingBox(new Point(minX, minY, minZ), new Point(maxX, maxY, maxZ));
        }
    }
}
