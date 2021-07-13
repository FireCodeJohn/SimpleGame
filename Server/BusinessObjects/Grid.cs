namespace KonicaServer.BusinessObjects
{
    // Grid object keeps a 4x4 array of GridPoint objects.  It is used to keep the game state
    public class Grid
    {
        public GridPoint[,] grid;
        public bool gridEmpty;

        public Grid()
        {
            grid = new GridPoint[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    grid[i, ii] = new GridPoint(i, ii, GridPointStatus.free);
                }
            }
            gridEmpty = true;
        }
    }
}
