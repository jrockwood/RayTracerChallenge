﻿// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Shape.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;

    /// <summary>
    /// Abstract base class for all geometric shapes.
    /// </summary>
    public abstract class Shape
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private Material _material;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected Shape(string? name = null, Matrix4x4? transform = null, Material? material = null)
        {
            Name = name;
            Transform = transform ?? Matrix4x4.Identity;
            _material = material ?? new Material();
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public abstract BoundingBox BoundingBox { get; }

        public string? Name { get; set; }

        public Matrix4x4 Transform { get; set; }

        public virtual Material Material
        {
            get => _material;
            set => _material = value;
        }

        public bool IsShadowHidden { get; set; }

        public Shape? Parent { get; set; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public Shape ChangeTransform(Matrix4x4 value)
        {
            Transform = value;
            return this;
        }

        public Shape ChangeMaterial(Material value)
        {
            Material = value;
            return this;
        }

        public Shape ChangeMaterial(Func<Material, Material> setter)
        {
            return ChangeMaterial(setter(Material));
        }

        public override string ToString()
        {
            return Name ?? GetType().Name;
        }

        public Point WorldToObject(Point point)
        {
            if (Parent != null)
            {
                point = Parent.WorldToObject(point);
            }

            if (Transform.IsIdentity)
            {
                return point;
            }

            return Transform.Invert() * point;
        }

        public Vector NormalToWorld(Vector normal)
        {
            normal = Transform.Invert().Transpose() * normal;
            normal = normal.Normalize();

            if (Parent != null)
            {
                normal = Parent.NormalToWorld(normal);
            }

            return normal;
        }

        public IntersectionList Intersect(Ray ray)
        {
            if (Transform.IsIdentity)
            {
                return LocalIntersect(ray);
            }

            var localRay = ray.Transform(Transform.Invert());
            return LocalIntersect(localRay);
        }

        protected internal abstract IntersectionList LocalIntersect(Ray localRay);

        public Vector NormalAt(Point worldPoint, Intersection? hit = null)
        {
            Point localPoint = WorldToObject(worldPoint);
            Vector localNormal = LocalNormalAt(localPoint, hit);
            Vector worldNormal = NormalToWorld(localNormal);
            return worldNormal;
        }

        protected internal abstract Vector LocalNormalAt(Point localPoint, Intersection? hit = null);
    }
}
