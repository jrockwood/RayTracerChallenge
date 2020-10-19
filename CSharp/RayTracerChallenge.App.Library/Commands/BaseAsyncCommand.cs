// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseAsyncCommand.cs" company="Justin Rockwood">
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

    /// <summary>
    /// Abstract base class for all reusable asynchronous commands.
    /// </summary>
    public abstract class BaseAsyncCommand : BaseCommand, IViewModelAsyncCommand
    {
        /// <summary>
        /// Executes the asynchronous command. Warning: typically you don't want to call this directly and instead call
        /// <see cref="ExecuteAsync"/> so you can track when the execution is complete. This invocation is only used for
        /// the standard <see cref="ICommand"/> interface implementation.
        /// </summary>
        protected sealed override void ExecuteImpl()
        {
            ExecuteAsync();
        }

        public Task ExecuteAsync(IProgress<int>? progress = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotEnabled();
            return ExecuteAsyncImpl(progress, cancellationToken);
        }

        public Task ExecuteAsyncIfEnabled(
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default)
        {
            if (CanExecute())
            {
                return ExecuteAsyncImpl(progress, cancellationToken);
            }

            return Task.CompletedTask;
        }

        protected abstract Task ExecuteAsyncImpl(
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default);
    }
}
