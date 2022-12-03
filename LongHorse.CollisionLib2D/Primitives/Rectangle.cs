using System.Numerics;

namespace LongHorse.CollisionLib2D.Primitives
{
    public class Rectangle : IBoundingArea
    {
        public Rectangle() { }

        /// <summary>
        /// Creates a new rectangle using top, left, width, and height values. 
        /// To make a rectangle from a center point and width/height, use direct instantiation instead of the constructor.
        /// </summary>
        /// <param name="left">The x-coordinate of the left edge of the boundary rectangle.</param>
        public Rectangle(float left, float top, float width, float height)
        {
            Size = new Vector2(width, height);
            SetLeft(left);
            SetTop(top);
        }

        public Vector2 Center { get; set; }
        public Vector2 Size { get; set; }
        public float Top => Center.Y - HalfHeight;
        public float Bottom => Center.Y + HalfHeight;
        public float Left => Center.X - HalfHeight;
        public float Right => Center.X + HalfHeight;
        public float HalfWidth => Size.X * 0.5f;
        public float HalfHeight => Size.Y * 0.5f;

        /// <summary>Updates the left value of the Rectangle while maintaining the same width, height, top, and bottom values.</summary>
        /// <param name="left">The x-coordinate of the left edge of the boundary rectangle.</param>
        public void SetLeft(float left)
        {
            Center = new Vector2(left + HalfWidth, Center.Y);
        }

        /// <summary>Updates the right value of the Rectangle while maintaining the same width, height, top, and bottom values.</summary>
        /// <param name="right">The x-coordinate of the right edge of the boundary rectangle.</param>
        public void SetRight(float right)
        {
            Center = new Vector2(right - HalfWidth, Center.Y);
        }

        /// <summary>Updates the top value of the Rectangle while maintaining the same width, height, left, and right values.</summary>
        /// <param name="top">The y-coordinate of the top edge of the boundary rectangle.</param>
        public void SetTop(float top)
        {
            Center = new Vector2(Center.X, top + HalfHeight);
        }

        /// <summary>Updates the bottom value of the Rectangle while maintaining the same width, height, left, and right values.</summary>
        /// <param name="bottom">The y-coordinate of the bottom edge of the boundary rectangle.</param>
        public void SetBottom(float bottom)
        {
            Center = new Vector2(Center.X, bottom - HalfHeight);
        }

        public Vector2 NearestPoint(Vector2 p)
        {
            return p.NearestPoint(this);
        }

        public BoundingType BoundingType => BoundingType.Rectangle;
    }

}
