

namespace Blackjack
{
    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    
    public enum CardRank
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 10,
        Queen = 10,
        King = 10
    }


    public class PlayingCard
    {
        public Suit CardSuit { get; }
        public CardRank CardRank { get; }

        public PlayingCard(Suit s, CardRank r)
        {
            this.CardSuit = s;
            this.CardRank = r;
        }

        public override string ToString()
        {
            return $"{CardRank} of {CardSuit}";
        }

     }

}
