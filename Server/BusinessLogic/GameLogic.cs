using System;
using KonicaServer.BusinessObjects;

namespace KonicaServer.BusinessLogic
{
    // Sharing the same GameLogic class across the BusinessLogic namespace for simplicity
    public static partial class GameLogic
    {
        public static GameState GameState; // static gamestate as we only need to support 1 game at a time

        // Initialize the gamestate and return init reponse to client
        public static string InitGame()
        {
            GameState = new GameState();
            return "{\"msg\": \"INITIALIZE\", \"body\": { \"newLine\": null, \"heading\": \"Player 1\", \"message\": \"Awaiting Player 1's Move\" }}";
        }

        // All logic involved in the first step of the game
        public static string GameStep1(Point point)
        {
            GameState.point1 = null; // reset the point1 property of gamestate

            string nodeStatus, playerTurn, bodyMsg;
            nodeStatus = IsStartNodeValid(point); // is the start node valid?
            playerTurn = GetCurrentPlayer(); // who's turn comes next (the current player)
            bodyMsg = GetBodyMsg(nodeStatus); // add any error message to http response payload string

            if (nodeStatus.Equals("VALID_START_NODE"))
                GameState.TurnStage = TurnStage.point2; // if start node valid, change TurnStage state to TurnStage.point2, otherwise we stay in TurnStage.point1

            // Create return string for reponse payload and return
            string returnStr = String.Format("{{\"msg\": \"{0}\", \"body\": {{ \"newLine\": null, \"heading\": \"{1}\", \"message\": \"{2}\" }} }}", nodeStatus, playerTurn, bodyMsg);
            return returnStr;
        }

        // All logic involved in the second step of the game
        public static string GameStep2(Point point)
        {
            string nodeStatus, playerTurn, bodyMsg, newLineString;
            nodeStatus = IsEndNodeValid(point); // is the end node valid?
            newLineString = GetBodyLineStr(point, nodeStatus); // if end node is valid, get newLine Json string (string is null if invalid)
            playerTurn = GetNextPlayer(nodeStatus); // who's turn comes next?
            bodyMsg = GetBodyMsg(nodeStatus); // add any error message to http response payload string

            if (nodeStatus.Equals("VALID_END_NODE"))
            {
                GameState.UpdateGridState(point); // if end node is valid, update the game state
                if (GameState.CheckIfGameOver()) // check to see if the game is over, and if so set the variables appropiately to tell the client
                {
                    nodeStatus = "GAME_OVER";
                    playerTurn = "Game Over";
                    if (GameState.PlayerTurn == PlayerTurn.player1)
                        bodyMsg = "Player 1 wins!";
                    else
                        bodyMsg = "Player 2 wins!";
                }
            }
            GameState.TurnStage = TurnStage.point1; // whether end node is valid or invalid, the next TurnStage is TurnStage.point1     

            // Create return string for response payload and return
            string returnStr = String.Format("{{\"msg\": \"{0}\", \"body\": {{ \"newLine\": {1}, \"heading\": \"{2}\", \"message\": \"{3}\" }} }}", nodeStatus, newLineString, playerTurn, bodyMsg);
            return returnStr;
        }
    }
}
    