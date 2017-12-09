using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack
{
    class GameParameters
    {
        public const int SoftHandValue = 17;  // Dealer must stand on 17 or greater
        public const int Blackjack = 21;
        public const int DeckLowCardCount = 12; // Create new deck when cards remaining get below
    }
}
