using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Catan.Model;

namespace Catan.View {
	/// <summary>
	/// Új játék ablak logikája
	/// 
	/// Tele van ronda megoldásokkal, majd talán átdolgozom
	/// </summary>
	public partial class NewGameWindow : Window {
		/// <summary>
		/// Játékosok száma
		/// </summary>
		private int _players;

		/// <summary>
		/// Getter a játékosok számához
		/// </summary>
		public int Players {
			get {
				return _players;
			}
		}

		/// <summary>
		/// Konstruktor
		/// Felépíti a felületet
		/// </summary>
		public NewGameWindow() {
			InitializeComponent();

			clbPlayers.Items.Add("Kék");
			clbPlayers.Items.Add("Piros");
			clbPlayers.Items.Add("Fehér");
			clbPlayers.Items.Add("Narancs");
		}

		/// <summary>
		/// Előállítja a játékosokat a megadott adatok alapján
		/// </summary>
		/// <returns>Játékos objektumok tömbje</returns>
		public Player[] getPlayers() {
			Player[] result = new Player[Players];
			int i = 0;

			if (Players != 0) {
				if (txtPlayerName1.IsEnabled) {
					result[i++] = new Player(txtPlayerName1.Text, PlayerColor.Blue);
				}
				if (txtPlayerName2.IsEnabled) {
					result[i++] = new Player(txtPlayerName2.Text, PlayerColor.Red);
				}
				if (txtPlayerName3.IsEnabled) {
					result[i++] = new Player(txtPlayerName3.Text, PlayerColor.White);
				}
				if (txtPlayerName4.IsEnabled) {
					result[i++] = new Player(txtPlayerName4.Text, PlayerColor.Orange);
				}
			}

			return result;
		}

		/// <summary>
		/// Mégse gomb eseménykezelője
		/// Beállítja a játékosszámot 0-ra és bezérja az ablakot
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCancel_Click(object sender, RoutedEventArgs e) {
			_players = 0;
			this.Close();
		}

		/// <summary>
		/// Játékos listában kijelölés változás eseménykezelője
		/// Frissíti a nevek beviteli mezőit
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void clbPlayers_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e) {
			_players = 0;
			txtPlayerName1.IsEnabled = txtPlayerName2.IsEnabled = txtPlayerName3.IsEnabled = txtPlayerName4.IsEnabled = false;
			foreach (string item in clbPlayers.SelectedItems) {
				_players += 1;
				switch (item) {
					case "Kék":
						txtPlayerName1.IsEnabled = true;
						break;
					case "Piros":
						txtPlayerName2.IsEnabled = true;
						break;
					case "Fehér":
						txtPlayerName3.IsEnabled = true;
						break;
					case "Narancs":
						txtPlayerName4.IsEnabled = true;
						break;
				}
			}
		}

		/// <summary>
		/// Ellenőrzi a játék kezdésének feltételeit:
		/// - legalább 2 játékos
		/// - mindenkinek van neve
		/// 
		/// @TODO: minden játékosnév egyedi kell legyen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnNewGame_Click(object sender, RoutedEventArgs e) {
			if (_players < 2) {
				MessageBox.Show("Legalább 2 játékost kell megadj!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			if (txtPlayerName1.IsEnabled && txtPlayerName1.Text == "") {
				MessageBox.Show("Adj nevet a kék játékosnak!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			if (txtPlayerName2.IsEnabled && txtPlayerName2.Text == "") {
				MessageBox.Show("Adj nevet a piros játékosnak!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			if (txtPlayerName3.IsEnabled && txtPlayerName3.Text == "") {
				MessageBox.Show("Adj nevet a fehér játékosnak!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			if (txtPlayerName4.IsEnabled && txtPlayerName4.Text == "") {
				MessageBox.Show("Adj nevet a narancs játékosnak!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			this.Close();
		}
	}
}
