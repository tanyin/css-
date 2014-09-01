using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace NewBaiduClick
{
    public class ManueMouseMove
    {
        public bool mouseMoving = false;
        private Point startPosition;
        private Point endPosition;
        public Point nowPosition;

        public ManueMouseMove(Point pos1, Point pos2)
        {
            mouseMoving = true;
            nowPosition = new Point(0, 0);
            startPosition = pos1;
            endPosition = pos2;
            nowPosition = startPosition;
        }
        public bool next()
        {
            Random random = new Random();
            int count = random.Next(40, 50);
            float kRandom = random.Next(5, 16) / 10;
            int lengthX = endPosition.X - nowPosition.X;
            int xDiff = 0;
            if ((endPosition.X - nowPosition.X) * (endPosition.X - nowPosition.X)
                + (endPosition.Y - nowPosition.Y) * (endPosition.Y - nowPosition.Y) < 100)
            {
                mouseMoving = false;
                return false;
            }
            if (endPosition.X - nowPosition.X == 0)
            {
                nowPosition.X = endPosition.X + 1;
            }
            if (endPosition.Y - nowPosition.Y == 0)
            {
                nowPosition.Y = endPosition.Y + 1;
            }
            float k = (endPosition.Y - nowPosition.Y) / (endPosition.X - nowPosition.X);
            if (endPosition.X > nowPosition.X)
            {
                xDiff = (int)(lengthX / count) + random.Next(3);
            }
            else
            {
                xDiff = (int)(lengthX / count) - random.Next(3);
            }

            nowPosition.X = nowPosition.X + xDiff;
            if ((int)(xDiff * k * kRandom) != 0)
            {
                nowPosition.Y = nowPosition.Y + (int)(xDiff * k * kRandom);
            }
            else
            {
                if (endPosition.Y > nowPosition.Y)
                {
                    nowPosition.Y = nowPosition.Y + 3;
                }
                else
                {
                    nowPosition.Y = nowPosition.Y - 3;
                }
            }
            if (Math.Abs(nowPosition.X - startPosition.X) > Math.Abs(endPosition.X - startPosition.X))
            {
                nowPosition.X = endPosition.X;
            }
            if (Math.Abs(nowPosition.Y - startPosition.Y) > Math.Abs(endPosition.Y - startPosition.Y))
            {
                nowPosition.Y = endPosition.Y;
            }
           
            return true;
        }
    }
}
