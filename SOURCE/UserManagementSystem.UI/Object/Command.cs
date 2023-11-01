using System;
using System.Windows.Input;

namespace UserManagementSystem.UI.Object
{
    public class Command : ICommand
    {
        protected Action _action;

        public event EventHandler CanExecuteChanged;

        public Command(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (_action != null)
                {
                    _action();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
