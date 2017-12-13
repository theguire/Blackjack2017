

namespace Blackjack
{
    public class Player : PlayerBase
    {
        public static int DoubleCount = 0;
        public static int DoubleDownWins = 0;

        public Player()
        {
            this.Hand = new Hand(isDealer: false);
            this.Hands.Add( this.Hand );
        }
    }

}
