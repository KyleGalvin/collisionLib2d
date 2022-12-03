using LongHorse.CollisionLib2D.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace XunitTests
{
    public class CircleGenerator : IEnumerable<object[]>
    {


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {

            //the trailing boolean indicates whether we expect this to intersect with a unit circle or not.

            //unit circle
            yield return new object[] { new Circle() { Radius = 0.5f, Center = new Vector2(0.0f, 0.0f) }, true };

            //inside small
            yield return new object[] { new Circle() { Radius = 0.25f, Center = new Vector2(0.0f, 0.0f) }, true };

            //inside big
            yield return new object[] { new Circle() { Radius = 10.0f, Center = new Vector2(0.0f, 0.0f) }, true };

            //intersecting ( 8 edges and corners )
            yield return new object[] { new Circle() { Radius = 2.0f, Center = new Vector2(1.0f, 1.0f) }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, Center = new Vector2(1.0f, 0.0f) }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, Center = new Vector2(1.0f, -1.0f) }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, Center = new Vector2(0.0f, 1.0f) }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, Center = new Vector2(0.0f, -1.0f) }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, Center = new Vector2(-1.0f, 1.0f) }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, Center = new Vector2(-1.0f, 0.0f) }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, Center = new Vector2(-1.0f, -1.0f) }, true };

            //not intersecting ( 8 edges and corners )
            yield return new object[] { new Circle() { Radius = 0.9f, Center = new Vector2(2.0f, 2.0f) }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, Center = new Vector2(2.0f, 0.0f) }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, Center = new Vector2(2.0f, -2.0f) }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, Center = new Vector2(0.0f, 2.0f) }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, Center = new Vector2(0.0f, -2.0f) }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, Center = new Vector2(-2.0f, 2.0f) }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, Center = new Vector2(-2.0f, 0.0f) }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, Center = new Vector2(-2.0f, -2.0f) }, false };
        }
    }

    public class TriangleGenerator : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {

            //the trailing boolean indicates whether we expect this to intersect with a unit circle or not. Also works for unit square

            //inside small
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(-0.25f, 0.0f), new Vector2(0.25f, 0.0f), new Vector2(0.0f, 0.25f) } }, true };

            //inside big
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(-10.0f, -10.0f), new Vector2(10.0f, 10.0f), new Vector2(0.0f, 20.0f) } }, true };

            //partially intersecting
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 0.0f), new Vector2(20.0f, 20.0f), new Vector2(20.0f, -20.0f) } }, true };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 0.0f), new Vector2(-20.0f, 20.0f), new Vector2(-20.0f, -20.0f) } }, true };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 0.0f), new Vector2(-20.0f, -20.0f), new Vector2(20.0f, -20.0f) } }, true };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 0.0f), new Vector2(-20.0f, 20.0f), new Vector2(20.0f, 20.0f) } }, true };

            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 0.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, -20.0f) } }, true };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 0.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, 20.0f) } }, true };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 0.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, -20.0f) } }, true };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 0.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, 20.0f) } }, true };

            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(20.0f, 0.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, 20.0f) } }, true };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(20.0f, 0.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, -20.0f) } }, true };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 20.0f), new Vector2(0.0f, -20.0f), new Vector2(-20.0f, 0.0f) } }, true };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 20.0f), new Vector2(0.0f, -20.0f), new Vector2(20.0f, 0.0f) } }, true };

            //non-intersecting
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(2.0f, 0.0f), new Vector2(20.0f, 20.0f), new Vector2(20.0f, -20.0f) } }, false };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(-2.0f, 0.0f), new Vector2(-20.0f, 20.0f), new Vector2(-20.0f, -20.0f) } }, false };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, -2.0f), new Vector2(-20.0f, -20.0f), new Vector2(20.0f, -20.0f) } }, false };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(0.0f, 2.0f), new Vector2(-20.0f, 20.0f), new Vector2(20.0f, 20.0f) } }, false };

            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(2.0f, -2.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, -20.0f) } }, false };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(2.0f, 2.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, 20.0f) } }, false };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(-2.0f, -2.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, -20.0f) } }, false };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(-2.0f, 2.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, 20.0f) } }, false };

            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(20.0f, 2.0f), new Vector2(-20.0f, 2.0f), new Vector2(0.0f, 20.0f) } }, false };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(20.0f, -2.0f), new Vector2(-20.0f, -2.0f), new Vector2(0.0f, -20.0f) } }, false };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(-2.0f, 20.0f), new Vector2(-2.0f, -20.0f), new Vector2(-20.0f, 0.0f) } }, false };
            yield return new object[] { new Triangle() { Points = new Vector2[] { new Vector2(2.0f, 20.0f), new Vector2(2.0f, -20.0f), new Vector2(20.0f, 0.0f) } }, false };
        }
    }

    public class RectangleGenerator : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {

            //the trailing boolean indicates whether we expect this to intersect with a unit rectangle or not.

            //unit rectangle 
            yield return new object[] { new Rectangle() { Size = new Vector2(1.0f, 1.0f), Center = new Vector2(0.0f, 0.0f) }, true };

            //inside small
            yield return new object[] { new Rectangle() { Size = new Vector2(0.5f, 0.5f), Center = new Vector2(0.0f, 0.0f) }, true };

            //inside big
            yield return new object[] { new Rectangle() { Size = new Vector2(10.0f, 10.0f), Center = new Vector2(0.0f, 0.0f) }, true };


            //intersecting ( 8 edges and corners )
            yield return new object[] { new Rectangle() { Size = new Vector2(2.0f, 2.0f), Center = new Vector2(1.0f, 1.0f) }, true };
            yield return new object[] { new Rectangle() { Size = new Vector2(2.0f, 2.0f), Center = new Vector2(1.0f, -0.5f) }, true };
            yield return new object[] { new Rectangle() { Size = new Vector2(2.0f, 2.0f), Center = new Vector2(1.0f, -1.0f) }, true };
            yield return new object[] { new Rectangle() { Size = new Vector2(2.0f, 2.0f), Center = new Vector2(0.0f, 1.0f) }, true };
            yield return new object[] { new Rectangle() { Size = new Vector2(2.0f, 2.0f), Center = new Vector2(0.0f, -1.0f) }, true };
            yield return new object[] { new Rectangle() { Size = new Vector2(2.0f, 2.0f), Center = new Vector2(-1.0f, 1.0f) }, true };
            yield return new object[] { new Rectangle() { Size = new Vector2(2.0f, 2.0f), Center = new Vector2(-1.0f, -0.5f) }, true };
            yield return new object[] { new Rectangle() { Size = new Vector2(2.0f, 2.0f), Center = new Vector2(-1.0f, -1.0f) }, true };

            //not intersecting ( 8 edges and corners )
            yield return new object[] { new Rectangle() { Size = new Vector2(0.9f, 0.9f), Center = new Vector2(1.0f, 1.0f) }, false };
            yield return new object[] { new Rectangle() { Size = new Vector2(0.9f, 0.9f), Center = new Vector2(1.0f, 0.0f) }, false };
            yield return new object[] { new Rectangle() { Size = new Vector2(0.9f, 0.9f), Center = new Vector2(1.0f, -1.5f) }, false };
            yield return new object[] { new Rectangle() { Size = new Vector2(0.9f, 0.9f), Center = new Vector2(0.0f, 1.0f) }, false };
            yield return new object[] { new Rectangle() { Size = new Vector2(0.9f, 0.9f), Center = new Vector2(0.0f, -1.0f) }, false };
            yield return new object[] { new Rectangle() { Size = new Vector2(0.9f, 0.9f), Center = new Vector2(-1.0f, 1.0f) }, false };
            yield return new object[] { new Rectangle() { Size = new Vector2(0.9f, 0.9f), Center = new Vector2(-1.0f, 0.0f) }, false };
            yield return new object[] { new Rectangle() { Size = new Vector2(0.9f, 0.9f), Center = new Vector2(-1.0f, -1.5f) }, false };
        }
    }

    public class LineSegmentGenerator : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {

            //the trailing boolean indicates whether we expect this to intersect with a unit circle or not.

            //line entirely in circle
            yield return new object[] { new LineSegment(new Vector2(0.05f, 0.0f), new Vector2(0.0f, 0.05f)), true };

            //diagonal intersections
            yield return new object[] { new LineSegment(new Vector2(0.6f, 0.0f), new Vector2(0.0f, 0.6f)), true };
            yield return new object[] { new LineSegment(new Vector2(-0.6f, 0.0f), new Vector2(0.0f, 0.6f)), true };
            yield return new object[] { new LineSegment(new Vector2(-0.6f, 0.0f), new Vector2(0.0f, -0.6f)), true };
            yield return new object[] { new LineSegment(new Vector2(0.6f, 0.0f), new Vector2(0.0f, -0.6f)), true };

            //axis aligned intersections
            yield return new object[] { new LineSegment(new Vector2(0.25f, 1.0f), new Vector2(0.25f, -1.0f)), true };
            yield return new object[] { new LineSegment(new Vector2(-0.25f, 1.0f), new Vector2(-0.25f, -1.0f)), true };
            yield return new object[] { new LineSegment(new Vector2(1.0f, 0.25f), new Vector2(-1.0f, 0.25f)), true };
            yield return new object[] { new LineSegment(new Vector2(1.0f, -0.25f), new Vector2(-1.0f, -0.25f)), true };

            //diagonal non-intersections
            yield return new object[] { new LineSegment(new Vector2(1.75f, 0.0f), new Vector2(0.0f, 1.75f)), false };
            yield return new object[] { new LineSegment(new Vector2(-1.75f, 0.0f), new Vector2(0.0f, 1.75f)), false };
            yield return new object[] { new LineSegment(new Vector2(-1.75f, 0.0f), new Vector2(0.0f, -1.75f)), false };
            yield return new object[] { new LineSegment(new Vector2(1.75f, 0.0f), new Vector2(0.0f, -1.75f)), false };

            //axis aligned non-intersections
            yield return new object[] { new LineSegment(new Vector2(0.75f, 0.0f), new Vector2(0.75f, 0.0f)), false };
            yield return new object[] { new LineSegment(new Vector2(-0.75f, 0.0f), new Vector2(-0.75f, 0.0f)), false };
            yield return new object[] { new LineSegment(new Vector2(0.0f, 0.75f), new Vector2(0.0f, 0.75f)), false };
            yield return new object[] { new LineSegment(new Vector2(0.0f, -0.75f), new Vector2(0.0f, -0.75f)), false };

        }
    }

    public class BoundingAreaGenerator : IEnumerable<object[]>
    {
        private CircleGenerator _circleGenerator = new CircleGenerator();
        private RectangleGenerator _rectangleGenerator = new RectangleGenerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {
            foreach(var boundingArea in _circleGenerator)
            {
                yield return boundingArea;
            }

            foreach (var boundingArea in _rectangleGenerator)
            {
                yield return boundingArea;
            }
        }
    }

    public class NearestPointToTriangleGenerator : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {
            // a right angle triangle with edges of length 3, 4, and 5.
            // the right angled point is at the origin
            var triangle345 = new Triangle { 
                Points = new Vector2[] { 
                    new Vector2(0.0f, 0.0f),
                    new Vector2(3.0f, 0.0f),
                    new Vector2(0.0f, 4.0f)
                } 
            };

            //nearest points touching the triangle while in the triangle is the input point
            yield return new object[] { triangle345, new Vector2(1.0f, 1.0f), new Vector2(1.0f, 1.0f) };
            yield return new object[] { triangle345, new Vector2(3.0f, 0.0f), new Vector2(3.0f, 0.0f) };
            yield return new object[] { triangle345, new Vector2(0.0f, 0.0f), new Vector2(0.0f, 0.0f) };
            yield return new object[] { triangle345, new Vector2(0.0f, 4.0f), new Vector2(0.0f, 4.0f) };

            //nearest point outside of the triangle can be the triangle vertecies
            yield return new object[] { triangle345, new Vector2(4.0f, -1.0f), new Vector2(3.0f, 0.0f) };
            yield return new object[] { triangle345, new Vector2(-1.0f, -1.0f), new Vector2(0.0f, 0.0f) };
            yield return new object[] { triangle345, new Vector2(-1.0f, 5.0f), new Vector2(0.0f, 4.0f) };

            // nearest point outside of the triangle can be along the triangle edges
            yield return new object[] { triangle345, new Vector2(2.0f, -1.0f), new Vector2(2.0f, 0.0f) };
            yield return new object[] { triangle345, new Vector2(-1.0f, 1.5f), new Vector2(0.0f, 1.5f) };
            yield return new object[] { triangle345, new Vector2(2.0f, 2.0f), new Vector2(1.68f, 1.76f) };
        }
    }

}
