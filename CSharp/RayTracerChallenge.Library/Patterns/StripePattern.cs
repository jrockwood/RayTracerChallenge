// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="StripePattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    public class StripePattern : AlternatingPattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public StripePattern(Color color1, Color color2, Matrix4x4? transform = null)
            : base(color1, color2, transform)
        {
        }

        public StripePattern(Pattern pattern1, Pattern pattern2, Matrix4x4? transform = null)
            : base(pattern1, pattern2, transform)
        {
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            return Math.Floor(point.X + NumberExtensions.Epsilon) % 2 == 0
                ? Pattern1.ColorAt(ToPattern1Point(point))
                : Pattern2.ColorAt(ToPattern2Point(point));
        }
    }
}
