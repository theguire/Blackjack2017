

namespace Blackjack
{

    using System.Collections.Generic;


    public abstract class PlayerBase
    {
        public List<Hand> Hands = new List<Hand>();
        public Hand Hand { get; protected set; }
        protected PlayerBase()
        {

        }



    }
}
