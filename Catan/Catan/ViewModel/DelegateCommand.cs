using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Catan.ViewModel
{
	public class ActionCommand : ICommand
	{
		private readonly Predicate<object> _CanExecute;
		private readonly Action _Execute;

		public ActionCommand(Action execute, Predicate<object> canExecute = null)
		{
			_Execute = execute;
			_CanExecute = canExecute;
		}

		public void Execute(object parameter)
		{
			_Execute();
		}

		public bool CanExecute(object parameter)
		{
			if (_CanExecute == null)
				return true;
			return _CanExecute(parameter);
		}

		public event EventHandler CanExecuteChanged;

		protected void OnCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}
	}

	public class DelegateCommand<T> : ICommand
	{
		private readonly Predicate<T> _canExecute;
		private readonly Action<T> _execute;

		void ICommand.Execute(object parameter)
		{
			Execute((T)parameter);
		}

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute((T)parameter);
		}

		public event EventHandler CanExecuteChanged;

		public DelegateCommand(Action<T> execute)
			: this(execute, null)
		{
		}

		public DelegateCommand(Action<T> execute,
					   Predicate<T> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(T parameter)
		{
			if (_canExecute == null)
			{
				return true;
			}

			return _canExecute(parameter);
		}

		public void Execute(T parameter)
		{
			_execute(parameter);
		}

		protected void OnCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}
	}

}
