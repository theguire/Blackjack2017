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
            // get { return this.cards.Select(c => (int)c.CardRank > 1 && (int)c.CardRank < 11 ? (int)c.CardRank : 10).Sum(); }
            get { return(GetHandValue());  }
        }


        // Return the number of Aces in the hand 
        private int GetAcesCount()
        {
            return( this.cards.Count(c => c.CardRank == CardRank.Ace));
        }


        public int TotalValue
        {
            get
            {
                var totalValue = this.SoftValue;
                var numberOfAces = GetAcesCount();

                while (numberOfAces-- > 0 && totalValue > 21)
                {
                    totalValue -= 9;
                }

                return totalValue;

            }

        }

        // Return the sum of the cards in the hand
        private int GetHandValue()
        {
            return( this.cards.Select(c => (int)c.CardRank).Sum());
        }
        public int FaceValue
        {
            get
            {
                //var faceValue = this.cards.Where(c => c.IsCardFaceUp)
                //.Select(c => (int)c.CardRank > 1 && (int)c.CardRank < 11 ? (int)c.CardRank : 10).Sum();

                var faceValue = GetHandValue();
                //var aces = this.cards.Count(c => c.CardRank == CardRank.Ace);
                var numberOfAces =GetAcesCount();

                while (numberOfAces-- > 0 && faceValue > 21)
                {
                    faceValue -= 9;
                }

                return faceValue;
            }
        }

        //public bool IsBlackjack
        //{
        //    get { throw new NotImplementedException(); }
        //}

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



