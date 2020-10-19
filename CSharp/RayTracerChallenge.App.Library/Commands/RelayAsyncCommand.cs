// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RelayAsyncCommand.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    internal delegate Task ExecuteCommandFunc(
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Implements an <see cref="IViewModelAsyncCommand"/> interface by calling delegates specified in the constructor.
    /// </summary>
    internal class RelayAsyncCommand : Observable, IViewModelAsyncCommand
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private bool _isEnabled;
        private readonly ExecuteCommandFunc _executeAction;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RelayAsyncCommand(ExecuteCommandFunc executeAction, bool isEnabled = true)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
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
            _executeAction(null, CancellationToken.None);
        }

        public bool CanExecute()
        {
            return _isEnabled;
        }

        void IViewModelCommand.Execute()
        {
            _ = ExecuteAsync(null, CancellationToken.None);
        }

        void IViewModelCommand.ExecuteIfEnabled()
        {
            _ = ExecuteAsyncIfEnabled(null, CancellationToken.None);
        }

        public async Task ExecuteAsync(IProgress<int>? progress = null, CancellationToken cancellationToken = default)
        {
            if (!CanExecute())
            {
                throw new InvalidOperationException("Cannot execute a disabled command");
            }

            await _executeAction(progress, cancellationToken);
        }

        public Task ExecuteAsyncIfEnabled(IProgress<int>? progress = null, CancellationToken cancellationToken = default)
        {
            if (CanExecute())
            {
                return _executeAction(progress, cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}
