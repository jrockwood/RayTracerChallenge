// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Pattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using RayTracerChallenge.Library.Shapes;

    public abstract class Pattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected Pattern(Matrix4x4? transform = null)
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

        public abstract Color ColorAt(Point point);

        public Color ColorOnShapeAt(Shape shape, Point worldPoint)
        {
            Point shapePoint = shape.Transform.Invert() * worldPoint;
            Point patternPoint = Transform.Invert() * shapePoint;
            Color color = ColorAt(patternPoint);
            return color;
        }
    }
}
