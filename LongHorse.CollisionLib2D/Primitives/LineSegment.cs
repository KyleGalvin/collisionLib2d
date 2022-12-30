using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LongHorse.CollisionLib2D.Primitives
{
    public class LineSegment : IBoundingArea
    {
        public LineSegment(Vector2 p1, Vector2 p2) { Points = new Vector2[2] { p1, p2 }; }

        public Vector2[] Points = new Vector2[2];
        public Vector2 Center
        {
            get => (Points[0] + Points[1]) / 2.0f;
            set
            {
                var delta = value - Center;
                Points[0] += delta;
                Points[1] += delta;
            }
        }

        public float Left => MathF.Min(Points[0].X, Points[1].X);

        public float Right => MathF.Max(Points[0].X, Points[1].X);

        public float Top => MathF.Max(Points[0].Y, Points[1].Y);

        public float Bottom => MathF.Min(Points[0].Y, Points[1].Y);

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
                _ => throw new NotImplementedException($"No intersects() method for given IBoundingArea {a.BoundingType}")
            };
        }

        public BoundingType BoundingType => BoundingType.LineSegment;
    }
}
