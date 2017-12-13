namespace BlackjackController
{
    using System;
    using System.Text;
    using Blackjack;

    public class Program
    {
        const int DealerCardPosY = 1;
        const int PlayerCardPosY = 13;
        const int StartMsgConsolePosX = 2;
        const int GameResultsStartMsgConsolePosX = 1;
        const int GamePlayMsgPosYOffset = 20;
        const int CardHeight = 6;
        const int ConsoleWindowWidth = 100;
        const int ConsoleWindowHeight = 50;
        
        const int CardStartPosX = 5;


        public static void Main(string[ ] args)
        {
            Console.BufferWidth = Console.WindowWidth = ConsoleWindowWidth;
            Console.BufferHeight = Console.WindowHeight = ConsoleWindowHeight;
            Console.CursorVisible = false;

            var game = new PlayGame();// Instantiate the game

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
                    case ConsoleKey.Enter:  //Deal 
                    case ConsoleKey.Spacebar:  // Stand
                        if ( game.IsActionAllowed(Blackjack.Action.Deal ))
                        {
                            game.DealHands();
                        }
                        else
                        {
                            game.Stand();
                        }

                        break;
                    case ConsoleKey.H:  // Hit
                        if ( game.IsActionAllowed(Blackjack.Action.Hit ))
                        {
                            game.Hit();
                        }
                        break;
                    case ConsoleKey.D:  // Double Down
                        if (game.IsActionAllowed(Blackjack.Action.Hit))
                        {
                            game.Hit();
                            if ( game.IsActionAllowed( Blackjack.Action.Stand))
                            {
                                game.Stand();
                            }
                        }
                        break;
                }
            }


        }

        private static void ShowAllowedActions( PlayGame game )
        {
            var sb = new StringBuilder();

           if (game.IsActionAllowed(Blackjack.Action.Hit))
            {
                sb.Append( "H)it" );
            }

            if (game.IsActionAllowed(Blackjack.Action.Stand))
            {
                sb.Append((sb.Length > 0 ? ", " : string.Empty) + "<SPACEBAR> Stand");
            }

            if ( game.IsActionAllowed( Blackjack.Action.DoubleDown ) )
            {
                sb.Append( ( sb.Length > 0 ? ", " : string.Empty ) + "D)ouble Down" );
            }

            if (game.IsActionAllowed(Blackjack.Action.Deal))
            {
                sb.Append((sb.Length > 0 ? ", " : string.Empty) + "<ENTER> Deal" );
            }

            Console.SetCursorPosition(Console.BufferWidth - 60, GamePlayMsgPosYOffset);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(sb.ToString().PadLeft(60));
            Console.ResetColor();
        }

        private static void OnAllowedActionsChanged(object sender, EventArgs e)
        {
            ShowAllowedActions((PlayGame)sender);
        }

        private static void ShowWinner(PlayGame game)
        {
            game.Stats.GameCount++;
            Console.ForegroundColor = ConsoleColor.Green;

            String winMessage = "Winner";
            String blankMessage = "      ";
            String dealerMessage = blankMessage;
            String playerMessage = blankMessage;

            switch (game.LastState)
            {

                case State.DealerWon:
                    game.Dealer.WinCount++;
                    dealerMessage = winMessage;
                    playerMessage = blankMessage;
                    break;

                case State.PlayerWon:
                    game.Player.WinCount++;
                    dealerMessage = blankMessage;
                    playerMessage = winMessage;
                    break;

                default:
                    game.Stats.DrawCount++;
                    dealerMessage = blankMessage;
                    playerMessage = blankMessage;
                    break;
            }

            var sb = new StringBuilder();
            Console.SetCursorPosition(StartMsgConsolePosX, PlayerCardPosY + 1);
            //sb.Append( playerMessage );
            //String message = new String( "Wins {0} Win % {1}", game.Player.WinCount,
            //                            game.Player.WinCount / game.Stats.GameCount )
            Console.Write(playerMessage.PadLeft(GameResultsStartMsgConsolePosX));

            Console.SetCursorPosition(StartMsgConsolePosX, DealerCardPosY + 1);
            Console.Write(dealerMessage.PadLeft(GameResultsStartMsgConsolePosX));

            Console.ResetColor();
        }

        private static void OnLastStateChanged(object sender, EventArgs e)
        {
            ShowWinner((PlayGame)sender);
        }

        private static void ShowCards(Hand hand)
        {
            var verticleOffset = hand.IsDealer ? DealerCardPosY : PlayerCardPosY;

            var player = hand.IsDealer ? "Dealer" : "Player";
            
            Console.SetCursorPosition(StartMsgConsolePosX, verticleOffset);
            Console.Write(string.Format("{0} ({1})   Hard: {2}   Soft: {3}", player, 
                                                                                                            hand.Value, 
                                                                                                            hand.HardValue, 
                                                                                                            hand.SoftValue)
                                                                                                            .PadRight(50));

            for (var i = 0; i < hand.Cards.Count; i++)
            {
                var isLastCard = ( i == hand.Cards.Count - 1 );
                Console.SetCursorPosition(StartMsgConsolePosX + (i * CardStartPosX ), verticleOffset + 2);
                WriteCardTop(isLastCard, i);
                
                Console.SetCursorPosition(StartMsgConsolePosX + (i * CardStartPosX ), verticleOffset + 3);
                WriteCard(hand, i);
                 
                for (int  j = 4; j < CardHeight; j++)
                {
                    Console.SetCursorPosition(StartMsgConsolePosX + (i * CardStartPosX ), verticleOffset + j);
                    WriteCardBorder(isLastCard, i);
                }

                Console.SetCursorPosition(StartMsgConsolePosX + (i * CardStartPosX ), verticleOffset + CardHeight);
                WriteCardBottom(isLastCard, i);
            }
        }

        private static void WriteCardBorder( bool isLastCard, int i)
        {
            Console.WriteLine(" ".PadRight(5) + (isLastCard ? "  " : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
        }
        private static void WriteCard(Hand hand, int i)
        {
            Console.WriteLine(" " + hand.Cards[i].ToString().PadRight(10) + string.Empty.PadRight(Console.BufferWidth - 12 - (i * 5)));
        }
        private static void WriteCardTop( bool isLastCard, int i)
        {
            Console.Write("┌────" + (isLastCard ? "──┐" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
        }

        private static void WriteCardBottom(bool isLastCard, int i)
        {
            Console.WriteLine("└────" + (isLastCard ? "──┘" : string.Empty).PadRight(Console.BufferWidth - 12 - (i * 5)));
        }

        private static void OnHandChanged(object sender, EventArgs e)
        {
            ShowCards( (Hand)sender );
        }

            
    }

}
