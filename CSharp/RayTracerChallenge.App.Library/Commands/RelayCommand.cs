namespace RayTracerChallenge.App.Library.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Implements an <see cref="ICommand"/> interface by calling delegates specified in the constructor.
    /// </summary>
    internal class RelayCommand<T> : Observable, IViewModelCommand<T>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private bool _isEnabled;
        private readonly Action<T> _executeAction;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RelayCommand(Action<T> executeAction, bool isEnabled = true)
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
            T convertedParameter = ConvertParameter(parameter);
            _executeAction(convertedParameter);
        }

        public bool CanExecute(T parameter)
        {
            return _isEnabled;
        }

        public void Execute(T parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Cannot execute a disabled command");
            }

            _executeAction(parameter);
        }

        public void ExecuteIfEnabled(T parameter)
        {
            if (CanExecute(parameter))
            {
                _executeAction(parameter);
            }
        }

        private static T ConvertParameter(object parameter)
        {
            string? paramAsString = parameter as string;

            // Convert a string to an an enum value.
            if (typeof(T).IsEnum && paramAsString != null)
            {
                return (T)Enum.Parse(typeof(T), paramAsString, ignoreCase: true);
            }

            // Convert a string to a boolean.
            if (typeof(T) == typeof(bool) && paramAsString != null)
            {
                return (T)(object)bool.Parse(paramAsString);
            }

            return (T)parameter;
        }
    }

    /// <summary>
    /// Implements an <see cref="ICommand"/> interface by calling delegates specified in the constructor.
    /// </summary>
    internal class RelayCommand : RelayCommand<object?>, IViewModelCommand
    {
        public RelayCommand(Action executeAction, bool isEnabled = true)
            : base(_ => executeAction(), isEnabled)
        {
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public void Execute()
        {
            Execute(null);
        }

        public void ExecuteIfEnabled()
        {
            ExecuteIfEnabled(null);
        }
    }
}
