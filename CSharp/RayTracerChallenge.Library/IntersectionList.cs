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
    using System.Linq;
    using RayTracerChallenge.Library.Shapes;

    /// <summary>
    /// Represents a list of <see cref="Intersection"/> objects.
    /// </summary>
    public class IntersectionList : IList<Intersection>, IReadOnlyList<Intersection>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public static readonly IntersectionList Empty = new IntersectionList();

        private readonly List<Intersection> _list;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public IntersectionList()
        {
            _list = new List<Intersection>();
        }

        public IntersectionList(IEnumerable<Intersection> intersections)
        {
            _list = new List<Intersection>(intersections);
            _list.Sort();
        }

        public IntersectionList(params Intersection[] intersections)
            : this((IEnumerable<Intersection>)intersections)
        {
        }

        public IntersectionList(params (double t, Shape shape)[] intersections)
            : this(intersections.Select(tuple => new Intersection(tuple)))
        {
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int Count => _list.Count;
        public bool IsReadOnly => false;

        public IEnumerable<double> Ts => this.Select(x => x.T);
        public IEnumerable<Shape> Shapes => this.Select(x => x.Shape);

        public Intersection? Hit => this.FirstOrDefault(x => x.T >= 0);

        //// ===========================================================================================================
        //// Indexers
        //// ===========================================================================================================

        public Intersection this[int index]
        {
            get => _list[index];
            set => throw new NotSupportedException();
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public IEnumerator<Intersection> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        public void Add(Intersection intersection)
        {
            _list.Add(intersection);
            _list.Sort();
        }

        public void AddRange(IEnumerable<Intersection> intersections)
        {
            _list.AddRange(intersections);
            _list.Sort();
        }

        public void AddRange(params (double t, Shape shape)[] intersections)
        {
            _list.AddRange(intersections.Select(x => new Intersection(x.t, x.shape)));
            _list.Sort();
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(Intersection intersection)
        {
            return _list.Contains(intersection);
        }

        public void CopyTo(Intersection[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int IndexOf(Intersection intersection)
        {
            return _list.IndexOf(intersection);
        }

        public void Insert(int index, Intersection item)
        {
            throw new NotSupportedException();
        }

        public bool Remove(Intersection item)
        {
            return _list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }
    }
}
