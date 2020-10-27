// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RingPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    public class RingPattern : AlternatingPattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RingPattern(Color color1, Color color2, Matrix4x4? transform = null)
            : base(color1, color2, transform)
        {
        }

        public RingPattern(Pattern pattern1, Pattern pattern2, Matrix4x4? transform = null)
            : base(pattern1, pattern2, transform)
        {
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            double distance = Math.Sqrt((point.X * point.X) + (point.Z * point.Z));
            return Math.Floor(distance + NumberExtensions.Epsilon) % 2 == 0
                ? Pattern1.ColorAt(ToPattern1Point(point))
                : Pattern2.ColorAt(ToPattern2Point(point));
        }
    }
}
