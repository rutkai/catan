using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Catan.Common;
using Catan.Model;
using Catan.ViewModel.Commons;

namespace Catan.ViewModel
{
    /// <summary>
    /// Kereskedelemért felelős nézetmodell osztály
    /// </summary>
    public class TradeItemContext : ViewModelBase
    {
        private DelegateCommand<int?> _BuyCommand;
        private string _Message;
        public TradeItem TradeItem { get; protected set; }
        public TradeContext TradeContext { get; protected set; }

        public TradeItemContext(TradeContext context, TradeItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            TradeItem = item;
            TradeContext = context;
        }

        /// <summary>
        /// Mennyiség
        /// </summary>
        public int Quantity
        {
            get { return TradeItem.Quantity; }
            set
            {
                TradeItem.Quantity = value < Player.Materials[Material] ? value : Player.Materials[Material];
                OnPropertyChanged(() => Quantity);
            }
        }

        /// <summary>
        /// Elérhető mennyiség
        /// </summary>
        public int AvailableQuantity { get; set; }

        /// <summary>
        /// Nyersanyag
        /// </summary>
        public Material Material
        {
            get { return TradeItem.Material; }
            set
            {
                TradeItem.Material = value;
                OnPropertyChanged(() => Material);
            }
        }

        /// <summary>
        /// Ára
        /// </summary>
        public int Price
        {
            get { return TradeItem.Price; }
            set
            {
                TradeItem.Price = value;
                OnPropertyChanged(() => Price);
            }
        }

        public Player Player
        {
            get { return TradeItem.Player; }
            set
            {
                TradeItem.Player = value;
                OnPropertyChanged(() => Player);
            }
        }

        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                OnPropertyChanged(() => Message);
            }
        }

        /// <summary>
        /// Elérhető nyersanyagok maximuma
        /// </summary>
        public int Maximum
        {
            get
            {
                if (Player != null && Player.Materials.ContainsKey(Material))
                    return Player.Materials[Material];

                return 0;
            }
        }

        /// <summary>
        /// Vásárlás parancs
        /// </summary>
        public DelegateCommand<int?> BuyCommand
        {
            get
            {
                return Lazy.Init(ref _BuyCommand,
                        () => new DelegateCommand<int?>(
                            quantity => {
                                if (quantity.HasValue) {
                                    if (Price * quantity.Value > TradeContext.GameTableContext.CurrentPlayer.Gold)
                                        TradeContext.GameTableContext.ShowMessage("Nincs elég aranyad megvásárolni!", "Kereskedelem",
                                            MessageType.Warning);
                                    else {
                                        var tradePrice = Price * quantity.Value;
                                        if (Player != null) {
                                            if (Player.Materials.ContainsKey(Material)) {
                                                Player.Materials[Material] -= quantity.Value;
                                            }
                                            var currentPlayer = TradeContext.GameTableContext.CurrentPlayer;

                                            if (currentPlayer.Materials.ContainsKey(Material))
                                                currentPlayer.Materials[Material] += quantity.Value;
                                            else
                                                currentPlayer.Materials.Add(Material, quantity.Value);

                                            currentPlayer.Gold -= tradePrice;
                                            Player.Gold += tradePrice;
                                            Quantity -= quantity.Value;
                                        }
                                    }
                                }

                                if (TradeContext != null)
                                    TradeContext.Refresh();
                            },
                            quantity => !quantity.HasValue ||
                                        (quantity <= Quantity && Quantity > 0 &&
                                        TradeContext != null &&
                                        TradeContext.GameTableContext != null &&
                                        TradeContext.GameTableContext.CurrentPlayer != null)
                        ));
            }
        }
    }
}
