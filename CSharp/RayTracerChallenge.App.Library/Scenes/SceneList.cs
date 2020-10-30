// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneList.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class SceneList : ReadOnlyObservableCollection<Scene>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private int _selectedIndex;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public SceneList()
            : base(CreateScenes())
        {
        }

        //// ===========================================================================================================
        //// Events
        //// ===========================================================================================================

        public event EventHandler? SelectedSceneChanged;

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex == value)
                {
                    return;
                }

                _selectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedIndex)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedScene)));

                SelectedSceneChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Scene? SelectedScene => _selectedIndex >= 0 && _selectedIndex < Count ? Items[_selectedIndex] : null;

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        private static ObservableCollection<Scene> CreateScenes()
        {
            return new ObservableCollection<Scene>
            {
                new Chapter02Cannonball(),
                new Chapter04ClockFace(),
                new Chapter05RedSphere(),
                new Chapter06ShadedSphere(),
                new Chapter07SixSpheres(),
                new Chapter07SixSpheresThreeLights(),
                new Chapter08ShadowPuppets(),
                new Chapter09Planes(),
                new Chapter10Patterns(),
                new Chapter10PerturbedPattern(),
                new Chapter10PerlinPattern(),
                new Chapter11Reflections(),
                new Chapter12TableInARoom(),
                new Chapter13Cylinders(),
                new Chapter13Cones(),
            };
        }
    }
}
