using System;
using KonicaServer.BusinessObjects;

namespace KonicaServer.BusinessLogic
{
    // RuleLogic refers to logic involved with enforcing rules - specifically rules concerning the second point in a turn
    public static partial class GameLogic
    {
        // If the newLine is vertical, check for validity
        public static string VerticalLineCheck(Point point2)
        {
            int startPoint = Math.Min(GameState.point1.y, point2.y);
            int endPoint = Math.Max(GameState.point1.y, point2.y);

            for (int i = startPoint; i <= endPoint; i++)
            {
                if (i == GameState.point1.y) // we know this point is endpoint - don't check
                    continue; 

                if (GameState.Grid.grid[point2.x, i].status != GridPointStatus.free) // if any point along the newLine isn't free (except point 1), the node is invalid
                    return "INVALID_END_NODE";
            }

            return "VALID_END_NODE"; // all nodes along line free, node valid
        }

        // If the newLine is horizontal, check for validity
        public static string HorizontalLineCheck(Point point2)
        {
            int startPoint = Math.Min(GameState.point1.x, point2.x);
            int endPoint = Math.Max(GameState.point1.x, point2.x);

            for (int i = startPoint; i <= endPoint; i++)
            {
                if (i == GameState.point1.x)
                    continue;

                if (GameState.Grid.grid[i, point2.y].status != GridPointStatus.free)
                    return "INVALID_END_NODE";
            }

            return "VALID_END_NODE";
        }

        // If the newLine is diagonal, check for validity
        public static string DiagonalLineCheck(Point point2)
        {
            Point point1 = GameState.point1;

            // Different logic required depending on direction of line
            if (point2.x > point1.x && point2.y > point1.y)
            {
                return UpRightLine(point2);
            }
            else if (point2.x > point1.x && point2.y < point1.y)
            {
                return DownRightLine(point2);
            }
            else if (point2.x < point1.x && point2.y > point1.y)
            {
                return UpLeftLine(point2);
            }
            else if (point2.x < point1.x && point2.y < point1.y)
            {
                return DownLeftLine(point2);
            }

            return "INVALID_END_NODE";
        }

        // Check the validity of a diagonal line going up-right
        public static string UpRightLine(Point point2)
        {
            Point point1 = GameState.point1;

            int xIndex = point1.x + 1;
            int yIndex = point1.y + 1;

            while (xIndex != (point2.x + 1))
            {
                if (GameState.Grid.grid[xIndex, yIndex].status != GridPointStatus.free) // if any point along the newLine isn't free (except point 1), the end node is invalid
                    return "INVALID_END_NODE";

                if (IsUpRightCrossingLine(GameState.Grid.grid[xIndex, yIndex])) // if the diagonal line crosses another diagonal line, the end node is invalid
                    return "INVALID_END_NODE";

                if ((xIndex == point2.x && yIndex != point2.y) ||
                    (xIndex != point2.x && yIndex == point2.y)) // If not an octilinear line, the end node is invalid
                    return "INVALID_END_NODE";

                xIndex++;
                yIndex++;
            }

            return "VALID_END_NODE"; // otherwise, valid end node
        }

        // Check the validity of a diagonal line going down-right
        public static string DownRightLine(Point point2)
        {
            Point point1 = GameState.point1;

            int xIndex = point1.x + 1;
            int yIndex = point1.y - 1;

            while (xIndex != (point2.x + 1))
            {
                if (GameState.Grid.grid[xIndex, yIndex].status != GridPointStatus.free)
                    return "INVALID_END_NODE";

                if (IsDownRightCrossingLine(GameState.Grid.grid[xIndex, yIndex]))
                    return "INVALID_END_NODE";

                if ((xIndex == point2.x && yIndex != point2.y) ||
                    (xIndex != point2.x && yIndex == point2.y))
                    return "INVALID_END_NODE";

                xIndex++;
                yIndex--;
            }

            return "VALID_END_NODE";
        }

        // Check the validity of a diagonal line going up-left
        public static string UpLeftLine(Point point2)
        {
            Point point1 = GameState.point1;

            int xIndex = point1.x - 1;
            int yIndex = point1.y + 1;

            while (xIndex != (point2.x - 1))
            {
                if (GameState.Grid.grid[xIndex, yIndex].status != GridPointStatus.free)
                    return "INVALID_END_NODE";

                if (IsUpLeftCrossingLine(GameState.Grid.grid[xIndex, yIndex]))
                    return "INVALID_END_NODE";

                if ((xIndex == point2.x && yIndex != point2.y) ||
                    (xIndex != point2.x && yIndex == point2.y))
                    return "INVALID_END_NODE";

                xIndex--;
                yIndex++;
            }

            return "VALID_END_NODE";
        }

        // Check the validity of a diagonal line going down-left
        public static string DownLeftLine(Point point2)
        {
            Point point1 = GameState.point1;

            int xIndex = point1.x - 1;
            int yIndex = point1.y - 1;

            while (xIndex != (point2.x - 1))
            {
                if (GameState.Grid.grid[xIndex, yIndex].status != GridPointStatus.free)
                    return "INVALID_END_NODE";

                if (IsDownLeftCrossingLine(GameState.Grid.grid[xIndex, yIndex]))
                    return "INVALID_END_NODE";

                if ((xIndex == point2.x && yIndex != point2.y) ||
                    (xIndex != point2.x && yIndex == point2.y))
                    return "INVALID_END_NODE";

                xIndex--;
                yIndex--;
            }

            return "VALID_END_NODE";
        }

        // Function to check if an up-right diagonal line segment crosses the line
        public static bool IsUpRightCrossingLine(GridPoint p1)
        {
            GridPoint UpLeftGp = GameState.Grid.grid[p1.x - 1, p1.y]; // get gridpoint up-left of newLine
            foreach (GridPoint gp in UpLeftGp.adjacentPoints)
            {
                if (gp.x == p1.x && gp.y == (p1.y - 1)) // check for crossing lines
                    return true;
            }
            return false;
        }

        // Function to check if an up-left diagonal line segment crosses the line
        public static bool IsUpLeftCrossingLine(GridPoint p1)
        {
            GridPoint UpRightGp = GameState.Grid.grid[p1.x + 1, p1.y]; // get gridpoint up-right of newLine
            foreach (GridPoint gp in UpRightGp.adjacentPoints)
            {
                if (gp.x == p1.x && gp.y == (p1.y - 1)) // check for crossing lines
                    return true;
            }
            return false;
        }

        // Function to check if a down-right diagonal line segment crosses the line
        public static bool IsDownRightCrossingLine(GridPoint p1)
        {
            GridPoint UpRightGp = GameState.Grid.grid[p1.x, p1.y + 1]; // get gridpoint up-right of newLine
            foreach (GridPoint gp in UpRightGp.adjacentPoints)
            {
                if (gp.x == (p1.x - 1) && gp.y == p1.y) // check for crossing lines
                    return true;
            }
            return false;
        }

        // Function to check if an down-left diagonal line segment crosses the line
        public static bool IsDownLeftCrossingLine(GridPoint p1)
        {
            GridPoint UpLeftGp = GameState.Grid.grid[p1.x, p1.y + 1]; // get gridpoint up-left of newLine
            foreach (GridPoint gp in UpLeftGp.adjacentPoints)
            {
                if (gp.x == (p1.x + 1) && gp.y == p1.y) // check for crossing lines
                    return true;
            }
            return false;
        }
    }
}
