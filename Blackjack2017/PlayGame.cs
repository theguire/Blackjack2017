
using System;

namespace Blackjack
{


    public class PlayGame
    {
        private Action allowedActions;
        private State lastState;
        public GameStats Stats = new GameStats();

        private Deck deck;

        public PlayGame()
        {
            this.Dealer = new Dealer();
            this.Player = new Player();
            this.AllowedActions = Action.None;
            this.LastState = State.Unknown;

        }

        public event EventHandler AllowedActionsChanged;
        public event EventHandler LastStateChanged;

        public Player Player { get; private set; }
        public Dealer Dealer { get; private set; }
        public Action AllowedActions
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
            this.AllowedActions = Action.Deal;

            if ( AllowedActionsChanged != null)
            {
                this.AllowedActionsChanged(this, EventArgs.Empty);
            }
        }

        public State LastState
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
                    if ( LastStateChanged != null)
                        this.LastStateChanged(this, EventArgs.Empty);
                }
            }
        }

        // Deal two cards to each player
        public void DealHands()
        {
            if ( !IsActionAllowed( Action.Deal))
            {
                throw new InvalidOperationException();
            }

            this.LastState = State.Unknown;

            // Build a new deck of cards before deal if less than 15 cards left in deck
            if ((this.deck == null) || (deck.Count() < GameParameters.DeckLowCardCount))
                this.deck = new Deck();
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
            if ( !IsActionAllowed( Action.Hit))
            {
                throw new InvalidOperationException();
            }

            this.deck.DealCard(this.Player.Hand);

            if ( this.Player.Hand.IsBustHand(this.Player.Hand.Value))
            {
                this.LastState = State.DealerWon;
                this.AllowedActions = Action.Deal;
            }
        }

        private void DealerPlay()
        {
            while ( this.Dealer.Hand.Value < GameParameters.DealerStandLimit )
            {
                this.deck.DealCard(Dealer.Hand);
            }
        }

        private void DetermineWinner()
        {
            if ( Dealer.Hand.IsBustHand( Dealer.Hand.Value ) || 
                Player.Hand.Value > Dealer.Hand.Value )
                this.LastState = State.PlayerWon;

            else if (this.Dealer.Hand.Value == this.Player.Hand.Value)
                this.LastState = State.Draw;

            else
                this.LastState = State.DealerWon;
        }
        public void Stand()
        {
           if ( ! IsActionAllowed(Action.Stand ))
            {
                throw new InvalidOperationException();
            }
            DealerPlay();   //Player stands.  Dealer will draw to (soft) seventeen
            DetermineWinner();

            this.AllowedActions = Action.Deal;
        }
       

        private void PlayTheDeal()
        {
            if ( Player.Hand.IsBlackjack() )
            {
                if ( Dealer.Hand.IsBlackjack() )
                {
                    this.LastState = State.Draw;
                }
                else
                {
                    this.LastState = State.PlayerWon;
                }
                this.AllowedActions = Action.Deal;
            }
            else if ( Dealer.Hand.IsBlackjack() )
            {
                this.LastState = State.DealerWon;
                this.AllowedActions = Action.Deal;
            }
            else
            {
                this.AllowedActions = Action.Hit | Action.Stand | Action.DoubleDown;
            }
        }

        public bool IsActionAllowed(Action desiredAction)
        {
            return ((this.AllowedActions & desiredAction) == desiredAction);
        }

    }

   
}


