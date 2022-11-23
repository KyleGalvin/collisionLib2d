using System;
using System.Numerics;

namespace LongHorse.CollisionLib2D
{
    public class Triangle : IBoundingArea
    {
        public Vector2[] Points = new Vector2[3];
        public Vector2 Center { 
            get { return new Vector2((Left + Right) / 2.0f, (Top + Bottom) / 2.0f); } 
            set { 
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
