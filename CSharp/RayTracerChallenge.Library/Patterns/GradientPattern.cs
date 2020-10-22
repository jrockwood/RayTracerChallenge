// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GradientPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    public class GradientPattern : Pattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public GradientPattern(Color color1, Color color2, Matrix4x4? transform = null)
            : base(transform)
        {
            Color1 = color1;
            Color2 = color2;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Color Color1 { get; }
        public Color Color2 { get; }

        public GradientPattern WithColor1(Color value)
        {
            return new GradientPattern(value, Color2, Transform);
        }

        public GradientPattern WithColor2(Color value)
        {
            return new GradientPattern(Color1, value, Transform);
        }

        public override Pattern WithTransform(Matrix4x4 value)
        {
            return new GradientPattern(Color1, Color2, value);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            Color distance = Color2 - Color1;
            double fraction = point.X - Math.Floor(point.X);
            Color color = Color1 + (distance * fraction);
            return color;
        }
    }
}
