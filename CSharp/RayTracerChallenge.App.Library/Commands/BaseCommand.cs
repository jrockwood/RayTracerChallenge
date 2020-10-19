// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCommand.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Abstract base class for all reusable synchronous commands.
    /// </summary>
    public abstract class BaseCommand : Observable, IViewModelCommand
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private bool _isEnabled;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected BaseCommand(bool isEnabled = true)
        {
            _isEnabled = isEnabled;
        }

        //// ===========================================================================================================
        //// Events
        //// ===========================================================================================================

        public event EventHandler? CanExecuteChanged;

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (SetProperty(ref _isEnabled, value))
                {
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        bool ICommand.CanExecute(object parameter)
        {
            return _isEnabled;
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }

        public bool CanExecute()
        {
            return _isEnabled;
        }

        public void Execute()
        {
            ThrowIfNotEnabled();
            ExecuteImpl();
        }

        public void ExecuteIfEnabled()
        {
            if (CanExecute())
            {
                ExecuteImpl();
            }
        }

        protected abstract void ExecuteImpl();

        protected void ThrowIfNotEnabled()
        {
            if (!CanExecute())
            {
                throw new InvalidOperationException("Cannot execute a disabled command");
            }
        }
    }
}
