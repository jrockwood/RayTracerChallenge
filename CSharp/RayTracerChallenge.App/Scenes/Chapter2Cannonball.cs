// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter2Cannonball.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Scenes
{
    using System;
    using RayTracerChallenge.Library;

    public sealed class Chapter2Cannonball : Scene
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Chapter2Cannonball()
            : base("Chapter 2 - Cannonball", "Traces the trajectory of a cannonball. Tests points, vectors, and the canvas.")
        {
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public override int RequestedWidth => 900;
        public override int RequestedHeight => 550;

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override void Render(Canvas canvas)
        {
            var start = new Point(0, 1, 0);
            var velocity = new Vector(1, 1.8f, 0).Normalize() * 11.25f;
            var cannonball = new Projectile(start, velocity);

            // gravity is -0.1 unit/tick and wind is -0.01 unit/tick
            var gravity = new Vector(0, -0.1f, 0);
            var wind = new Vector(-0.01f, 0, 0);
            var environment = new Environment(gravity, wind);

            var color = Colors.Magenta;
            const int pixelBorderSize = 2;

            while (cannonball.Position.Y > 0)
            {
                cannonball = Tick(environment, cannonball);
                int pointX = (int)MathF.Round(cannonball.Position.X);
                int pointY = (int)MathF.Round(canvas.Height - cannonball.Position.Y);
                canvas.FillRect(
                    top: pointY - pixelBorderSize,
                    left: pointX - pixelBorderSize,
                    bottom: pointY + pixelBorderSize,
                    right: pointX + pixelBorderSize,
                    color);
            }
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
