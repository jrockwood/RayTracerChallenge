// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="StripePattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    public class StripePattern : Pattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public StripePattern(Color color1, Color color2, Matrix4x4? transform = null)
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

        public StripePattern WithColor1(Color value)
        {
            return new StripePattern(value, Color2, Transform);
        }

        public StripePattern WithColor2(Color value)
        {
            return new StripePattern(Color1, value, Transform);
        }

        public override Pattern WithTransform(Matrix4x4 value)
        {
            return new StripePattern(Color1, Color2, value);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            return Math.Floor(point.X) % 2 == 0 ? Color1 : Color2;
        }
    }
}
