using LongHorse.CollisionLib2D;
using LongHorse.CollisionLib2D.Primitives;
using System.Numerics;
using Xunit;

namespace XunitTests
{
    [Trait("Module", "Geometry")]
    [Trait("Category", "Unit")]
    public class NearestPointTests
    {
        Rectangle _unitRectangle = new Rectangle() { Size = new Vector2(1.0f, 1.0f), Center = new Vector2(0.0f, 0.0f) };
        Circle _unitCircle = new Circle() { Radius = 0.5f, Center = new Vector2(0.0f, 0.0f) };

        [Theory]
        [ClassData(typeof(NearestPointToTriangleGenerator))]
        public void Triangle_Nearest_Point(Triangle t, Vector2 p, Vector2 nearestPoint)
        {
            // floating point error means we have to check that the input and output is 'close'.
            // so we subtract the two points and check that the length of the difference is sufficiently small

            // Assert.Equal(nearestPoint, p.NearestPoint(t));
            var delta = p.NearestPoint(t) - nearestPoint;
            var squaredLength = delta.LengthSquared();
            Assert.True(squaredLength < 0.00001);
        }

        [Fact]
        public void Circle_Nearest_Point()
        {

            //the center of the circle is the nearest point to itself
            var p = new Vector2(0.0f, 0.0f);
            var nearestPoint = p.NearestPoint(_unitCircle);
            var distance = nearestPoint.Length();
            Assert.Equal(0.0f, distance);

            //the edge of the circle is the nearest point to itself
            p = new Vector2(0.5f, 0.0f);
            nearestPoint = p.NearestPoint(_unitCircle);
            distance = nearestPoint.Length();
            Assert.Equal(0.5f, distance);

            //the edge of the circle is the nearest point
            p = new Vector2(2.0f, 0.0f);
            nearestPoint = p.NearestPoint(_unitCircle);
            distance = nearestPoint.Length();
            Assert.Equal(0.5f, distance);

            //any point outside of the circle should have a nearest point distance of 0.5
            //but floating point precision kicks in when we dont choose nice even numbers along the axis
            p = new Vector2(2.0f, 2.0f);
            nearestPoint = p.NearestPoint(_unitCircle);
            distance = nearestPoint.Length();
            Assert.True(distance - 0.5f < 0.00001);
        }
    }
}
