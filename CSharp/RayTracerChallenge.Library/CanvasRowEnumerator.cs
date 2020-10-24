// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasRowEnumerator.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
//

namespace RayTracerChallenge.Library
{
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class CanvasRowEnumerator : IEnumerable<Color>
    {
        private readonly int _rowStartIndex;
        private readonly int _rowEndIndex;
        private readonly IReadOnlyList<Color> _pixels;

        public CanvasRowEnumerator(int rowStartIndex, int width, IReadOnlyList<Color> pixels)
        {
            _rowStartIndex = rowStartIndex;
            _rowEndIndex = _rowStartIndex + width;
            _pixels = pixels;
        }

        public IEnumerator<Color> GetEnumerator()
        {
            for (int index = _rowStartIndex; index < _rowEndIndex; index++)
            {
                yield return _pixels[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
