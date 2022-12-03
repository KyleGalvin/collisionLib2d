using LongHorse.CollisionLib2D;
using LongHorse.CollisionLib2D.Primitives;
using System.Numerics;
using Xunit;

namespace XunitTests
{
    [Trait("Module", "Collision")]
    [Trait("Module", "Geometry")]
    [Trait("Category", "Unit")]
    public class CollisionTests
    {
        Rectangle _unitRectangle = new Rectangle() { Size = new Vector2(1.0f, 1.0f), Center = new Vector2(0.0f, 0.0f) };
        Circle _unitCircle = new Circle() { Radius = 0.5f, Center = new Vector2(0.0f, 0.0f) };
        Triangle _unitTriangle = new Triangle() { Points = new Vector2[] { new Vector2(-1.0f, -1.0f), new Vector2(0.0f, 1.0f), new Vector2(1.0f, -1.0f) } };

        [Theory]
        [ClassData(typeof(CircleGenerator))]
        public void Circle_Circle_Collision(Circle c, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, _unitCircle.Intersects(c));

        }

        [Theory]
        [ClassData(typeof(RectangleGenerator))]
        public void Rect_Rect_Collision(Rectangle r, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, _unitRectangle.Intersects(r));
        }

        [Theory]
        [ClassData(typeof(BoundingAreaGenerator))]
        public void Rect_Circle_Collision(IBoundingArea b, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, b.Intersects(_unitRectangle));
            Assert.Equal(intersectionExpected, b.Intersects(_unitCircle));
        }

        [Theory]
        [ClassData(typeof(TriangleGenerator))]
        public void Triangle_Circle_Collision(Triangle t, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, t.Intersects(_unitCircle));
        }

        [Theory]
        [ClassData(typeof(TriangleGenerator))]
        public void Triangle_Rect_Collision(Triangle t, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, t.Intersects(_unitRectangle));
        }

        [Theory]
        [ClassData(typeof(TriangleGenerator))]
        public void Triangle_Triangle_Collision(Triangle t, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, t.Intersects(_unitTriangle));
        }

        [Theory]
        [ClassData(typeof(LineSegmentGenerator))]
        public void LineSegment_Circle_Collision(LineSegment l, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, l.Intersects(_unitCircle));
        }

        [Theory]
        [ClassData(typeof(LineSegmentGenerator))]
        public void Rect_LineSegment_Collision(LineSegment l, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, l.Intersects(_unitRectangle));
        }
    }
}
