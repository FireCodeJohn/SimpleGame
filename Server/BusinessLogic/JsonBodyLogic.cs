using System;
using KonicaServer.BusinessObjects;

namespace KonicaServer.BusinessLogic
{
    public static partial class GameLogic
    {
        // Get the body message for the http response... it is dependent on the validity of the current node and is used to report invalid game moves
        public static string GetBodyMsg(string nodeStatus)
        {
            if (nodeStatus.Equals("INVALID_START_NODE"))
                return "You must start on either end of the path!";
            if (nodeStatus.Equals("INVALID_END_NODE"))
                return "Invalid move. Try again.";

            return "";
        }

        // Gets the json string of the newLine object for http response
        public static String GetBodyLineStr(Point point2, string nodeStatus)
        {
            if (nodeStatus.Equals("INVALID_END_NODE"))
                return "null";
            else
                return String.Format("{{ \"start\": {{ \"x\": {0}, \"y\": {1} }}, \"end\": {{ \"x\": {2}, \"y\": {3} }} }}", GameState.point1.x, GameState.point1.y, point2.x, point2.y);
        }
    }
}
