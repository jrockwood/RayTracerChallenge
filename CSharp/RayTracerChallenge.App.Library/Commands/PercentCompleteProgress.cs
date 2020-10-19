// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PercentCompleteProgress.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Commands
{
    using System;

    public class PercentCompleteProgress : Progress<int>
    {
        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected override void OnReport(int value)
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            base.OnReport(value);
        }
    }
}
