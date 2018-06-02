using System.Collections.Generic;

namespace Blackjack
{




    public abstract class PlayerBase
    {
        public int WinCount { get; set; }
        public List<Hand> Hands { get; set; }
        public Hand Hand { get; protected set; }
        //public List<Hand> Hands = new List<Hand>();
        //private List<Hand> Hands = new List<Hand>();
        //public Hand Hand { get; protected set; }
        //private Hand Hand { get; set; }
        protected PlayerBase()
        {
            this.WinCount = 0;
            this.Hands = new List<Hand>();
        }

        //public Hand Hand()
        //{
        //    this.hand = new Hand();
        //    this.hands.Add( this.hand );

        //    return (hand);
        //}





    }
}
