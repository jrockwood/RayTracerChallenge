// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IntersectionList.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using RayTracerChallenge.Library.Shapes;

    /// <summary>
    /// Represents an immutable list of <see cref="Intersection"/> objects.
    /// </summary>
    public sealed class IntersectionList : IReadOnlyList<Intersection>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private ImmutableArray<Intersection> _intersections;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public IntersectionList(params Intersection[] intersections)
        {
            _intersections = intersections.OrderBy(x => x.T).ToImmutableArray();
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int Count => _intersections.Length;

        public IEnumerable<float> Ts => _intersections.Select(x => x.T);
        public IEnumerable<Shape> Shapes => _intersections.Select(x => x.Shape);

        public Intersection? Hit => _intersections.FirstOrDefault(x => x.T >= 0);

        //// ===========================================================================================================
        //// Indexers
        //// ===========================================================================================================

        public Intersection this[int index] => _intersections[index];

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public IEnumerator<Intersection> GetEnumerator()
        {
            return ((IEnumerable<Intersection>)_intersections).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IntersectionList Add(params Intersection[] intersections)
        {
            return new IntersectionList(_intersections.AddRange(intersections).ToArray());
        }
    }
}
