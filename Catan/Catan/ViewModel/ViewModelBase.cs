using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Catan.ViewModel
{
	/// <summary>
	/// Nézetmodellek absztrakt ősosztálya
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected ViewModelBase OnPropertyChanged(params Expression<Func<object>>[] expressions)
		{
			foreach (var exp in expressions)
				OnPropertyChanged<object>(exp);
			return this;
		}

		protected ViewModelBase OnPropertyChanged<TPropertyType>(Expression<Func<TPropertyType>> expression)
		{
			var body = expression.Body as MemberExpression;
			if (body == null)
				throw new ArgumentException("Not supported expression!", "expression");

			return OnPropertyChanged(body.Member.Name);
		}

		protected ViewModelBase OnPropertyChanged(params string[] propertyNames)
		{
			foreach (var prop in propertyNames)
				OnPropertyChanged(prop);
			return this;
		}

		protected virtual ViewModelBase OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
			return this;
		}

		public virtual void Refresh()
		{
			foreach (var propertyName in GetType().GetProperties().Select(prop => prop.Name))
				OnPropertyChanged(propertyName);
		}
	}
}
