using KonicaServer.BusinessObjects;

namespace KonicaServer.BusinessLogic
{
    public static partial class GameLogic
    {
        // Function returns a string of the current player
        public static string GetCurrentPlayer()
        {
            if (GameState.PlayerTurn == PlayerTurn.player2)
                return "Player 2";
            else
                return "Player 1";
        }

        // Function returns string of the next player (same as current player if the end node is invalid) and updates the PlayerTurn state
        public static string GetNextPlayer(string nodeStatus)
        {
            if (nodeStatus.Equals("VALID_END_NODE"))
            {
                if (GameState.PlayerTurn == PlayerTurn.player1)
                {
                    GameState.PlayerTurn = PlayerTurn.player2; // update state
                    return "Player 2";
                }
                else
                {
                    GameState.PlayerTurn = PlayerTurn.player1; // update state
                    return "Player 1";
                }
            }
            else return GetCurrentPlayer();
        }
    }
}
