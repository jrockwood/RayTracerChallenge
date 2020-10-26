// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PerturbedPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    /// <summary>
    /// Uses the Perlin noise function to make the pattern look perturbed.
    /// </summary>
    public class PerturbedPattern : Pattern
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly Perlin _perlin;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public PerturbedPattern(Pattern pattern, double scale = 0.01, int octaves = 1, double persistence = 1)
        {
            Pattern = pattern;
            Scale = scale;
            Octaves = octaves;
            Persistence = persistence;

            _perlin = new Perlin();
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Pattern Pattern { get; }
        public double Scale { get; }
        public int Octaves { get; }
        public double Persistence { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override Color ColorAt(Point point)
        {
            double perlinX = _perlin.CalculateOctavePerlin(point.X, point.Y, point.Z, Octaves, Persistence);
            double perlinY = _perlin.CalculateOctavePerlin(point.X, point.Y, point.Z + 1, Octaves, Persistence);
            double perlinZ = _perlin.CalculateOctavePerlin(point.X, point.Y, point.Z + 2, Octaves, Persistence);

            double newX = point.X + (perlinX * Scale);
            double newY = point.Y + (perlinY * Scale);
            double newZ = point.Z + (perlinZ * Scale);

            Point patternPoint = Pattern.Transform.Invert() * new Point(newX, newY, newZ);
            Color color = Pattern.ColorAt(patternPoint);
            return color;
        }
    }
}
