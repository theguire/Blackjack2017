

namespace Blackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Deck
    {
        private static IEnumerable<Suit> Suits() => Enum.GetValues(typeof(Suit)) as IEnumerable<Suit>;
        private static IEnumerable<CardRank> CardRanks() => Enum.GetValues(typeof(CardRank)) as IEnumerable<CardRank>;

        List<PlayingCard> deck = new List<PlayingCard>();
        public Deck()
        {
            deck = MakeADeck();
        }


        // Number of cards remaining in deck.
        public int Count()
        {
            return (deck.Count());
        }

        public static void ShowDeck(List<PlayingCard> deck)
        {
            foreach (PlayingCard card in deck)
            {
                Console.WriteLine(card);
            }

        }

        // Deal a random card from the deck.
        public PlayingCard DealCard(Hand hand)
        {
            Random rand = new Random();

            if (deck.Count == 0)
                deck = MakeADeck();
            int idx = rand.Next( deck.Count);   // Get a random # between 0-# of cards left in deck

            PlayingCard card = deck[idx];           // Get the card at the random index

            hand.TakeCard(card);
            deck.RemoveAt(idx);                         // Remove the card from the deck
            return (card);
        }

        public static List<PlayingCard> MakeADeck()
        {
            
            return (
                        from s in Suits()
                        from r in CardRanks()
                        select new PlayingCard(s, r)).ToList<PlayingCard>();
        }

    }
}