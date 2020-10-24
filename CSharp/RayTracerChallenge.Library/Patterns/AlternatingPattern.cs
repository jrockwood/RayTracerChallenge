// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="AlternatingPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    public abstract class AlternatingPattern : Pattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected AlternatingPattern(Color color1, Color color2, Matrix4x4? transform = null)
            : this(new SolidColorPattern(color1), new SolidColorPattern(color2), transform)
        {
        }

        protected AlternatingPattern(Pattern pattern1, Pattern pattern2, Matrix4x4? transform = null)
            : base(transform)
        {
            Pattern1 = pattern1;
            Pattern2 = pattern2;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Pattern Pattern1 { get; }
        public Pattern Pattern2 { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected Point ToPattern1Point(Point patternPoint)
        {
            return Pattern1.Transform.Invert() * patternPoint;
        }

        protected Point ToPattern2Point(Point patternPoint)
        {
            return Pattern2.Transform.Invert() * patternPoint;
        }
    }
}
