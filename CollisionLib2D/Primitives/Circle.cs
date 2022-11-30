﻿using System.Numerics;

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
        public BoundingType BoundingType => BoundingType.Circle;
    }
}
