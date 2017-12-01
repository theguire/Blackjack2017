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

        private readonly List<PlayingCard> cards = new List<PlayingCard>();


        public Hand(bool isDealer = false)
        {
            this.IsDealer = isDealer;
            AddHandToPlayer();
        }

        public ReadOnlyCollection<PlayingCard> Cards
        {
            get { return this.cards.AsReadOnly(); }
        }

        public int SoftValue
        {
            get { return(GetHandValue());  }
        }

        // Return the number of Aces in the hand 
        private int GetAcesCount()
        {
            return( this.cards.Count(c => c.CardRank == CardRank.Ace));
        }

        public int RealValue
        {
            get
            {
                return (BustHand(HardValue) ? SoftValue : (HardValue >= SoftValue ? HardValue : SoftValue));
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

        public bool BustHand( int handValue )
        {
            return (handValue > 21);   
        }
        public void AddCard(PlayingCard card)
        {
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

        public void Clear()
        {
            this.cards.Clear();
        }
    }
}



