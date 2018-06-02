

using System;

namespace Blackjack
{
    public class Player : PlayerBase
    {
        public static int DoubleCount = 0;
        public static int DoubleDownWins = 0;

        private PlayingCard splitCard;  // hold one of the split cards until the first hand has finished

        public Player()
        {
            this.Hand = new Hand( isDealer: false );
            this.Hands.Add( this.Hand );


        }

       

        public PlayingCard SplitCard
        {
            set { splitCard = value;  }
            get { return splitCard; }
        }
    }

}
