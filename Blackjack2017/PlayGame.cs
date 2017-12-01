
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
            if ( !IsActionAllowed( GameAction.Deal))
            {
                throw new InvalidOperationException();
            }

            this.LastState = GameState.Unknown;


            // Build a new deck of cards before deal if less than 15 cards left in deck
            if ((this.deck == null) || (deck.Count() < 15))
            {
                this.deck = new Deck();
            }

            this.DealNewHands(); // Deal hand to player and dealer
            this.PlayTheDeal();
        }

        private void DealPlayersCard()
        {
            this.deck.DealCard(this.Player.Hand);
            this.deck.DealCard(this.Dealer.Hand);
        }
        private void ClearHands()
        {
            this.Dealer.Hand.Clear();
            this.Player.Hand.Clear();

        }

        private void DealNewHands()
        {
            this.ClearHands();
            this.DealPlayersCard();  // Deal first card to Player and Dealer
            this.DealPlayersCard();  // Deal second card to Player and Dealer

        }
        public void Hit()
        {
            if ( !IsActionAllowed( GameAction.Hit))
            {
                throw new InvalidOperationException();
            }

            this.deck.DealCard(this.Player.Hand);

            if ( this.Player.Hand.BustHand(this.Player.Hand.RealValue))
            {
                this.LastState = GameState.DealerWon;
                this.AllowedActions = GameAction.Deal;
            }
        }


        private void DealerPlay()
        {

            while ((this.Dealer.Hand.SoftValue < 17) && (this.Dealer.Hand.RealValue < 17))
            {
                this.deck.DealCard(Dealer.Hand);
            }


        }

        private void DetermineWinner()
        {
            if (this.Dealer.Hand.BustHand(this.Dealer.Hand.RealValue) || 
                            this.Player.Hand.RealValue > this.Dealer.Hand.RealValue)
            {
                this.LastState = GameState.PlayerWon;
            }
            else if (this.Dealer.Hand.RealValue == this.Player.Hand.RealValue)
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
           if ( ! IsActionAllowed(GameAction.Stand ))
            {
                throw new InvalidOperationException();
            }
            DealerPlay();
            DetermineWinner();

            this.AllowedActions = GameAction.Deal;
        }

        public void PlayTheDeal()
        {

            if (this.Player.Hand.SoftValue == 21 || this.Player.Hand.RealValue == 21 )
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
            else if (this.Dealer.Hand.RealValue == 21)
            {
                this.LastState = GameState.DealerWon;
                this.AllowedActions = GameAction.Deal;
            }
            else
            {
                this.AllowedActions = GameAction.Hit | GameAction.Stand;
            }
        }

        public bool IsActionAllowed(GameAction desiredAction)
        {
            return ((this.AllowedActions & desiredAction) == desiredAction);
        }

    }

   
}


