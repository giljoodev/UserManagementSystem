using System;
using System.Windows.Input;

namespace UserManagementSystem.UI.Object
{
    public class ParameterizedCommand : ICommand
    {
        protected Action<object> _parameterizedAction;

        public event EventHandler CanExecuteChanged;

        public ParameterizedCommand(Action<object> parameterizedAction)
        {
            _parameterizedAction = parameterizedAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (_parameterizedAction != null)
                {
                    _parameterizedAction(parameter);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
