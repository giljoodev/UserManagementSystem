using System;
using System.Threading;
using System.Windows.Input;

namespace UserManagementSystem.UI.Object
{
    //새로운 Thread를 만들어 Action을 수행하는 Command
    public class GenerateThreadCommand : ICommand
    {
        private Thread _thread;
        private Action _action;
        private object _lockObject;

        public event EventHandler CanExecuteChanged;

        public GenerateThreadCommand(Action action, object lockObject)
        {
            _action     = action;
            _lockObject = lockObject;
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
                    lock (_lockObject)
                    {
                        _thread = new Thread(() =>
                        {
                            // 생성된 스레드에서 실행될 동작(Action)을 정의합니다.
                            _action();
                        });

                        _thread.Start();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
