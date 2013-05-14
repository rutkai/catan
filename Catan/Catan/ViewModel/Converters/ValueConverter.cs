using System;
using System.Globalization;
using System.Windows.Data;

namespace Catan.ViewModel.Converters
{
	/// <summary>
	/// Generikus absztrakt ValueConverter osztály
	/// </summary>
	public abstract class ValueConverter<TSource, TTarget> : IValueConverter
	{
		public abstract TTarget Convert(TSource value, Type targetType, object parameter, CultureInfo culture);
		public abstract TSource ConvertBack(TTarget value, Type targetType, object parameter, CultureInfo culture);

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert((TSource)value, targetType, parameter, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ConvertBack((TTarget) value, targetType, parameter, culture);
		}
	}
}