namespace BlackjackController
{
    using System;
    using System.Text;
    using Blackjack;

    public class Program
    {
        const int DealerOffsetTop = 1;
        const int PlayerOffsetTop = 13;
        const int HorizontalOffsetStart = 2;
        const int GameResultsHorizontalOffset = 20;

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
                    case ConsoleKey.D:  //Deal 
                    case ConsoleKey.S:  // Stand
                        if ( game.IsActionAllowed( GameAction.Deal ))
                        {
                            game.DealHands();
                        }
                        else
                        {
                            game.Stand();

                        }

                        break;
                    case ConsoleKey.H:  // Hit
                        if ( game.IsActionAllowed( GameAction.Hit ))
                        {
                            game.Hit();
                        }

                        break;
                }
            }


        }

        private static void ShowAllowedActions( PlayGame game )
        {
            var sb = new StringBuilder();

            if (game.IsActionAllowed(GameAction.Hit))
            {
                sb.Append("H)it");
            }

            if (game.IsActionAllowed(GameAction.Stand))
            {
                sb.Append((sb.Length > 0 ? ", " : string.Empty) + "S)tand");
            }

            if (game.IsActionAllowed(GameAction.Deal))
            {
                sb.Append((sb.Length > 0 ? ", " : string.Empty) + "D)eal");
            }

            Console.SetCursorPosition(Console.BufferWidth - 50, 24);
            Console.WriteLine(sb.ToString().PadLeft(20));
        }

        private static void OnAllowedActionsChanged(object sender, EventArgs e)
        {
            ShowAllowedActions((PlayGame)sender);
        }

        private static void ShowWinner(PlayGame game)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            String winMessage = "Winner";
            String blankMessage = "      ";
            String dealerMessage = blankMessage;
            String playerMessage = blankMessage;

            switch (game.LastState)
            {

                case GameState.DealerWon:
                    dealerMessage = winMessage;
                    playerMessage = blankMessage;
                    break;

                case GameState.PlayerWon:
                    dealerMessage = blankMessage;
                    playerMessage = winMessage;
                    break;

                default:
                    dealerMessage = blankMessage;
                    playerMessage = blankMessage;
                    break;
            }

            Console.SetCursorPosition(HorizontalOffsetStart, PlayerOffsetTop + 1);
            Console.Write(playerMessage.PadLeft(GameResultsHorizontalOffset));

            Console.SetCursorPosition(HorizontalOffsetStart, DealerOffsetTop + 1);
            Console.Write(dealerMessage.PadLeft(GameResultsHorizontalOffset));

            Console.ResetColor();
        }

        //private static void ShowWinner( PlayGame game )
        //{
        //    {
        //        Console.ForegroundColor = ConsoleColor.DarkGreen;

        //        Console.SetCursorPosition(Console.BufferWidth - 30, 1);
        //        Console.Write((game.LastState == GameState.DealerWon ? "DEALER WON!" : "           ").PadLeft(28));

        //        Console.SetCursorPosition(Console.BufferWidth - 30, 13);
        //        Console.Write((game.LastState == GameState.PlayerWon ? "PLAYER WON!" : "           ").PadLeft(28));

        //        Console.ResetColor();
        //    }
        //}
        private static void OnLastStateChanged(object sender, EventArgs e)
        {
            ShowWinner((PlayGame)sender);
        }

        private static void ShowCards( Hand hand )
        {
            var offsetTop = hand.IsDealer ? DealerOffsetTop : PlayerOffsetTop;
            var name = hand.IsDealer ? "Dealer" : "Player";
            var value = hand.IsDealer ? hand.FaceValue : hand.TotalValue;

            Console.SetCursorPosition(HorizontalOffsetStart, offsetTop);
            Console.Write(string.Format("{0} ({1}):", name, value).PadRight(25));

            for (var i = 0; i < hand.Cards.Count; i++)
            {
                var last = i == hand.Cards.Count - 1;
                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + 2);
                Console.Write("┌────" + (last ? "─┐" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + 3);
                Console.WriteLine(" " + hand.Cards[i].ToString().PadRight(10) + string.Empty.PadRight(Console.BufferWidth - 12 - (i * 4)));
                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + 4);
                Console.WriteLine(" ".PadRight(5) + (last ? "  " : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + 5);
                Console.WriteLine(" ".PadRight(5) + (last ? "  " : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + 6);
                Console.WriteLine(" ".PadRight(5) + (last ? "  " : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + 7);
                Console.WriteLine(" ".PadRight(5) + (last ? "  " : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + 8);
                Console.WriteLine("└────" + (last ? "─┘" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
            }
        }



        private static void OnHandChanged(object sender, EventArgs e)
        {
            ShowCards((Hand)sender);
        }

            
    }

}
