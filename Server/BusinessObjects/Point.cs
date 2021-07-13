using System.Collections.Generic;

namespace KonicaServer.BusinessObjects
{
    // simple Point class with x and y properties
    public class Point
    {
        public int x;
        public int y;
    }

    // GridPoint (subclass of Point), which also has the status of the point and up to 2 pointers to adjacent points (pointers needed to check for line crossings)
    public class GridPoint : Point
    {
        public GridPointStatus status;
        public List<GridPoint> adjacentPoints;

        public GridPoint(int x, int y, GridPointStatus status)
        {
            this.x = x;
            this.y = y;
            this.status = status;
            adjacentPoints = new List<GridPoint>();
        }
    }

    // All GridPoints are either an endpoint, a linethrough (in the middle of the line), or free
    public enum  GridPointStatus
    {
        endpoint,
        linethrough,
        free
    }
}
