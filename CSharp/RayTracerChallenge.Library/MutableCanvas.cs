// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MutableCanvas.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Represents a mutable canvas that contains a color for every pixel.
    /// </summary>
    public class MutableCanvas : ICanvas
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly Color[] _pixels;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public MutableCanvas(int width, int height)
        {
            Width = width;
            Height = height;

            _pixels = Enumerable.Repeat(Colors.Black, width * height).ToArray();
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int Width { get; }
        public int Height { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public Canvas ToImmutable()
        {
            return DoInReadLock(() => new Canvas(Width, Height, _pixels.ToImmutableArray()));
        }

        public Color GetPixel(int x, int y)
        {
            return DoInReadLock(
                () =>
                {
                    int index = CalculateIndex(x, y);
                    Color pixel = _pixels[index];
                    return pixel;
                });
        }

        public void SetPixel(int x, int y, Color color)
        {
            DoInWriteLock(
                () =>
                {
                    int index = CalculateIndex(x, y);
                    _pixels[index] = color;
                });
        }

        public IEnumerable<Color> GetRow(int y)
        {
            return new CanvasRowEnumerator(y * Width, Width, _pixels);
        }

        public void FillRect(int top, int left, int bottom, int right, Color color)
        {
            DoInWriteLock(
                () =>
                {
                    for (int x = Math.Max(0, left); x <= Math.Min(right, Width - 1); x++)
                    {
                        for (int y = Math.Max(0, top); y <= Math.Min(bottom, Height - 1); y++)
                        {
                            int index = CalculateIndex(x, y);
                            _pixels[index] = color;
                        }
                    }
                });
        }

        private int CalculateIndex(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                throw new IndexOutOfRangeException($"Index is outside the bounds of the canvas: x={x}, y={y}");
            }

            return (y * Width) + x;
        }

        private T DoInReadLock<T>(Func<T> func)
        {
            _lock.EnterReadLock();

            try
            {
                return func();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private void DoInWriteLock(Action action)
        {
            _lock.EnterWriteLock();

            try
            {
                action();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
