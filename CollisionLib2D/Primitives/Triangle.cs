using System;
using System.Numerics;

namespace LongHorse.CollisionLib2D.Primitives
{
    public class Triangle : IBoundingArea
    {
        public Vector2[] Points = new Vector2[3];
        public Vector2 Center
        {
            get
            {
                //centroid
                var ab = Points[0] + Points[1] / 2.0f;
                var ac = Points[0] + Points[2] / 2.0f;
                return new LineSegment(ab, Points[2]).Intersection(new LineSegment(ac, Points[1]));
            }
            set
            {
                var delta = value - Center; //translate all points from the existing center to the new center
                Points[0] += delta;
                Points[1] += delta;
                Points[2] += delta;
            }
        }

        public float Left => MathF.Min(MathF.Min(Points[0].X, Points[1].X), Points[2].X);

        public float Right => MathF.Max(MathF.Max(Points[0].X, Points[1].X), Points[2].X);

        public float Top => MathF.Min(MathF.Min(Points[0].Y, Points[1].Y), Points[2].Y);

        public float Bottom => MathF.Max(MathF.Max(Points[0].Y, Points[1].Y), Points[2].Y);

        public BoundingType BoundingType => BoundingType.Triangle;
    }
}
