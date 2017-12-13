

namespace Blackjack
{

    using System.Collections.Generic;


    public abstract class PlayerBase
    {
        public int WinCount { get; set; } 
        public List<Hand> Hands = new List<Hand>();
        public Hand Hand { get; protected set; }
        protected PlayerBase()
        {
            this.WinCount = 0;

        }



    }
}
