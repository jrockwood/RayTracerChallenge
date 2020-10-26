// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckerPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    public class CheckerPattern : AlternatingPattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public CheckerPattern(Color color1, Color color2, Matrix4x4? transform = null)
            : base(color1, color2, transform)
        {
        }

        public CheckerPattern(Pattern pattern1, Pattern pattern2, Matrix4x4? transform = null)
            : base(pattern1, pattern2, transform)
        {
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            double x = Math.Floor(point.X + NumberExtensions.Epsilon);
            double y = Math.Floor(point.Y + NumberExtensions.Epsilon);
            double z = Math.Floor(point.Z + NumberExtensions.Epsilon);
            return (x + y + z) % 2 == 0
                ? Pattern1.ColorAt(ToPattern1Point(point))
                : Pattern2.ColorAt(ToPattern2Point(point));
        }
    }
}
