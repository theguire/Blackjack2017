

namespace Blackjack
{
    public class Player : PlayerBase
    {
        public Player()
        {
            this.Hand = new Hand(isDealer: false);
        }
    }

}
