namespace LongHorse.CollisionLib2D
{
    public class Rectangle: IBoundingArea
    {
        public Rectangle() { }

        /// <summary>
        /// Creates a new rectangle using top, left, width, and height values. 
        /// To make a rectangle from a center point and width/height, use direct instantiation instead of the constructor.
        /// </summary>
        /// <param name="left">The x-coordinate of the left edge of the boundary rectangle.</param>
        public Rectangle(float left, float top, float height, float width)
        {
            Height = height;
            Width = width;
            SetLeft(left);
            SetTop(top);
        }

        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Top => CenterY - HalfHeight;
        public float Bottom => CenterY + HalfHeight;
        public float Left => CenterX - HalfHeight;
        public float Right => CenterX + HalfHeight;
        public float HalfWidth => Width * 0.5f;
        public float HalfHeight => Height * 0.5f;

        /// <summary>Updates the left value of the Rectangle while maintaining the same width, height, top, and bottom values.</summary>
        /// <param name="left">The x-coordinate of the left edge of the boundary rectangle.</param>
        public void SetLeft(float left)
        {
            CenterX = left + HalfWidth;
        }

        /// <summary>Updates the right value of the Rectangle while maintaining the same width, height, top, and bottom values.</summary>
        /// <param name="right">The x-coordinate of the right edge of the boundary rectangle.</param>
        public void SetRight(float right)
        {
            CenterX = right - HalfWidth;
        }

        /// <summary>Updates the top value of the Rectangle while maintaining the same width, height, left, and right values.</summary>
        /// <param name="top">The y-coordinate of the top edge of the boundary rectangle.</param>
        public void SetTop(float top)
        {
            CenterY = top + HalfHeight;
        }

        /// <summary>Updates the bottom value of the Rectangle while maintaining the same width, height, left, and right values.</summary>
        /// <param name="bottom">The y-coordinate of the bottom edge of the boundary rectangle.</param>
        public void SetBottom(float bottom)
        {
            CenterY = bottom - HalfHeight;
        }

        public BoundingType BoundingType => BoundingType.Rectangle;
    }

}
