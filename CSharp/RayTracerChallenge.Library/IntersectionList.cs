// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IntersectionList.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using RayTracerChallenge.Library.Shapes;

    /// <summary>
    /// Represents an immutable list of <see cref="Intersection"/> objects.
    /// </summary>
    public readonly struct IntersectionList : IReadOnlyList<Intersection>, IEquatable<IntersectionList>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public static readonly IntersectionList Empty = new IntersectionList(Enumerable.Empty<Intersection>());

        private readonly ImmutableArray<Intersection> _intersections;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        private IntersectionList(IEnumerable<Intersection> intersections)
        {
            _intersections = intersections.OrderBy(x => x.T).ToImmutableArray();
        }

        private IntersectionList(params Intersection[] intersections)
            : this((IEnumerable<Intersection>)intersections)
        {
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int Count => _intersections.Length;

        public IEnumerable<double> Ts => _intersections.Select(x => x.T);
        public IEnumerable<Shape> Shapes => _intersections.Select(x => x.Shape);

        public Intersection? Hit => _intersections.FirstOrDefault(x => x.T >= 0);

        //// ===========================================================================================================
        //// Indexers
        //// ===========================================================================================================

        public Intersection this[int index] => _intersections[index];

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public static IntersectionList Create(IEnumerable<Intersection> intersections)
        {
            return new IntersectionList(intersections);
        }

        public static IntersectionList Create(params Intersection[] intersections)
        {
            return new IntersectionList(intersections);
        }

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
            return new IntersectionList(_intersections.AddRange(intersections).ToImmutableArray());
        }

        //// ===========================================================================================================
        //// Equality Methods
        //// ===========================================================================================================

        public bool Equals(IntersectionList other)
        {
            return _intersections.Equals(other._intersections);
        }

        public override bool Equals(object? obj)
        {
            return obj is IntersectionList other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _intersections.GetHashCode();
        }

        public static bool operator ==(IntersectionList left, IntersectionList right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntersectionList left, IntersectionList right)
        {
            return !left.Equals(right);
        }
    }
}
