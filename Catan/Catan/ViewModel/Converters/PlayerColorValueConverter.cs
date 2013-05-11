using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using Catan.Model;

namespace Catan.ViewModel.Converters
{
	/// <summary>
	/// ValueConvert a DataBinding-hoz
	/// </summary>
	public class PlayerColorValueConverter : IValueConverter
	{
		public PlayerColor ConvertBack(Color value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == Colors.Blue)
				return PlayerColor.Blue;
			if (value == Colors.Red)
				return PlayerColor.Red;
			if (value == Colors.Green)
				return PlayerColor.Green;
			if (value == Colors.Orange)
				return PlayerColor.Orange;

			throw new Exception("Nincs ilyen szín definiálva!");
		}

		public Color Convert(PlayerColor value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value)
			{
				case PlayerColor.Blue:
					return Colors.Blue;
				case PlayerColor.Red:
					return Colors.Red;
				case PlayerColor.Green:
					return Colors.Green;
				case PlayerColor.Orange:
					return Colors.Orange;
				default:
					throw new ArgumentOutOfRangeException("value", "Nincs ilyen szín definiálva!");
			}
		}

		/// <summary>
		/// Converts a value. 
		/// </summary>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		/// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() != typeof(PlayerColor))
				throw new Exception("Nem megfelelő az érték típusa!");

			return Convert((PlayerColor)value, targetType, parameter, culture);
		}

		/// <summary>
		/// Converts a value. 
		/// </summary>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		/// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() != typeof(Color))
				throw new Exception("Nem megfelelő az érték típusa!");

			return ConvertBack((Color)value, targetType, parameter, culture);
		}
	}
}
