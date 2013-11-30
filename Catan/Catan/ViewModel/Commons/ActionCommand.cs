using System;
using System.Windows.Input;

namespace Catan.ViewModel.Commons
{
    /// <summary>
    /// Akció parancs
    /// </summary>
    public class ActionCommand : ICommand
    {
        /// <summary>
        /// A parancs futásának elõfeltételét tároló függvény
        /// </summary>
        private readonly Predicate<object> _CanExecute;

        /// <summary>
        /// A parancs futását tároló függvény
        /// </summary>
        private readonly Action _Execute;

        /// <summary>
        /// ActionCommand konstruktor
        /// </summary>
        public ActionCommand(Action execute)
            : this(null, execute)
        {

        }

        /// <summary>
        /// ActionCommand konstruktor
        /// </summary>
        public ActionCommand(Predicate<object> canExecute, Action execute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        /// <summary>
        /// Parancs lefuttatása
        /// </summary>
        public void Execute(object parameter)
        {
            _Execute();
        }

        /// <summary>
        /// Igazzal tér vissza, ha le lehet futtatni a parancsot
        /// </summary>
        public bool CanExecute(object parameter)
        {
            if (_CanExecute == null)
                return true;
            return _CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        protected void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null) {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}