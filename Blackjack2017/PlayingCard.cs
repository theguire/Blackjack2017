

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

        public const int JACK = 9;
        public const int QUEEN = 10;
        public const int KING = 11;

        public Suit CardSuit { get; }
        public CardRank CardRank { get; }
      //  public bool IsCardFaceUp { get; set; }

        public int HardValue { get; }
        public int SoftValue { get; }

        public PlayingCard(Suit s, CardRank r)
        {
            this.CardSuit = s;
            this.CardRank = r;
      //      this.IsCardFaceUp = true;

        }

        //public void ShowFace()
        //{
        //    IsCardFaceUp = !IsCardFaceUp;
        //}


        public override string ToString()
        {
            return $"{CardRank} of {CardSuit}";
        }

        //public int GetCardValue(CardRank CardRank)
        //{

        //    int CardRankValue = CardRank.GetHashCode();
        //    switch (CardRankValue)
        //    {
        //        case JACK:
        //        case QUEEN:
        //        case KING:
        //            return (10);
        //        default:
        //            return (CardRank.GetHashCode() + 1);

        //    }
        //}


    }

}
