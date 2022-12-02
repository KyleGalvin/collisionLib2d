using LongHorse.CollisionLib2D;
using LongHorse.CollisionLib2D.Primitives;
using System.Numerics;
using Xunit;

namespace XunitTests
{
    public class NearestPointTests
    {
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
    }
}
