using System;
using System.Globalization;

namespace Catan.ViewModel.Converters
{
	/// <summary>
	/// String-et Int32-vé konvertáló osztály
	/// </summary>
	public class StringToInteger : ValueConverter<string, int>
	{
		public override int Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Value cannot be null!");

			int result;

			if (!int.TryParse(value, out result))
				throw new ArgumentException("Cannot parse string to int!");

			return result;
		}

		public override string ConvertBack(int value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString();
		}
	}
}