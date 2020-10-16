// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.ViewModels
{
    using RayTracerChallenge.App.Library.Scenes;

    public class MainWindowViewModel : Observable
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public MainWindowViewModel()
        {
            Scenes = new SceneList();
            Scenes.SelectedIndex = Scenes.Count > 0 ? Scenes.Count - 1 : -1;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public SceneList Scenes { get; }
    }
}
