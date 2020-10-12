// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneList.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Scenes
{
    using System.Collections.Generic;

    public class SceneList
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public SceneList()
        {
            Scenes = new List<Scene>
            {
                new Chapter2Cannonball(),
                new Chapter4ClockFace(),
                new Chapter5RedSphere(),
                new Chapter6ShadedSphere(),
            };
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public IReadOnlyList<Scene> Scenes { get; }
    }
}
