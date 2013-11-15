using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Catan.Model;

namespace Catan.ViewModel.Converters
{
	/// <summary>
	/// Nyersanyagot karakterliterállá konvertáló osztály
	/// </summary>
	public class MaterialToStringConverter : IValueConverter
	{
		string Convert(Material material, Type targetType, object parameter, CultureInfo culture)
		{
			switch (material)
			{
				case Material.Wood:
					return "Fa";
				case Material.Clay:
					return "Agyag";
				case Material.Iron:
					return "Vas";
				case Material.Wheat:
					return "Búza";
				case Material.Wool:
					return "Gyapjú";
				default:
					throw new ArgumentOutOfRangeException("material");
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
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return Material.Clay;

            if (!(value is Material))
                throw new Exception("Nem megfelelő típusú a konvertálandó objektum!");
         
			return Convert((Material)value, targetType, parameter, culture);
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
			throw new NotImplementedException();
		}
	}
}
