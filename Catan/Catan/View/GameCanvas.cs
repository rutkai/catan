using System;
using System.Windows;
using System.Windows.Controls;
using Catan.Common;

namespace Catan.View
{
	/// <summary>
	/// Játéktábla felülete
	/// </summary>
	public class GameCanvas : Canvas
	{
		/// <summary>
		/// Maximálisan megjelenített sorok száma
		/// </summary>
		public int Rows
		{
			get { return (int)GetValue(RowsProperty); }
			set { SetValue(RowsProperty, value); }
		}

		/// <summary>
		/// Sorokhoz tartozó dependency property
		/// </summary>
		public static readonly DependencyProperty RowsProperty =
			DependencyProperty.Register("Rows", typeof(int), typeof(GameCanvas), new PropertyMetadata(1, (obj, e) =>
			{
				var canvas = obj as GameCanvas;
				canvas.InvalidateArrange();
			}));

		/// <summary>
		/// Maximálisan megjelenített oszlopok száma
		/// </summary>
		public int Columns
		{
			get { return (int)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}

		/// <summary>
		/// Oszlopokhoz tartozó dependency property
		/// </summary>
		public static readonly DependencyProperty ColumnsProperty =
			DependencyProperty.Register("Columns", typeof(int), typeof(GameCanvas), new PropertyMetadata(1, (d, e) =>
			{
				var canvas = d as GameCanvas;
				canvas.InvalidateArrange();
			}));

		protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize)
		{
			var row = 0;
			var column = 0;

			foreach (UIElement element in base.InternalChildren)
			{
				element.Visibility = System.Windows.Visibility.Hidden;
			}

			foreach (UIElement element in base.InternalChildren)
			{
				if (element != null)
				{
					element.Visibility = System.Windows.Visibility.Visible;

					/*var positionY = GetPositionY(element);
					if (positionY % 2 == 0)
						x = GetPositionX(element) * element.DesiredSize.Width * 1.5;
					else
						x = GetPositionX(element) * element.DesiredSize.Width * 1.5 + element.DesiredSize.Width * 0.75;
					y = positionY * element.DesiredSize.Height * 0.5;*/

					/*if (row % 2 == 0)
						x = column * element.DesiredSize.Width * 1.5;
					else
						x = column * element.DesiredSize.Width * 1.5 + element.DesiredSize.Width * 0.75;

					y = row * element.DesiredSize.Height * 0.5;*/

					var x = column * element.DesiredSize.Width * 0.75;
					var y = (row + 0.5 * Math.Abs(column - Math.Floor(Columns / 2.0))) * element.DesiredSize.Height;

					element.Arrange(new Rect(new Point(x, y), element.DesiredSize));

					row++;
					if (row == Rows - (int)Math.Abs(column - Math.Floor(Columns / 2.0)))
					{
						row = 0;
						column++;

						if (Columns % 2 == 0 && column == Columns + 1)
							break;
						if (Columns % 2 == 1 && column == Columns)
							break;
					}
				}
			}
			return arrangeSize;
		}
	}
}
