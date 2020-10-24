// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GradientPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    public class GradientPattern : AlternatingPattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public GradientPattern(Color color1, Color color2, Matrix4x4? transform = null)
            : base(color1, color2, transform)
        {
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
