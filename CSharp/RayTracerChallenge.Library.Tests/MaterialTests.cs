// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MaterialTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Patterns;
    using RayTracerChallenge.Library.Shapes;

    public class MaterialTests
    {
        [Test]
        public void The_default_material()
        {
            var m = new Material();
            m.Color.Should().Be(Material.DefaultColor);
            m.Ambient.Should().Be(Material.DefaultAmbient);
            m.Diffuse.Should().Be(Material.DefaultDiffuse);
            m.Specular.Should().Be(Material.DefaultSpecular);
            m.Shininess.Should().Be(Material.DefaultShininess);
        }

        [Test]
        public void Ctor_should_throw_on_negative_values()
        {
            Action action = () => _ = new Material(ambient: -1);
            action.Should().ThrowExactly<ArgumentException>().And.ParamName.Should().Be("ambient");

            action = () => _ = new Material(diffuse: -1);
            action.Should().ThrowExactly<ArgumentException>().And.ParamName.Should().Be("diffuse");

            action = () => _ = new Material(specular: -1);
            action.Should().ThrowExactly<ArgumentException>().And.ParamName.Should().Be("specular");

            action = () => _ = new Material(shininess: -1);
            action.Should().ThrowExactly<ArgumentException>().And.ParamName.Should().Be("shininess");
        }

        [Test]
        public void WithX_methods_should_return_a_new_object_with_the_value_changed()
        {
            var m = new Material();
            m.WithColor(Colors.Magenta).Should().BeEquivalentTo(new Material(Colors.Magenta));
            m.WithAmbient(2).Should().BeEquivalentTo(new Material(ambient: 2));
            m.WithDiffuse(3).Should().BeEquivalentTo(new Material(diffuse: 3));
            m.WithSpecular(4).Should().BeEquivalentTo(new Material(specular: 4));
            m.WithShininess(5).Should().BeEquivalentTo(new Material(shininess: 5));
        }

        //// ===========================================================================================================
        //// CalculateLighting Tests
        //// ===========================================================================================================

        [Test]
        public void Lighting_with_the_eye_between_the_light_and_the_surface()
        {
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), Colors.White);
            Color color = new Material().CalculateLighting(new Sphere(), light, Point.Zero, eye, normal, false);
            color.Should().Be(new Color(1.9, 1.9, 1.9));
        }

        [Test]
        public void Lighting_with_the_eye_between_light_and_surface_with_the_eye_offset_45_degrees()
        {
            var eye = new Vector(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), Colors.White);
            Color color = new Material().CalculateLighting(new Sphere(), light, Point.Zero, eye, normal, false);
            color.Should().Be(new Color(1.0, 1.0, 1.0));
        }

        [Test]
        public void Lighting_with_the_eye_opposite_the_surface_with_light_offset_45_degrees()
        {
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 10, -10), Colors.White);
            Color color = new Material().CalculateLighting(new Sphere(), light, Point.Zero, eye, normal, false);
            color.Should().Be(new Color(0.7364, 0.7364, 0.7364));
        }

        [Test]
        public void Lighting_with_the_eye_in_the_path_of_the_reflection_vector()
        {
            var eye = new Vector(0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 10, -10), Colors.White);
            Color color = new Material().CalculateLighting(new Sphere(), light, Point.Zero, eye, normal, false);
            color.Should().Be(new Color(1.63639, 1.63639, 1.63639));
        }

        [Test]
        public void Lighting_with_the_light_behind_the_surface()
        {
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, 10), Colors.White);
            Color color = new Material().CalculateLighting(new Sphere(), light, Point.Zero, eye, normal, false);
            color.Should().Be(new Color(0.1, 0.1, 0.1));
        }

        [Test]
        public void Lighting_with_the_surface_in_shadow()
        {
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), Colors.White);
            Color color = new Material().CalculateLighting(new Sphere(), light, Point.Zero, eye, normal, isInShadow: true);
            color.Should().Be(new Color(0.1, 0.1, 0.1));
        }

        [Test]
        public void Lighting_with_a_pattern_applied()
        {
            var material = new Material(
                pattern: new StripePattern(Colors.White, Colors.Black),
                ambient: 1,
                diffuse: 0,
                specular: 0);
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), Colors.White);
            Color color1 = material.CalculateLighting(new Sphere(), light, new Point(0.9, 0, 0), eye, normal, false);
            Color color2 = material.CalculateLighting(new Sphere(), light, new Point(1.1, 0, 0), eye, normal, false);
            color1.Should().Be(Colors.White);
            color2.Should().Be(Colors.Black);
        }
    }
}
