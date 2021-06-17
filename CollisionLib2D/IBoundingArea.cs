namespace CollisionLib2D
{
    /// <summary>
    /// IBoundingArea is the minimum interface any object needs to implement in order to be placed into a QuadTree
    /// </summary>
    public interface IBoundingArea
    {
        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float Left { get; }
        public float Right { get; }
        public float Top { get; }
        public float Bottom { get; }
        BoundingType BoundingType { get; }
    }
}
