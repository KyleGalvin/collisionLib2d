namespace LongHorse.CollisionLib2D
{
    public static class BoundsExtensions
    {
        /// <summary>Determines if one bounding area overlaps with another.</summary>
        /// <param name="b1">The first bounding area</param>
        /// <param name="b2">The second bounding area</param>
        /// <returns>true if the two bounding areas overlap</returns>
        public static bool Intersects(this IBoundingArea b1, IBoundingArea b2)
        {
            if(b1.BoundingType == BoundingType.Rectangle)
            {
                if(b2.BoundingType == BoundingType.Rectangle)
                {
                    return ((Rectangle)(b1)).Intersects((Rectangle)b2);
                }
                else
                {
                    return ((Rectangle)(b1)).Intersects((Circle)b2);
                }
            } 
            else
            {
                if (b2.BoundingType == BoundingType.Rectangle)
                {
                    return ((Circle)(b1)).Intersects((Rectangle)b2);
                }
                else
                {
                    return ((Circle)(b1)).Intersects((Circle)b2);
                }
            }
        }

        /// <summary>Determines if one rectangle overlaps with another.</summary>
        /// <param name="r1">The first rectangle</param>
        /// <param name="r2">The second rectangle</param>
        /// <returns>true if the two rectangles overlap</returns>
        public static bool Intersects(this Rectangle r1, Rectangle r2)
        {
            if (r1.Right <= r2.Left || r1.Left >= r2.Right) return false;
            if (r1.Top >= r2.Bottom || r1.Bottom <= r2.Top) return false;
            return true;
        }

        /// <summary>Determines if one circle overlaps with another.</summary>
        /// <param name="c1">The first circle</param>
        /// <param name="c2">The second circle</param>
        /// <returns>true if the two circles overlap</returns>
        public static bool Intersects(this Circle c1, Circle c2)
        {
            return ((c1.Radius + c2.Radius) * (c1.Radius + c2.Radius)) > ((c1.Center.X - c2.Center.X) * (c1.Center.X - c2.Center.X)) + ((c1.Center.Y - c2.Center.Y) * (c1.Center.Y - c2.Center.Y));
        }

        /// <summary>Determines if the circle overlaps with the rectangle</summary>
        /// <param name="c">The circle</param>
        /// <param name="r">The rectangle</param>
        /// <returns>true if the circle and rectangle overlap</returns>
        public static bool Intersects(this Circle c, Rectangle r)
        {
            float nearestRectPointX;
            float nearestRectPointY;
            if(c.Bottom < r.Top)
            {
                nearestRectPointY = r.Top;
            } 
            else if(c.Top > r.Bottom)
            {
                nearestRectPointY = r.Bottom;
            }
            else
            {
                //the circle will intersect with the rect at a left or right line segment.
                //the circles Y value will match the rect X value at the point nearest the center
                nearestRectPointY = c.Center.Y;
            }

            if(c.Right < r.Left)
            {
                nearestRectPointX = r.Left;
            }
            else if(c.Left > r.Right)
            {
                nearestRectPointX = r.Right;
            }
            else
            {
                //the circle will intersect with the rect at a top or bottom line segment.
                //the circles X value will match the rect X value at the point nearest the center
                nearestRectPointX = c.Center.X;
            }
            return ((c.Radius) * (c.Radius)) > ((c.Center.X - nearestRectPointX) * (c.Center.X - nearestRectPointX)) + ((c.Center.Y - nearestRectPointY) * (c.Center.Y - nearestRectPointY));
        }

        /// <summary>Determines if the circle overlaps with the rectangle</summary>
        /// <param name="c">The circle</param>
        /// <param name="r">The rectangle</param>
        /// <returns>true if the circle and rectangle overlap</returns>
        public static bool Intersects(this Rectangle r, Circle c)
        {
            return c.Intersects(r);
        }
    }
}
