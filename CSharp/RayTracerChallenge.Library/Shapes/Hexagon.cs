// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Hexagon.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;
    using System.Collections.Generic;

    public class Hexagon : Group
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Hexagon(string? name = null, Matrix4x4? transform = null, Material? material = null)
            : base(name, transform, material, CreateShapes())
        {
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        private static Shape[] CreateShapes()
        {
            var shapes = new List<Shape>();

            for (int i = 0; i < 6; i++)
            {
                Group side = CreateSide();
                side.Transform = Matrix4x4.CreateRotationY(i * (Math.PI / 3));
                shapes.Add(side);
            }

            return shapes.ToArray();
        }

        private static Sphere CreateCorner()
        {
            var corner = new Sphere("Corner", Matrix4x4.CreateScaling(0.25, 0.25, 0.25).Translate(0, 0, -1));
            return corner;
        }

        private static Cylinder CreateEdge()
        {
            var edge = new Cylinder(
                minimumY: 0,
                maximumY: 1,
                name: "Edge",
                transform:
                Matrix4x4.CreateScaling(0.25, 1, 0.25).RotateZ(-Math.PI / 2).RotateY(-Math.PI / 6).Translate(0, 0, -1));

            return edge;
        }

        private static Group CreateSide()
        {
            var side = new Group("Side", CreateCorner(), CreateEdge());
            return side;
        }
    }
}
