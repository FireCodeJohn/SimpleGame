using KonicaServer.BusinessObjects;

namespace KonicaServer.BusinessLogic
{
    public static partial class GameLogic
    {
        // Function determines if a start node is valid
        public static string IsStartNodeValid(Point point)
        {
            // Is it within the bounds of the 2-d grid array?
            if (point.x > 3 || point.x < 0)
            {
                return "INVALID_START_NODE";
            }
            else if (point.y > 3 || point.y < 0)
            {
                return "INVALID_START_NODE";
            }

            else if (GameState.Grid.gridEmpty) // if grid is empty, is valid
            {
                GameState.point1 = new GridPoint(point.x, point.y, GridPointStatus.endpoint);
                return "VALID_START_NODE";
            }
            else if (GameState.Grid.grid[point.x, point.y].status == GridPointStatus.endpoint) // else, if is an endpoint, is valid
            {
                GameState.point1 = new GridPoint(point.x, point.y, GridPointStatus.linethrough);
                return "VALID_START_NODE";
            }
            else // all other cases it is not valid
            {
                return "INVALID_START_NODE";
            }
        }

        // Function determines if an end node is valid
        public static string IsEndNodeValid(Point point)
        {
            // Is it within the bounds of the 2-d grid array?
            if (point.x > 3 || point.x < 0)
            {
                return "INVALID_END_NODE";
            }
            else if (point.y > 3 || point.y < 0)
            {
                return "INVALID_END_NODE";
            }

            else if (GameState.point1.x == point.x && GameState.point1.y == point.y) // if same point as start node, end node is invalid
            {
                return "INVALID_END_NODE";
            }
            else if (GameState.point1.x == point.x && GameState.point1.y != point.y) // if vertical line, check vertical line logic
            {
                return VerticalLineCheck(point);
            }
            else if (GameState.point1.x != point.x && GameState.point1.y == point.y) // if horizontal line, check horizontal line logic
            {
                return HorizontalLineCheck(point);
            }
            else if (GameState.point1.x != point.x && GameState.point1.y != point.y) // if diagonal line, check diagonal line logic
            {
                return DiagonalLineCheck(point);
            }

            return "INVALID_END_NODE"; // theoretically unreachable
        }
    }
}
