// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter02Cannonball.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System;
    using RayTracerChallenge.Library;

    public sealed class Chapter02Cannonball : Scene
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Chapter02Cannonball()
            : base(
                "Chapter 2 - Cannonball",
                "Traces the trajectory of a cannonball. Tests points, vectors, and the canvas.",
                900,
                550)
        {
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected override Canvas Render(CameraRenderOptions options)
        {
            var start = new Point(0, 1, 0);
            var velocity = new Vector(1, 1.8, 0).Normalize() * 11.25;
            var cannonball = new Projectile(start, velocity);

            // gravity is -0.1 unit/tick and wind is -0.01 unit/tick
            var gravity = new Vector(0, -0.1, 0);
            var wind = new Vector(-0.01, 0, 0);
            var environment = new Environment(gravity, wind);

            var color = Colors.Magenta;
            const int pixelBorderSize = 2;

            var canvas = new MutableCanvas(CanvasWidth, CanvasHeight);
            while (cannonball.Position.Y > 0)
            {
                cannonball = Tick(environment, cannonball);
                int pointX = (int)Math.Round(cannonball.Position.X);
                int pointY = (int)Math.Round(canvas.Height - cannonball.Position.Y);
                canvas.FillRect(
                    top: pointY - pixelBorderSize,
                    left: pointX - pixelBorderSize,
                    bottom: pointY + pixelBorderSize,
                    right: pointX + pixelBorderSize,
                    color);
            }

            return canvas.ToImmutable();
        }

        private static Projectile Tick(Environment environment, Projectile projectile)
        {
            Point position = projectile.Position + projectile.Velocity;
            Vector velocity = projectile.Velocity + environment.Gravity + environment.Wind;
            return new Projectile(position, velocity);
        }

        //// ===========================================================================================================
        //// Classes
        //// ===========================================================================================================

        private readonly struct Projectile
        {
            public readonly Point Position;
            public readonly Vector Velocity;

            public Projectile(Point position, Vector velocity)
            {
                Position = position;
                Velocity = velocity;
            }
        }

        private readonly struct Environment
        {
            public readonly Vector Gravity;
            public readonly Vector Wind;

            public Environment(Vector gravity, Vector wind)
            {
                Gravity = gravity;
                Wind = wind;
            }
        }
    }
}
