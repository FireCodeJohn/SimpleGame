using System;
using System.Collections.Generic;
using KonicaServer.BusinessLogic;

namespace KonicaServer.BusinessObjects
{
    // This is the game state object, contains the state properties as well as several functions used to update the state at the end of each turn.
    public class GameState
    {
        public Grid Grid; // Grid state
        public List<Point> EndPoints; // Endpoints (is always length 2 after first turn)
        public PlayerTurn PlayerTurn; // player turn status
        public TurnStage TurnStage; // turn stage status
        public Point point1; // the first potential point when drawing a line.  Used heavily in TurnStage.point2 to determine viability of the second point

        public GameState() // initial state
        {
            Grid = new Grid();
            EndPoints = new List<Point>();
            point1 = null;
            PlayerTurn = PlayerTurn.player1;
            TurnStage = TurnStage.point1;
        }

        // Function for updating the grid state
        public void UpdateGridState(Point point2)
        {
            // if the grid is empty, the first point is an endpoint, otherwise it becomes a linethrough point
            if (Grid.gridEmpty) 
            {
                Grid.grid[point1.x, point1.y].status = GridPointStatus.endpoint;
                Grid.gridEmpty = false;
            }
            else
                Grid.grid[point1.x, point1.y].status = GridPointStatus.linethrough;

            // the second point is always an endpoint
            Grid.grid[point2.x, point2.y].status = GridPointStatus.endpoint; 
         
            // Depending on line direction, logic will be different for the rest of the update
            if (point1.x == point2.x && point1.y < point2.y) // if upwards vertial line update state
            {
                FinishUpdateUpLine(point2);                
            }
            else if (point1.x == point2.x && point1.y > point2.y) // if downwards vertial line update state
            {
                FinishUpdateDownLine(point2);
            }
            else if (point1.x < point2.x && point1.y == point2.y) // if right horizontal line update state
            {
                FinishUpdateRightLine(point2);
            }
            else if (point1.x > point2.x && point1.y == point2.y) // if left horizontal line update state
            {
                FinishUpdateLeftLine(point2);
            }
            else if (point2.x > point1.x && point2.y > point1.y) // Up Right Line
            {
                FinishUpdateUpRightLine(point2);
            }
            else if (point2.x > point1.x && point2.y < point1.y) // down right line
            {
                FinishUpdateDownRightLine(point2);
            }
            else if (point2.x < point1.x && point2.y > point1.y)  // up left line
            {
                FinishUpdateUpLeftLine(point2);
            }
            else if (point2.x < point1.x && point2.y < point1.y) // down left line
            {
                FinishUpdateDownLeftLine(point2);
            }

            // Update the Endpoints property and do a quality check on the state of the grid
            UpdateGridEndpointsAndQualCheck();
        }

        // Finish updating a line going straight up.  Set AdjacentPoints for endpoints and throughlines, and set status of throughline nodes to throughline
        public void FinishUpdateUpLine(Point point2)
        {
            int startIndex = point1.y + 1;
            int endIndex = point2.y;

            Grid.grid[point1.x, point1.y].adjacentPoints.Add(Grid.grid[point1.x, point1.y + 1]);

            for (int i = startIndex; i <= endIndex; i++)
            {
                Grid.grid[point1.x, i].adjacentPoints.Add(Grid.grid[point1.x, i - 1]);
                if (i != endIndex) // if i == maxIndex, do not have upper adjacent point
                {
                    Grid.grid[point1.x, i].status = GridPointStatus.linethrough;
                    Grid.grid[point1.x, i].adjacentPoints.Add(Grid.grid[point1.x, i + 1]);
                }
            }
        }

        // Finish updating a line going straight down.  Set AdjacentPoints for endpoints and throughlines, and set status of throughline nodes to throughline
        public void FinishUpdateDownLine(Point point2)
        {
            int startIndex = point1.y - 1;
            int endIndex = point2.y;

            Grid.grid[point1.x, point1.y].adjacentPoints.Add(Grid.grid[point1.x, point1.y - 1]);

            for (int i = startIndex; i >= endIndex; i--)
            {
                Grid.grid[point1.x, i].adjacentPoints.Add(Grid.grid[point1.x, i + 1]);
                if (i != endIndex) // if i == endIndex, do not have lower adjacent point
                {
                    Grid.grid[point1.x, i].status = GridPointStatus.linethrough;
                    Grid.grid[point1.x, i].adjacentPoints.Add(Grid.grid[point1.x, i - 1]);
                }
            }
        }

        // Finish updating a line going straight right.  Set AdjacentPoints for endpoints and throughlines, and set status of throughline nodes to throughline
        public void FinishUpdateRightLine(Point point2)
        {
            int startIndex = point1.x + 1;
            int endIndex = point2.x;

            Grid.grid[point1.x, point1.y].adjacentPoints.Add(Grid.grid[point1.x + 1, point1.y]);

            for (int i = startIndex; i <= endIndex; i++)
            {
                Grid.grid[i, point1.y].adjacentPoints.Add(Grid.grid[i - 1, point1.y]);
                if (i != endIndex) // if i == endIndex, do not have right adjacent point
                {
                    Grid.grid[i, point1.y].status = GridPointStatus.linethrough;
                    Grid.grid[i, point1.y].adjacentPoints.Add(Grid.grid[i + 1, point1.y]);
                }
            }
        }

        // Finish updating a line going straight left.  Set AdjacentPoints for endpoints and throughlines, and set status of throughline nodes to throughline
        public void FinishUpdateLeftLine(Point point2)
        {
            int startIndex = point1.x - 1;
            int endIndex = point2.x;

            Grid.grid[point1.x, point1.y].adjacentPoints.Add(Grid.grid[point1.x - 1, point1.y]);

            for (int i = startIndex; i >= endIndex; i--)
            {
                Grid.grid[i, point1.y].adjacentPoints.Add(Grid.grid[i + 1, point1.y]);
                if (i != endIndex) // if i == endIndex, do not have left adjacent point
                {
                    Grid.grid[i, point1.y].status = GridPointStatus.linethrough;
                    Grid.grid[i, point1.y].adjacentPoints.Add(Grid.grid[i - 1, point1.y]);
                }
            }
        }

        // Finish updating a line going diagonally up-right.  Set AdjacentPoints for endpoints and throughlines, and set status of throughline nodes to throughline
        public void FinishUpdateUpRightLine(Point point2)
        {
            Point currentPoint = new Point() { x = point1.x + 1, y = point1.y + 1 };

            Grid.grid[point1.x, point1.y].adjacentPoints.Add(Grid.grid[point1.x + 1, point1.y + 1]);

            while (currentPoint.x <= point2.x && currentPoint.y <= point2.y)
            {
                Grid.grid[currentPoint.x, currentPoint.y].adjacentPoints.Add(Grid.grid[currentPoint.x - 1, currentPoint.y - 1]);
                if (currentPoint.x != point2.x) // if currentPoint == point2, do not have upper-right adjacent point
                {
                    Grid.grid[currentPoint.x, currentPoint.y].status = GridPointStatus.linethrough;
                    Grid.grid[currentPoint.x, currentPoint.y].adjacentPoints.Add(Grid.grid[currentPoint.x + 1, currentPoint.y + 1]);
                }
                currentPoint.x++;
                currentPoint.y++;
            }

            if (currentPoint.x != (point2.x + 1) || currentPoint.y != (point2.y + 1))
                throw new Exception("Internal Server Error: Problem with Diagonal line.");
        }

        // Finish updating a line going diagonally up-left.  Set AdjacentPoints for endpoints and throughlines, and set status of throughline nodes to throughline
        public void FinishUpdateUpLeftLine(Point point2)
        {
            Point currentPoint = new Point() { x = point1.x - 1, y = point1.y + 1 };

            Grid.grid[point1.x, point1.y].adjacentPoints.Add(Grid.grid[point1.x - 1, point1.y + 1]);

            while (currentPoint.x >= point2.x && currentPoint.y <= point2.y)
            {
                Grid.grid[currentPoint.x, currentPoint.y].adjacentPoints.Add(Grid.grid[currentPoint.x + 1, currentPoint.y - 1]);
                if (currentPoint.x != point2.x) // if currentPoint == point2, do not have upper-left adjacent point
                {
                    Grid.grid[currentPoint.x, currentPoint.y].status = GridPointStatus.linethrough;
                    Grid.grid[currentPoint.x, currentPoint.y].adjacentPoints.Add(Grid.grid[currentPoint.x - 1, currentPoint.y + 1]);
                }
                currentPoint.x--;
                currentPoint.y++;
            }

            if (currentPoint.x != (point2.x - 1) || currentPoint.y != (point2.y + 1))
                throw new Exception("Internal Server Error: Problem with Diagonal line.");
        }

        // Finish updating a line going diagonally down-right.  Set AdjacentPoints for endpoints and throughlines, and set status of throughline nodes to throughline
        public void FinishUpdateDownRightLine(Point point2)
        {
            Point currentPoint = new Point() { x = point1.x + 1, y = point1.y - 1 };

            Grid.grid[point1.x, point1.y].adjacentPoints.Add(Grid.grid[point1.x + 1, point1.y - 1]);

            while (currentPoint.x <= point2.x && currentPoint.y >= point2.y)
            {
                Grid.grid[currentPoint.x, currentPoint.y].adjacentPoints.Add(Grid.grid[currentPoint.x - 1, currentPoint.y + 1]);
                if (currentPoint.x != point2.x) // if currentPoint == point2, do not have down-right adjacent point
                {
                    Grid.grid[currentPoint.x, currentPoint.y].status = GridPointStatus.linethrough;
                    Grid.grid[currentPoint.x, currentPoint.y].adjacentPoints.Add(Grid.grid[currentPoint.x + 1, currentPoint.y - 1]);
                }
                currentPoint.x++;
                currentPoint.y--;
            }

            if (currentPoint.x != (point2.x + 1) || currentPoint.y != (point2.y - 1))
                throw new Exception("Internal Server Error: Problem with Diagonal line.");
        }

        // Finish updating a line going diagonally down-left.  Set AdjacentPoints for endpoints and throughlines, and set status of throughline nodes to throughline
        public void FinishUpdateDownLeftLine(Point point2)
        {
            Point currentPoint = new Point() { x = point1.x - 1, y = point1.y - 1 };

            Grid.grid[point1.x, point1.y].adjacentPoints.Add(Grid.grid[point1.x - 1, point1.y - 1]);

            while (currentPoint.x >= point2.x && currentPoint.y >= point2.y)
            {
                Grid.grid[currentPoint.x, currentPoint.y].adjacentPoints.Add(Grid.grid[currentPoint.x + 1, currentPoint.y + 1]);
                if (currentPoint.x != point2.x) // if currentPoint == point2, do not have down-left adjacent point
                {
                    Grid.grid[currentPoint.x, currentPoint.y].status = GridPointStatus.linethrough;
                    Grid.grid[currentPoint.x, currentPoint.y].adjacentPoints.Add(Grid.grid[currentPoint.x - 1, currentPoint.y - 1]);
                }
                currentPoint.x--;
                currentPoint.y--;
            }

            if (currentPoint.x != (point2.x - 1) || currentPoint.y != (point2.y - 1))
                throw new Exception("Internal Server Error: Problem with Diagonal line.");
        }

        // Update the End points, ensure there are exactly 2, and check every node has the correct number of Adjacent Node pointers
        public void UpdateGridEndpointsAndQualCheck()
        {
            EndPoints = new List<Point>();

            for (int i = 0; i < 4; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    if (Grid.grid[i, ii].status == GridPointStatus.endpoint)
                    {
                        EndPoints.Add(Grid.grid[i, ii]);
                        if (Grid.grid[i, ii].adjacentPoints.Count != 1)
                            throw new Exception("All endpoints must have exactly 1 adjacent point");
                    }
                    else if (Grid.grid[i, ii].status == GridPointStatus.linethrough)
                    {
                        if (Grid.grid[i, ii].adjacentPoints.Count != 2)
                            throw new Exception("All mid-line points must have exactly 2 adjacent points");
                    }
                    else if (Grid.grid[i, ii].status == GridPointStatus.free)
                    {
                        if (Grid.grid[i, ii].adjacentPoints.Count != 0)
                            throw new Exception("All free points must have exactly 0 adjacent points");
                    }
                }
            }

            if (EndPoints.Count != 2)
                throw new Exception("There must be 2 endpoints at the end of each turn!");
        }

        // check to see if the game is over
        public bool CheckIfGameOver()
        {
            foreach (Point endpoint in EndPoints)
            {
                this.point1 = endpoint;

                int xStartPt = endpoint.x - 1;
                int xEndPt = endpoint.x + 1;
                int yStartPt = endpoint.y - 1;
                int yEndPt = endpoint.y + 1;

                // ensure we stay within bounds of 2D array
                int xStart = Math.Max(0, xStartPt);
                int xEnd = Math.Min(3, xEndPt);
                int yStart = Math.Max(0, yStartPt);
                int yEnd = Math.Min(3, yEndPt);

                for (int i = xStart; i <= xEnd; i++)
                {
                    for (int ii = yStart; ii <= yEnd; ii++)
                    {
                        if (GameLogic.IsEndNodeValid(Grid.grid[i, ii]).Equals("VALID_END_NODE")) // if any surrounding nodes are valid game continues
                            return false;
                    }
                }
            }
            return true; // if no surrounding nodes for both endpoints are valid, the game is over
        }
    }
}
