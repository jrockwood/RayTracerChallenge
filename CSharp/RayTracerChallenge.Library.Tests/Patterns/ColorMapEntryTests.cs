// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorMapEntryTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Patterns
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Patterns;

    public class ColorMapEntryTests
    {
        [Test]
        public void Ctor_should_store_the_key_and_color()
        {
            var entry = new ColorMapEntry(0.33, Colors.Cyan);
            entry.Key.Should().Be(0.33);
            entry.Color.Should().Be(Colors.Cyan);
        }

        [Test]
        public void Ctor_should_throw_if_the_key_is_less_than_0_or_greater_than_1()
        {
            Action action = () => _ = new ColorMapEntry(-0.1, Colors.White);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>().And.ParamName.Should().Be("key");

            action = () => _ = new ColorMapEntry(1.1, Colors.White);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>().And.ParamName.Should().Be("key");
        }

        [Test]
        public void Equals_should_return_true_for_the_same_instance()
        {
            var entry1 = new ColorMapEntry(0.5, Colors.Yellow);
            entry1.Equals(entry1).Should().BeTrue();

            // ReSharper disable EqualExpressionComparison
#pragma warning disable CS1718 // Comparison made to same variable
            (entry1 == entry1).Should().BeTrue();
            (entry1 != entry1).Should().BeFalse();
#pragma warning restore CS1718 // Comparison made to same variable
            // ReSharper restore EqualExpressionComparison
        }

        [Test]
        public void Equals_should_return_true_for_equal_instances()
        {
            var entry1 = new ColorMapEntry(0.5, Colors.Yellow);
            var entry2 = new ColorMapEntry(0.5, Colors.Yellow);
            entry1.Equals(entry2).Should().BeTrue();
            (entry1 == entry2).Should().BeTrue();
            (entry1 != entry2).Should().BeFalse();
        }

        [Test]
        public void Equals_should_return_true_for_almost_equal_instances()
        {
            var entry1 = new ColorMapEntry(0.5, Colors.Yellow);
            var entry2 = new ColorMapEntry(0.500001, Colors.Yellow);
            entry1.Equals(entry2).Should().BeTrue();
            (entry1 == entry2).Should().BeTrue();
            (entry1 != entry2).Should().BeFalse();
        }

        [Test]
        public void GetHashCode_should_return_the_same_hash_code_for_two_equal_instances()
        {
            var entry1 = new ColorMapEntry(0.5, Colors.Yellow);
            var entry2 = new ColorMapEntry(0.5, Colors.Yellow);
            entry1.GetHashCode().Should().Be(entry2.GetHashCode());
        }

        [Test]
        public void GetHashCode_should_return_the_same_hash_code_for_two_almost_equal_instances()
        {
            var entry1 = new ColorMapEntry(0.5, Colors.Yellow);
            var entry2 = new ColorMapEntry(0.500001, Colors.Yellow);
            entry1.GetHashCode().Should().Be(entry2.GetHashCode());
        }
    }
}
