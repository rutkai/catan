using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using Catan.Common;
using Catan.Model;
using Catan.View;
using Catan.ViewModel.Converters;

namespace Catan.ViewModel {
	public class NewGameContext : ViewModelBase {
		private ActionCommand _NewGame;
		private List<string> _SelectedPlayers;
		public GameTableContext GameTableContext;
		protected Size tableSize;

		public NewGameContext(GameTableContext context, Size size) {
			GameTableContext = context;
			tableSize = size;
			_SelectedPlayers = new List<string>();

			Players = new Dictionary<PlayerColor, Player>();
			EnabledPlayers = new Dictionary<PlayerColor, bool>();
			foreach (PlayerColor color in Enum.GetValues(typeof(PlayerColor))) {
				Players.Add(color, new Player(color.ToString(), color));
				EnabledPlayers.Add(color, false);
			}
		}

		public Dictionary<PlayerColor, Player> Players {
			get;
			private set;
		}

		public Dictionary<PlayerColor, bool> EnabledPlayers {
			get;
			private set;
		}

		public IEnumerable<PlayerColor> PlayerColors {
			get {
				return new[] {
								PlayerColor.Blue, PlayerColor.Green, PlayerColor.Orange, PlayerColor.Red
				};
			}
		}

		public string SelectedPlayerValues {
			get {
				return String.Join(",", _SelectedPlayers.ToArray());
			}
			set {
				_SelectedPlayers = new List<string>(value.Split(','));
				foreach (PlayerColor color in Enum.GetValues(typeof(PlayerColor))) {
					EnabledPlayers[color] = _SelectedPlayers.Contains(color.ToString());
				}
				OnPropertyChanged(() => EnabledPlayers);
			}
		}

		public ActionCommand NewGameCommand {
			get {
				return Lazy.Init(ref _NewGame,
					() => new ActionCommand(() => {
							List<Player> players = new List<Player>();
							foreach (PlayerColor color in Enum.GetValues(typeof(PlayerColor))) {
								if (EnabledPlayers[color]) {
									players.Add(Players[color]);
								}
							}

							if (players.Count > 1) {
								GameController.Instance.Init((uint)tableSize.Width, players);
							} else {
								MessageBox.Show("Legalább két játékos kell!");
							}
						}));
			}
		}

	}
}
