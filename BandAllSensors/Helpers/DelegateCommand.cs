using System;
using System.Diagnostics;

namespace AllInOneSensorDemo.UWP.Helpers
{
    // http://codepaste.net/jgxazh

    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-MVVM
    public class DelegateCommand : System.Windows.Input.ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action execute, Func<bool> canexecute = null)
        {
            if (execute == null)
            {
                //throw new ArgumentNullException(nameof(execute));
                return;
            }
            _execute = execute;
            _canExecute = canexecute ?? (() => true);
        }

        [DebuggerStepThrough]
        public bool CanExecute(object p = null)
        {
            try
            {
                return _canExecute();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] " + ex.Message);
                return false;
            }
        }

        public void Execute(object p = null)
        {
            if (!CanExecute(p))
                return;

            try
            {
                _execute();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] " + ex.Message);
                //Debugger.Break();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-MVVM
    public class DelegateCommand<T> : System.Windows.Input.ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<T> execute, Func<T, bool> canexecute = null)
        {
            if (execute == null)
            {
                //throw new ArgumentNullException(nameof(execute));
                return;
            }
            _execute = execute;
            _canExecute = canexecute ?? (e => true);
        }

        [DebuggerStepThrough]
        public bool CanExecute(object p)
        {
            try
            {
                var _Value = (T)Convert.ChangeType(p, typeof(T));
                return _canExecute == null ? true : _canExecute(_Value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] " + ex.Message);
                return false;
            }
        }

        public void Execute(object p)
        {
            if (!CanExecute(p))
                return;
            var _Value = (T)Convert.ChangeType(p, typeof(T));
            _execute(_Value);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
