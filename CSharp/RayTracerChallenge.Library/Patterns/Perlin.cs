// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Perlin.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    /// <summary>
    /// Implementation of the Improved Perlin Noise algorithm for generating procedural content that looks natural.
    /// </summary>
    /// <remarks>Taken almost verbatim from <see href="http://adrianb.io/2014/08/09/perlinnoise.html"/>.</remarks>
    public sealed class Perlin
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        // Hash lookup table as defined by Ken Perlin. This is a randomly arranged array of all numbers from 0-255 inclusive.
        private static readonly int[] s_permutation =
        {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        private static readonly int[] s_p;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        static Perlin()
        {
            s_p = new int[512];

            for (int x = 0; x < 512; x++)
            {
                s_p[x] = s_permutation[x % 256];
            }
        }

        public Perlin(int repeat = -1)
        {
            Repeat = repeat;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int Repeat { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public double CalculateOctavePerlin(double x, double y, double z, int octaves, double persistence)
        {
            double total = 0;
            double frequency = 1;
            double amplitude = 1;

            // Used for normalizing the result to 0.0 - 1.0
            double maxValue = 0;

            for (int i = 0; i < octaves; i++)
            {
                total += CalculatePerlin(x * frequency, y * frequency, z * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }

        public double CalculatePerlin(double x, double y, double z)
        {
            // If we have any repeat on, change the coordinates to their "local" repetitions
            if (Repeat > 0)
            {
                x %= Repeat;
                y %= Repeat;
                z %= Repeat;
            }

            // Calculate the "unit cube" that the point asked will be located in The left bound is ( |_x_|,|_y_|,|_z_| )
            // and the right bound is that plus 1. Next we calculate the location (from 0.0 to 1.0) in that cube. We
            // also fade the location to smooth the result.

            int xi = (int)x & 255;
            int yi = (int)y & 255;
            int zi = (int)z & 255;
            double xf = x - (int)x;
            double yf = y - (int)y;
            double zf = z - (int)z;
            double u = Fade(xf);
            double v = Fade(yf);
            double w = Fade(zf);

            int aaa = s_p[s_p[s_p[xi] + yi] + zi];
            int aba = s_p[s_p[s_p[xi] + Inc(yi)] + zi];
            int aab = s_p[s_p[s_p[xi] + yi] + Inc(zi)];
            int abb = s_p[s_p[s_p[xi] + Inc(yi)] + Inc(zi)];
            int baa = s_p[s_p[s_p[Inc(xi)] + yi] + zi];
            int bba = s_p[s_p[s_p[Inc(xi)] + Inc(yi)] + zi];
            int bab = s_p[s_p[s_p[Inc(xi)] + yi] + Inc(zi)];
            int bbb = s_p[s_p[s_p[Inc(xi)] + Inc(yi)] + Inc(zi)];

            // The gradient function calculates the dot product between a pseudorandom gradient vector and the vector
            // from the input coordinate to the 8 surrounding points in its unit cube. This is all then lerped together
            // as a sort of weighted average based on the faded (u,v,w) values we made earlier.
            double x1 = LinearInterpolate(Gradient(aaa, xf, yf, zf), Gradient(baa, xf - 1, yf, zf), u);
            double x2 = LinearInterpolate(Gradient(aba, xf, yf - 1, zf), Gradient(bba, xf - 1, yf - 1, zf), u);
            double y1 = LinearInterpolate(x1, x2, v);

            x1 = LinearInterpolate(Gradient(aab, xf, yf, zf - 1), Gradient(bab, xf - 1, yf, zf - 1), u);
            x2 = LinearInterpolate(Gradient(abb, xf, yf - 1, zf - 1), Gradient(bbb, xf - 1, yf - 1, zf - 1), u);
            double y2 = LinearInterpolate(x1, x2, v);

            // For convenience we bound it to 0 - 1 (theoretical min/max before is -1 - 1)
            return (LinearInterpolate(y1, y2, w) + 1) / 2;
        }

        private int Inc(int num)
        {
            num++;
            if (Repeat > 0)
            {
                num %= Repeat;
            }

            return num;
        }

        // Source: http://riven8192.blogspot.com/2010/08/calculate-perlinnoise-twice-as-fast.html
        private static double Gradient(int hash, double x, double y, double z)
        {
            return (hash & 0xF) switch
            {
                0x0 => +x + y,
                0x1 => -x + y,
                0x2 => +x - y,
                0x3 => -x - y,
                0x4 => +x + z,
                0x5 => -x + z,
                0x6 => +x - z,
                0x7 => -x - z,
                0x8 => +y + z,
                0x9 => -y + z,
                0xA => +y - z,
                0xB => -y - z,
                0xC => +y + x,
                0xD => -y + z,
                0xE => +y - x,
                0xF => -y - z,
                _ => throw new InvalidOperationException(),
            };
        }

        // Fade function as defined by Ken Perlin. This eases coordinate values so that they will "ease" towards
        // integral values. This ends up smoothing the final output.
        private static double Fade(double t)
        {
            // 6t^5 - 15t^4 + 10t^3
            double result = t * t * t * ((t * ((t * 6) - 15)) + 10);
            return result;
        }

        private static double LinearInterpolate(double a, double b, double x)
        {
            return a + (x * (b - a));
        }
    }
}
