namespace Blackjack
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Hand
    {
        public event EventHandler Changed;
        public bool IsDealer { get; private set; }

        //private readonly List<PlayingCard> cards = new List<PlayingCard>();
        private readonly List<PlayingCard> cards = new List<PlayingCard>();


        public Hand(bool isDealer = false)
        {
            this.IsDealer = isDealer;
            AddHandToPlayer();
        }

        //public ReadOnlyCollection<PlayingCard> Cards
        //{
        //    get { return this.cards.AsReadOnly(); }
        //}

        public ReadOnlyCollection<PlayingCard> Cards
        {
            get { return this.cards.AsReadOnly(); }
        }

        public int SoftValue
        {
            get { return(GetHandValue());  }
        }

        public bool IsBlackjack()
        {
            return ( this.Value == GameParameters.Blackjack );
        }
        // Return the number of Aces in the hand 
        private int GetAcesCount()
        {
            return( this.cards.Count(c => c.CardRank == CardRank.Ace));
        }


        // The hand being played.
        public int Value
        {
            get
            {
                // If the hard value of the hand is a bust, play with the Soft hand
                return ( IsBustHand( HardValue ) ? SoftValue : HardValue );
            }
        }

        
        public int HardValue
        {
            get
            {
                // If there is at least one Ace, add 10 to the hard.
                return( this.SoftValue +  (GetAcesCount() >= 1 ? 10 : 0)); 
            }
        }
        // Return the sum of the cards in the hand
        private int GetHandValue()
        {
            return( this.cards.Select(c => (int)c.CardRank).Sum());
        }

        public bool IsBustHand( int handValue )
        {
            return (handValue > GameParameters.Blackjack);   
        }

        //The hand takes a card
        public void TakeCard(PlayingCard card)
        {
            // 'cards' represent the cards held in 'this' hand
            this.cards.Add(card);

            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }

        // For splits - Player could have more than one hand
        private static void AddHandToPlayer()
        {

        }

        // Clear the hand of all cards
        public void Clear()
        {
            this.cards.Clear();
        }

        // The hand gets split
        public PlayingCard SplitHand()
        {
            PlayingCard splitCard = this.cards.Last<PlayingCard>();
            this.cards.Remove( splitCard );

            return ( splitCard );
        }
    }
}



