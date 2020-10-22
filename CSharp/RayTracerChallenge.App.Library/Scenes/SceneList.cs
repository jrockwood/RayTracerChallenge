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
                new Chapter2Cannonball(),
                new Chapter4ClockFace(),
                new Chapter5RedSphere(),
                new Chapter6ShadedSphere(),
                new Chapter7SixSpheres(),
                new Chapter8ShadowPuppets(),
                new Chapter9Planes(),
                new Chapter10Patterns(),
            };
        }
    }
}
