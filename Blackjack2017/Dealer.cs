

namespace Blackjack
{
    public class Dealer : PlayerBase
    {
        public Dealer()
        {
            this.Hand = new Hand(isDealer: true);
            this.Hands.Add(this.Hand);
        }
    }
}
