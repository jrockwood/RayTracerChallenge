// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SolidColorPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    /// <summary>
    /// Pattern that returns the same color for every point.
    /// </summary>
    public class SolidColorPattern : Pattern
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public SolidColorPattern(Color color, Matrix4x4? transform = null)
            : base(transform)
        {
            Color = color;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Color Color { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            return Color;
        }
    }
}
