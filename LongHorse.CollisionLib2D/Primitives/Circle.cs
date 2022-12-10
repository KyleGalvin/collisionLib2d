using System;
using System.Numerics;

namespace LongHorse.CollisionLib2D.Primitives
{
    public class Circle : IBoundingArea
    {
        public Vector2 Center { get; set; }

        public float Radius;
        public float Left => Center.X - Radius;
        public float Right => Center.X + Radius;
        public float Top => Center.Y - Radius;
        public float Bottom => Center.Y + Radius;

        public Vector2 NearestPoint(Vector2 p)
        {
            return p.NearestPoint(this);
        }
        public bool Intersects(IBoundingArea a)
        {
            return a.BoundingType switch
            {
                BoundingType.Rectangle => BoundsExtensions.Intersects(this, (Rectangle)a),
                BoundingType.Triangle => BoundsExtensions.Intersects(this, (Triangle)a),
                BoundingType.Circle => BoundsExtensions.Intersects(this, (Circle)a),
                BoundingType.LineSegment => BoundsExtensions.Intersects(this, (LineSegment)a),
                _ => throw new NotImplementedException($"No intersects() method for given IBoundingArea pair ({BoundingType}, {a.BoundingType})")
            };
        }
        public BoundingType BoundingType => BoundingType.Circle;
    }
}
