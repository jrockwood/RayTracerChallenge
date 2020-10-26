// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RadialGradientPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    public class RadialGradientPattern : Pattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RadialGradientPattern(Color color1, Color color2, Matrix4x4? transform = null)
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

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            double distance = Math.Sqrt((point.X * point.X) + (point.Z * point.Z));
            double fraction = distance - Math.Floor(distance);
            Color color = Color1 + ((Color2 - Color1) * fraction);
            return color;
        }
    }
}
