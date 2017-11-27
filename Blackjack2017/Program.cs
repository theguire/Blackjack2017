namespace BlackjackController
{
    using System;
    using System.Text;
    using Blackjack;

    public class Program
    {
        public static void Main(string[ ] args)
        {

            Console.BufferWidth = Console.WindowWidth = 70;
            Console.BufferHeight = Console.WindowHeight = 26;
            Console.CursorVisible = false;
            var game = new PlayGame();
            game.AllowedActionsChanged += OnAllowedActionsChanged;
            game.LastStateChanged += OnLastStateChanged;
            game.Dealer.Hand.Changed += OnHandChanged;
            game.Player.Hand.Changed += OnHandChanged;
            game.Play();

            while (true)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        if ((game.AllowedActions & GameAction.Deal) == GameAction.Deal)
                        {
                            game.DealHands();
                        }
                        else
                        {
                            game.Stand();

                        }

                        break;
                    case ConsoleKey.Spacebar:
                        if ((game.AllowedActions & GameAction.Hit) == GameAction.Hit)
                        {
                            game.Hit();
                        }

                        break;
                }
            }


        }

        private static bool IsDeal(PlayGame game)
        {
            return ((game.AllowedActions & GameAction.Deal) == GameAction.Deal);
        }

        private static bool IsNewDeck(PlayGame game)
        {
            return ((game.AllowedActions & GameAction.Shuffle) == GameAction.Shuffle);
        }

        private static void OnAllowedActionsChanged(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var game = (PlayGame)sender;

            if ((game.AllowedActions & GameAction.Hit) == GameAction.Hit)
            {
                sb.Append("HIT (Spacebar)");
            }

            if ((game.AllowedActions & GameAction.Stand) == GameAction.Stand)
            {
                sb.Append((sb.Length > 0 ? ", " : string.Empty) + "STAND (Enter)");
            }

            if ((game.AllowedActions & GameAction.Deal) == GameAction.Deal)
            {
                sb.Append((sb.Length > 0 ? ", " : string.Empty) + "DEAL (Enter)");
            }

            Console.SetCursorPosition(Console.BufferWidth - 31, 24);
            Console.WriteLine(sb.ToString().PadLeft(29));
        }

        private static void OnLastStateChanged(object sender, EventArgs e)
        {
            var game = (PlayGame)sender;

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.SetCursorPosition(Console.BufferWidth - 30, 1);
            Console.Write((game.LastState == GameState.DealerWon ? "DEALER WON!" : "           ").PadLeft(28));

            Console.SetCursorPosition(Console.BufferWidth - 30, 13);
            Console.Write((game.LastState == GameState.PlayerWon ? "PLAYER WON!" : "           ").PadLeft(28));

            Console.ResetColor();
        }



        /// Show the cards dealt
        private static void OnHandChanged(object sender, EventArgs e)
        {
            var hand = (Hand)sender;
            var offsetTop = hand.IsDealer ? 1 : 13;
            var name = hand.IsDealer ? "DEALER" : "PLAYER";
            var value = hand.IsDealer ? hand.FaceValue : hand.TotalValue;

            Console.SetCursorPosition(2, hand.IsDealer ? 1 : 13);
            Console.Write(string.Format("{0}'s HAND ({1}):", name, value).PadRight(25));

            for (var i = 0; i < hand.Cards.Count; i++)
            {
                var last = i == hand.Cards.Count - 1;
                Console.SetCursorPosition(2 + (i * 5), offsetTop + 2);
                Console.Write("┌────" + (last ? "─┐" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(2 + (i * 5), offsetTop + 3);
                Console.WriteLine("│" + hand.Cards[i].ToString().PadRight(10));// + string.Empty.PadRight(Console.BufferWidth - 12 - (i * 4)));
                //Console.WriteLine("│" + (hand.Cards[i].IsCardFaceUp ? hand.Cards[i].ToString() : "XXX").PadRight(4) + (last ? " │" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(2 + (i * 5), offsetTop + 4);
                Console.WriteLine("│".PadRight(5) + (last ? " │" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(2 + (i * 5), offsetTop + 5);
                Console.WriteLine("│".PadRight(5) + (last ? " │" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(2 + (i * 5), offsetTop + 6);
                Console.WriteLine("│".PadRight(5) + (last ? " │" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(2 + (i * 5), offsetTop + 7);
                Console.WriteLine("│".PadRight(5) + (last ? " │" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(2 + (i * 5), offsetTop + 8);
                Console.WriteLine("└────" + (last ? "─┘" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
            }
        }
    }

}
