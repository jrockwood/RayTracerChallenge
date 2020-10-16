// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Observable.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class Observable : INotifyPropertyChanged
    {
        //// ===========================================================================================================
        //// Events
        //// ===========================================================================================================

        public event PropertyChangedEventHandler? PropertyChanged;

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected bool SetProperty<T>(ref T property, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(property, value))
            {
                return false;
            }

            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
