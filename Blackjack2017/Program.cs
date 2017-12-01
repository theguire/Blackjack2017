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
        const int CardHeight = 8;

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

        private static void OnLastStateChanged(object sender, EventArgs e)
        {
            ShowWinner((PlayGame)sender);
        }

        private static void ShowCards(Hand hand)
        {
            var offsetTop = hand.IsDealer ? DealerOffsetTop : PlayerOffsetTop;

            var name = hand.IsDealer ? "Dealer" : "Player";
            
            Console.SetCursorPosition(HorizontalOffsetStart, offsetTop);
            Console.Write(string.Format("{0} Real: ({1}) Hard: {2} Soft: {3}", 
                                    name, hand.RealValue, hand.HardValue, hand.SoftValue)
                                    .PadRight(30));

            for (var i = 0; i < hand.Cards.Count; i++)
            {
                var isPlayerCard = i == hand.Cards.Count - 1;
                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + 2);
                WriteCardTop(isPlayerCard, i);
                
                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + 3);
                WriteCard(hand, i);

                for (int  j = 4; j < CardHeight; j++)
                {
                    Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + j);
                    WriteCardBorder(isPlayerCard, i);
                }

                Console.SetCursorPosition(HorizontalOffsetStart + (i * 5), offsetTop + CardHeight);
                WriteCardBottom(isPlayerCard, i);

                
            }
        }

        private static void WriteCardBorder( bool isPlayerCard, int i)
        {
            Console.WriteLine(" ".PadRight(5) + (isPlayerCard ? "  " : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
        }
        private static void WriteCard(Hand hand, int i)
        {
            Console.WriteLine(" " + hand.Cards[i].ToString().PadRight(10) + string.Empty.PadRight(Console.BufferWidth - 12 - (i * 4)));
        }
        private static void WriteCardTop( bool isPlayerCard, int i)
        {
            Console.Write("┌────" + (isPlayerCard ? "─┐" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
        }

        private static void WriteCardBottom(bool isPlayerCard, int i)
        {
            Console.WriteLine("└────" + (isPlayerCard ? "─┘" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
        }

        private static void OnHandChanged(object sender, EventArgs e)
        {
            ShowCards((Hand)sender);
        }

            
    }

}
