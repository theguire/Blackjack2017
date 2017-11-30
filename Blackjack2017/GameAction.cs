namespace Blackjack
{
    public enum GameAction : byte
    {
        None = 1,
        Shuffle = 2,
        Deal = 3,
        Stand = 4,
        Hit = 5,
        Split = 6,
        DoubleDown = 7

    }

    //public class GameActionPlay
    //{
    //    public bool DealAction(GameAction action)
    //    {
    //        return ((action & GameAction.Deal) == GameAction.Deal);
    //    }
    //}



}

