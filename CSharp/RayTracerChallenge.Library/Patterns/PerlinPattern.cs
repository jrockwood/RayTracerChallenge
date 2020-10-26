// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PerlinPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    public class PerlinPattern : Pattern
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly Perlin _perlin;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public PerlinPattern(
            ColorMap colorMap,
            int octaves = 1,
            double persistence = 1,
            Matrix4x4? transform = null)
            : base(transform)
        {
            ColorMap = colorMap;
            Octaves = octaves;
            Persistence = persistence;

            _perlin = new Perlin();
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public ColorMap ColorMap { get; }
        public int Octaves { get; }
        public double Persistence { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            double key = _perlin.CalculateOctavePerlin(
                Math.Abs(point.X),
                Math.Abs(point.Y),
                Math.Abs(point.Z),
                Octaves,
                Persistence);

            Color color = ColorMap.GetColor(key);
            return color;
        }
    }
}
