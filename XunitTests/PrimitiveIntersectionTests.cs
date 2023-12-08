using LongHorse.CollisionLib2D;
using LongHorse.CollisionLib2D.Primitives;
using System.Numerics;
using Xunit;

namespace XunitTests
{
    [Trait("Module", "Collision")]
    [Trait("Module", "Geometry")]
    [Trait("Category", "Unit")]
    public class PrimitiveIntersectionTests
    {
        Rectangle _unitRectangle = new Rectangle() { Size = new Vector2(1.0f, 1.0f), Center = new Vector2(0.0f, 0.0f) };
        Circle _unitCircle = new Circle() { Radius = 0.5f, Center = new Vector2(0.0f, 0.0f) };
        Triangle _unitTriangle = new Triangle(new Vector2(-1.0f, -1.0f), new Vector2(0.0f, 1.0f), new Vector2(1.0f, -1.0f));

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


        [Fact]
        public void Triangle_LineSegment_Collision()
        {
            var triangle345 = new Triangle(
                new Vector2(0.0f, 0.0f),
                new Vector2(3.0f, 0.0f),
                new Vector2(0.0f, 4.0f));

            //through a side
            var line = new LineSegment(new Vector2(-1.0f, 1.0f), new Vector2(0.5f, 1.0f));
            Assert.True(line.Intersects(triangle345));

            //across a corner
            line = new LineSegment(new Vector2(-0.5f, 1.0f), new Vector2(1.0f, -0.5f));
            Assert.True(line.Intersects(triangle345));

            //through a corner (but not a side)
            line = new LineSegment(new Vector2(-0.025f, -0.025f), new Vector2(0.025f, 0.025f));
            Assert.True(line.Intersects(triangle345));

            //fully inside the triangle
            line = new LineSegment(new Vector2(1.0f, 1.0f), new Vector2(1.1f, 1.1f));
            Assert.True(line.Intersects(triangle345));

            //parallel
            line = new LineSegment(new Vector2(-1.0f, 0.0f), new Vector2(-1.0f, 4.0f));
            Assert.False(line.Intersects(triangle345));

        }

        [Theory]
        [ClassData(typeof(LineSegmentGenerator))]
        public void LineSegment_Circle_Collision(LineSegment l, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, l.Intersects(_unitCircle));
        }

        [Fact]
        public void LineSegment_LineSegment_Collision()
        {
            var l1 = new LineSegment(new Vector2(1.0f, 0.0f), new Vector2(-1.0f, 0.0f));
            var l2 = new LineSegment(new Vector2(1.0f, 1.0f), new Vector2(-1.0f, 1.0f));
            var l3 = new LineSegment(new Vector2(0.5f, 1.0f), new Vector2(0.5f, -1.0f));
            Assert.False(l1.Intersects(l2));
            Assert.False(l2.Intersects(l1));
            Assert.True(l3.Intersects(l1));
            Assert.True(l3.Intersects(l2));
            Assert.True(l2.Intersects(l3));
            Assert.True(l1.Intersects(l3));
        }

        [Theory]
        [ClassData(typeof(LineSegmentGenerator))]
        public void Rect_LineSegment_Collision(LineSegment l, bool intersectionExpected)
        {
            Assert.Equal(intersectionExpected, l.Intersects(_unitRectangle));
        }
    }
}
