using System.Windows;
using System.Windows.Controls;

namespace Catan.View
{
	public enum GameCellState { Default, BuildRoad, BuildTown }

	//http://msdn.microsoft.com/en-us/library/ms752097.aspx#overriding_default_hit_testing
	public partial class GameCell : UserControl
	{
		public GameCellState GameCellState
		{
			get { return (GameCellState)GetValue(GameCellStateProperty); }
			set { SetValue(GameCellStateProperty, value); }
		}

		public static readonly DependencyProperty GameCellStateProperty =
			DependencyProperty.Register("GameCellState", typeof(GameCellState), typeof(GameCell), new PropertyMetadata(GameCellState.Default));

		public GameCell()
		{
			InitializeComponent();

			/*ApplyTemplate();
			var itemsBox = (ItemsControl)this.Template.FindName("RoadItems", this);
			var lines = itemsBox.Items.OfType<Line>();*/
		}
	}						  
}
