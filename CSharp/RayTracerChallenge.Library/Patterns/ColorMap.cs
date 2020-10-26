// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorMap.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;
    using System.Collections.Generic;

    public class ColorMap
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private static readonly EntryComparer s_comparer = new EntryComparer();

        private readonly List<ColorMapEntry> _entries = new List<ColorMapEntry>();

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public ColorMap(params ColorMapEntry[] entries)
        {
            AddRange(entries);
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public IReadOnlyList<ColorMapEntry> Entries => _entries;

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public ColorMap Add(ColorMapEntry entry)
        {
            int index = _entries.BinarySearch(entry, s_comparer);
            if (index < 0)
            {
                index = ~index;
            }
            else
            {
                while (_entries[index].Key.IsApproximatelyEqual(entry.Key) && index < _entries.Count)
                {
                    index++;
                }
            }

            _entries.Insert(index, entry);
            return this;
        }

        public ColorMap AddRange(params ColorMapEntry[] entries)
        {
            foreach (ColorMapEntry entry in entries)
            {
                Add(entry);
            }

            return this;
        }

        public Color GetColor(double key)
        {
            if (_entries.Count == 0)
            {
                return Colors.Black;
            }

            if (key < _entries[0].Key)
            {
                Color color = Interpolate(0, _entries[0].Key, key, Colors.Black, _entries[0].Color);
                return color;
            }

            if (key > _entries[^1].Key)
            {
                Color color = Interpolate(_entries[^1].Key, 1, key, _entries[^1].Color, Colors.Black);
                return color;
            }

            for (int i = 0; i < _entries.Count - 1; i++)
            {
                double key1 = _entries[i].Key;
                double key2 = _entries[i + 1].Key;

                if (key >= key1 && key <= key2)
                {
                    Color color = Interpolate(key1, key2, key, _entries[i].Color, _entries[i + 1].Color);
                    return color;
                }
            }

            return Colors.Black;
        }

        private static double LinearInterpolate(double x, double x0, double x1, double y0, double y1)
        {
            double deltaX = x1 - x0;
            if (Math.Abs(deltaX) < 1e-4)
            {
                return (y0 + y1) / 2;
            }

            return y0 + (((x - x0) * (y1 - y0)) / deltaX);
        }

        private static Color Interpolate(double key1, double key2, double key, Color color1, Color color2)
        {
            double red = LinearInterpolate(key, key1, key2, color1.Red, color2.Red);
            double green = LinearInterpolate(key, key1, key2, color1.Green, color2.Green);
            double blue = LinearInterpolate(key, key1, key2, color1.Blue, color2.Blue);

            return new Color(red, green, blue);
        }

        //// ===========================================================================================================
        //// Classes
        //// ===========================================================================================================

        private sealed class EntryComparer : IComparer<ColorMapEntry>
        {
            public int Compare(ColorMapEntry? x, ColorMapEntry? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (y is null)
                {
                    return 1;
                }

                if (x is null)
                {
                    return -1;
                }

                return x.Key.IsApproximatelyEqual(y.Key) ? 0 : x.Key.CompareTo(y.Key);
            }
        }
    }
}
