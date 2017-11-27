
namespace Blackjack
{
    using System;

    class PlayGame
    {
        private GameAction allowedActions;
        private GameState lastState;

        private Deck deck;

        public PlayGame()
        {
            this.Dealer = new Dealer();
            this.Player = new Player();
            this.AllowedActions = GameAction.None;
            this.LastState = GameState.Unknown;

        }

        public event EventHandler AllowedActionsChanged;
        public event EventHandler LastStateChanged;

        public Player Player { get; private set; }
        public Dealer Dealer { get; private set; }
        public GameAction AllowedActions
        {
            get
            {
                return (this.allowedActions);
            }

            private set
            {
                if (this.allowedActions != value)
                {
                    this.allowedActions = value;
                    if (this.AllowedActionsChanged != null)
                    {
                        AllowedActionsChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public void Play()
        {
            this.AllowedActions = GameAction.Deal;

            if (this.AllowedActionsChanged != null)
            {
                this.AllowedActionsChanged(this, EventArgs.Empty);
            }
        }

        public GameState LastState
        {
            get
            {
                return this.lastState;
            }

            private set
            {
                if (this.lastState != value)
                {
                    this.lastState = value;
                    if (this.LastStateChanged != null)
                    {
                        this.LastStateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        // Deal two cards to each player
        public void DealHands()
        {
            if ((this.AllowedActions & GameAction.Deal) != GameAction.Deal)
            {
                throw new InvalidOperationException();
            }

            this.LastState = GameState.Unknown;


            if ((this.deck == null) || (deck.Count() < 15))
            {
                this.deck = new Deck();
            }

            this.Dealer.Hand.Clear();
            this.Player.Hand.Clear();

            this.deck.DealCard(this.Player.Hand);
            this.deck.DealCard(this.Dealer.Hand);

            this.deck.DealCard(this.Player.Hand);
            this.deck.DealCard(this.Dealer.Hand);

            PlayTheDeal();
        }

        private bool BustHand(Hand hand)
        {
            return (this.Player.Hand.TotalValue > 21);
        }
        public void Hit()
        {
            if ((this.AllowedActions & GameAction.Hit) != GameAction.Hit)
            {
                throw new InvalidOperationException();
            }

            this.deck.DealCard(this.Player.Hand);

            if (BustHand(this.Player.Hand))
            {
                this.LastState = GameState.DealerWon;
                this.AllowedActions = GameAction.Deal;
            }
        }


        private void DealerPlay()
        {

            while (this.Dealer.Hand.SoftValue < 17)
            {
                this.deck.DealCard(Dealer.Hand);
            }


        }

        public void DetermineWinner()
        {
            if (this.Dealer.Hand.TotalValue > 21 || this.Player.Hand.TotalValue > this.Dealer.Hand.TotalValue)
            {
                this.LastState = GameState.PlayerWon;
            }
            else if (this.Dealer.Hand.TotalValue == this.Player.Hand.TotalValue)
            {
                this.LastState = GameState.Draw;
            }
            else
            {
                this.LastState = GameState.DealerWon;
            }

        }
        public void Stand()
        {
            if ((this.AllowedActions & GameAction.Stand) != GameAction.Stand)
            {
                throw new InvalidOperationException();
            }
            DealerPlay();
            DetermineWinner();

            this.AllowedActions = GameAction.Deal;
        }

        public void PlayTheDeal()
        {

            if (this.Player.Hand.SoftValue == 21)
            {
                if (this.Dealer.Hand.SoftValue == 21)
                {
                    this.LastState = GameState.Draw;
                }
                else
                {
                    this.LastState = GameState.PlayerWon;
                }
                this.AllowedActions = GameAction.Deal;
            }
            else if (this.Dealer.Hand.TotalValue == 21)
            {
                this.LastState = GameState.DealerWon;
                this.AllowedActions = GameAction.Deal;
            }
            else
            {
                this.AllowedActions = GameAction.Hit | GameAction.Stand;
            }
        }
    }
}


