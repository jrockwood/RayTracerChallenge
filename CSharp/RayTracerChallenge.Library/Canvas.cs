// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Canvas.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    /// <summary>
    /// Represents an immutable canvas that contains a color for every pixel.
    /// </summary>
    public sealed class Canvas : ICanvas
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private ImmutableArray<Color> _pixels;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        internal Canvas(int width, int height, ImmutableArray<Color> pixels)
        {
            if (pixels.Length != width * height)
            {
                throw new ArgumentException("The pixel array should be size 'width * height'.");
            }

            Width = width;
            Height = height;
            _pixels = pixels;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int Width { get; }
        public int Height { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public Color GetPixel(int x, int y)
        {
            int index = CalculateIndex(x, y);
            return _pixels[index];
        }

        public IEnumerable<Color> GetRow(int y)
        {
            return new CanvasRowEnumerator(y * Width, Width, _pixels);
        }

        private int CalculateIndex(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                throw new IndexOutOfRangeException($"Index is outside the bounds of the canvas: x={x}, y={y}");
            }

            return (y * Width) + x;
        }
    }
}
