// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewModelCommand.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Commands
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Inherited <see cref="ICommand"/> with no parameters to <see cref="ICommand.Execute"/> and <see cref="ICommand.CanExecute"/>.
    /// </summary>
    public interface IViewModelCommand : ICommand, INotifyPropertyChanged
    {
        bool IsEnabled { get; set; }

        bool CanExecute();

        void Execute();

        /// <summary>
        /// Conditionally invokes <see cref="Execute"/> if the command is enabled. If the command is not enabled,
        /// nothing happens.
        /// </summary>
        void ExecuteIfEnabled();
    }

    /// <summary>
    /// Type-safe command for use in ViewModels.
    /// </summary>
    /// <typeparam name="T">The type of the parameter that is passed into the methods.</typeparam>
    public interface IViewModelCommand<in T> : ICommand, INotifyPropertyChanged
    {
        bool IsEnabled { get; set; }

        bool CanExecute(T parameter);

        void Execute(T parameter);

        /// <summary>
        /// Conditionally invokes <see cref="Execute"/> if the command is enabled. If the command is not enabled,
        /// nothing happens.
        /// </summary>
        void ExecuteIfEnabled(T parameter);
    }

    /// <summary>
    /// Asynchronous version of <see cref="IViewModelCommand"/>.
    /// </summary>
    public interface IViewModelAsyncCommand : IViewModelCommand
    {
        Task ExecuteAsync(IProgress<int>? progress = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Conditionally invokes <see cref="ExecuteAsync"/> if the command is enabled. If the command is not enabled,
        /// nothing happens.
        /// </summary>
        Task ExecuteAsyncIfEnabled(IProgress<int>? progress = null, CancellationToken cancellationToken = default);
    }
}
