namespace CollisionLib2D
{
    public class Circle : IBoundingArea
    {
        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float Radius;
        public float Left => CenterX - Radius;
        public float Right => CenterX + Radius;
        public float Top => CenterY - Radius;
        public float Bottom => CenterY + Radius;
        public BoundingType BoundingType => BoundingType.Circle;
    }
}
