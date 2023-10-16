using System;

namespace CavesOfQuickMenu.Concepts
{
    public static class LegacyCoord
    {
        public const int OFFSET_X = 23;
        public const int OFFSET_Y = 1;
        public const int HEIGHT = 20;
        public const int WIDTH = 32;
        public const int HEIGHT_SELECTED = 5;
        public const int WIDTH_SELECTED = 8;

        /// <summary>
        /// Return 4 int represent 2 sets of coordinate for quick menu screen region.<br/>
        /// <code>
        /// return topLeftX, topLeftY, bottomRightX, bottomRightY
        /// </code>
        /// </summary>
        public static (int, int, int, int) GetBaseCoord()
        {
            int x1 = 24;
            int y1 = 2;
            int x2 = 55;
            int y2 = 22;
            return (x1, y1, x2, y2);
        }

        /// <summary>
        /// Return 4 int represent 2 sets of coordinate for selected item region.<br/>
        /// <code>
        /// return topLeftX, topLeftY, bottomRightX, bottomRightY
        /// </code>
        /// </summary>
        public static (int, int, int, int) GetSelectedCoord(Direction direction)
        {
            switch (direction)
            {
                case Direction.NW:
                    return (24, 2, 31, 6);
                case Direction.N:
                    return (36, 2, 43, 6);
                case Direction.NE:
                    return (48, 2, 55, 6);
                case Direction.W:
                    return (24, 10, 31, 14);
                case Direction.M:
                    return (36, 10, 43, 14);
                case Direction.E:
                    return (48, 10, 55, 14);
                case Direction.SW:
                    return (24, 18, 31, 22);
                case Direction.S:
                    return (36, 18, 43, 22);
                case Direction.SE:
                    return (48, 18, 55, 22);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
