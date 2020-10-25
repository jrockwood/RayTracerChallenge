// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="BlendedPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    /// <summary>
    /// Pattern that takes two other patterns and blends the colors together.
    /// </summary>
    public class BlendedPattern : AlternatingPattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public BlendedPattern(Color color1, Color color2, double color1Percent, Matrix4x4? transform = null)
            : this(new SolidColorPattern(color1), new SolidColorPattern(color2), color1Percent, transform)
        {
        }

        public BlendedPattern(Pattern pattern1, Pattern pattern2, double color1Percent, Matrix4x4? transform = null)
            : base(pattern1, pattern2, transform)
        {
            if (color1Percent < 0 || color1Percent > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(color1Percent),
                    color1Percent,
                    "Percentage must be between 0 and 1, inclusive");
            }

            Color1Percent = color1Percent;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double Color1Percent { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            Color color1 = Pattern1.ColorAt(ToPattern1Point(point));
            Color color2 = Pattern2.ColorAt(ToPattern2Point(point));
            Color color = (color1 * Color1Percent) + (color2 * (1 - Color1Percent));
            return color;
        }
    }
}
