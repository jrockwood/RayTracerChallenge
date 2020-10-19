// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeProgress.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FakeProgress : IProgress<int>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly List<int> _reportedValues = new List<int>();

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public IReadOnlyList<int> ReportedValues => _reportedValues;
        public int LastReportedValue => _reportedValues.Last();

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public void Report(int value)
        {
            _reportedValues.Add(value);
        }
    }
}
