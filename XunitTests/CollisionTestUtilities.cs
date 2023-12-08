using LongHorse.CollisionLib2D;
using LongHorse.CollisionLib2D.Primitives;
using System;
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
            yield return new object[] { new Triangle(new Vector2(-0.25f, 0.0f), new Vector2(0.25f, 0.0f), new Vector2(0.0f, 0.25f)), true };

            //inside big
            yield return new object[] { new Triangle(new Vector2(-10.0f, -10.0f), new Vector2(10.0f, 10.0f), new Vector2(0.0f, 20.0f)), true };

            //partially intersecting
            yield return new object[] { new Triangle(new Vector2(0.0f, 0.0f), new Vector2(20.0f, 20.0f), new Vector2(20.0f, -20.0f)), true };
            yield return new object[] { new Triangle(new Vector2(0.0f, 0.0f), new Vector2(-20.0f, 20.0f), new Vector2(-20.0f, -20.0f)), true };
            yield return new object[] { new Triangle(new Vector2(0.0f, 0.0f), new Vector2(-20.0f, -20.0f), new Vector2(20.0f, -20.0f)), true };
            yield return new object[] { new Triangle(new Vector2(0.0f, 0.0f), new Vector2(-20.0f, 20.0f), new Vector2(20.0f, 20.0f)), true };

            yield return new object[] { new Triangle(new Vector2(0.0f, 0.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, -20.0f)), true };
            yield return new object[] { new Triangle(new Vector2(0.0f, 0.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, 20.0f)), true };
            yield return new object[] { new Triangle(new Vector2(0.0f, 0.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, -20.0f)), true };
            yield return new object[] { new Triangle(new Vector2(0.0f, 0.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, 20.0f)), true };

            yield return new object[] { new Triangle(new Vector2(20.0f, 0.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, 20.0f)), true };
            yield return new object[] { new Triangle(new Vector2(20.0f, 0.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, -20.0f)), true };
            yield return new object[] { new Triangle(new Vector2(0.0f, 20.0f), new Vector2(0.0f, -20.0f), new Vector2(-20.0f, 0.0f)), true };
            yield return new object[] { new Triangle(new Vector2(0.0f, 20.0f), new Vector2(0.0f, -20.0f), new Vector2(20.0f, 0.0f)), true };

            //non-intersecting
            yield return new object[] { new Triangle(new Vector2(2.0f, 0.0f), new Vector2(20.0f, 20.0f), new Vector2(20.0f, -20.0f)), false };
            yield return new object[] { new Triangle(new Vector2(-2.0f, 0.0f), new Vector2(-20.0f, 20.0f), new Vector2(-20.0f, -20.0f)), false };
            yield return new object[] { new Triangle(new Vector2(0.0f, -2.0f), new Vector2(-20.0f, -20.0f), new Vector2(20.0f, -20.0f)), false };
            yield return new object[] { new Triangle(new Vector2(0.0f, 2.0f), new Vector2(-20.0f, 20.0f), new Vector2(20.0f, 20.0f)), false };

            yield return new object[] { new Triangle(new Vector2(2.0f, -2.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, -20.0f)), false };
            yield return new object[] { new Triangle(new Vector2(2.0f, 2.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, 20.0f)), false };
            yield return new object[] { new Triangle(new Vector2(-2.0f, -2.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, -20.0f)), false };
            yield return new object[] { new Triangle(new Vector2(-2.0f, 2.0f), new Vector2(-20.0f, 0.0f), new Vector2(0.0f, 20.0f)), false };

            yield return new object[] { new Triangle(new Vector2(20.0f, 2.0f), new Vector2(-20.0f, 2.0f), new Vector2(0.0f, 20.0f)), false };
            yield return new object[] { new Triangle(new Vector2(20.0f, -2.0f), new Vector2(-20.0f, -2.0f), new Vector2(0.0f, -20.0f)), false };
            yield return new object[] { new Triangle( new Vector2(-2.0f, 20.0f), new Vector2(-2.0f, -20.0f), new Vector2(-20.0f, 0.0f)), false };
            yield return new object[] { new Triangle(new Vector2(2.0f, 20.0f), new Vector2(2.0f, -20.0f), new Vector2(20.0f, 0.0f)), false };
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

    public class CollisionResponseGenerator : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] 
            { 
                new Circle() { Radius = 0.5f, Center = new Vector2(0, 0) },
                new Vector2(2, 0),
                new IBoundingArea[] 
                {
                    new Rectangle() { Center = new Vector2(2, 0), Size = new Vector2(1, 1) }
                },
                new Vector2(1,0)
            };

            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(0, 0) },
                new Vector2(2, 0),
                new IBoundingArea[]
                {
                    new Triangle(new Vector2(1, 1), new Vector2(1, -1), new Vector2(2, 0)),
                },
                new Vector2(0.5f,0)
            };

            //Circle can go straight through a line segment (or any shape) if it clears it all at once
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(0, 0) },
                new Vector2(2, 0),
                new IBoundingArea[]
                {
                    new LineSegment(new Vector2(1, 1), new Vector2(1, -1)),
                },
                new Vector2(2,0)
            };

            //straight collision perpendicular to a wall stops dead.
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(0, 0) },
                new Vector2(2, 0),
                new IBoundingArea[]
                {
                    new LineSegment(new Vector2(2, 1), new Vector2(2, -1)),
                },
                new Vector2(1.5f,0)
            };

            //values in the -x quadrant to check the vector math
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(-5, 0) },
                new Vector2(-3, 0),
                new IBoundingArea[]
                {
                    new LineSegment(new Vector2(-3, 1), new Vector2(-3, -1)),
                },
                new Vector2(-3.5f,0)
            };

            //collide with the end of a line segment head-on
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(0, 0) },
                new Vector2(2, 0),
                new IBoundingArea[]
                {
                    new LineSegment(new Vector2(1, 0), new Vector2(3, 0)),
                },
                new Vector2(0.5f,0)
            };

            //line segment points reversed from previous
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(0, 0) },
                new Vector2(2, 0),
                new IBoundingArea[]
                {
                    new LineSegment(new Vector2(3, 0), new Vector2(1, 0)),
                },
                new Vector2(0.5f,0)
            };

            //collide equally on two points should come to a full stop
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(0, 0) },
                new Vector2(2, 0),
                new IBoundingArea[]
                {
                    new LineSegment(new Vector2(1, 0), new Vector2(3, 1)),
                    new LineSegment(new Vector2(1, 0), new Vector2(3, -1)),
                },
                new Vector2(0.5f,0)
            };

            //multi-collision does not go through second obj (deflected twice)
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(-3, 0) },
                new Vector2(7, 0),
                new IBoundingArea[]
                {
                    new Triangle(new Vector2(-10, -10), new Vector2(10, 10), new Vector2(10,-10)),
                    new Rectangle(1.0f, 10.0f, 5.0f, 10.0f),
                },
                new Vector2(0.5f,1.20712f)
            };

            //multi-collision does not go through second obj (deflected then stopped)
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(0, 0.25f) },
                new Vector2(6, 0),
                new IBoundingArea[]
                {
                    new Triangle(new Vector2(0, -20), new Vector2(5, 0), new Vector2(8, 0)),
                    new Triangle(new Vector2(0, 20), new Vector2(5, 0), new Vector2(8, 0)),
                },
                new Vector2(4.5f,0)
            };

            //throw new NotImplementedException("Corner hits with multiple triangles are a snagging issue still");
            //multiple triangles at collision point doesnt snag
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(0, 3) },
                new Vector2(0, -3),
                new IBoundingArea[]
                {
                    new Triangle(new Vector2(10, -10), new Vector2(-10, 10), new Vector2(-10, -10)),
                    new Triangle(new Vector2(0, 0), new Vector2(10, -10), new Vector2(10, -5)),
                },
                new Vector2(1.41f,-0.14f)
            };

            //multiple triangles sliding over a collision point doesnt snag
            yield return new object[]
            {
                new Circle() { Radius = 0.5f, Center = new Vector2(-0.1f, 3) },
                new Vector2(-0.1f, -3),
                new IBoundingArea[]
                {
                    new Triangle(new Vector2(10, -10), new Vector2(-10, 10), new Vector2(-10, -10)),
                    new Triangle(new Vector2(0, 0), new Vector2(10, -10), new Vector2(10, -5)),
                },
                new Vector2(1.41f,-0.14f)
            };

            //Trouble case found in the wild.
            //Caused by floating point rounding errors when touching two objects simultaneously (in rare cases)
            yield return new object[]
            {
                new Circle() { Radius = 12.5f, Center = new Vector2(-682.5261f, 43.176544f) },
                new Vector2(-682.5261f, 53.176544f),
                new IBoundingArea[]
                {
                    new Triangle(new Vector2(-750.2f, -289.137f), new Vector2(-711.322f, -301.144f), new Vector2(-694.952f, 45.0391f)),
                    new Triangle(new Vector2(-750.2f, 403.228f), new Vector2(-750.2f, -289.137f), new Vector2(-694.952f, 45.0391f)),//present but unecessary
                    new Triangle(new Vector2(-750.2f, 403.228f), new Vector2(-694.952f, 45.0391f), new Vector2(-670.397f, 141.09f))
                },
                new Vector2(-680.26215f,52.032623f)
            };

            yield return new object[]
{
                new Circle() { Radius = 12.5f, Center = new Vector2(-682.5261f, 43.176514f) },
                new Vector2(-682.5261f, 53.176514f),
                new IBoundingArea[]
                {
                    new Triangle(new Vector2(-750.2f, -289.137f), new Vector2(-711.322f, -301.144f), new Vector2(-694.952f, 45.0391f)),
                    new Triangle(new Vector2(-750.2f, 403.228f), new Vector2(-750.2f, -289.137f), new Vector2(-694.952f, 45.0391f)),//present but unecessary
                    new Triangle(new Vector2(-750.2f, 403.228f), new Vector2(-694.952f, 45.0391f), new Vector2(-670.397f, 141.09f))
                },
                new Vector2(-680.2593f,52.043846f)
            };

            yield return new object[]
            {
                new Circle() { Radius = 12.5f, Center = new Vector2(-682.5261f, 43.17652f) },
                new Vector2(-682.5261f, 53.17652f),
                new IBoundingArea[]
                {
                    new Triangle(new Vector2(-750.2f, -289.137f), new Vector2(-711.322f, -301.144f), new Vector2(-694.952f, 45.0391f)),
                    new Triangle(new Vector2(-750.2f, 403.228f), new Vector2(-750.2f, -289.137f), new Vector2(-694.952f, 45.0391f)),//present but unecessary
                    new Triangle(new Vector2(-750.2f, 403.228f), new Vector2(-694.952f, 45.0391f), new Vector2(-670.397f, 141.09f))
                },
                new Vector2(-680.26215f,52.0326f)
            };
            
            //Single triangle problem
            yield return new object[]
            {
                new Circle() { Radius = 12.5f, Center = new Vector2(-478.6976f,318.96674f) },
                new Vector2(-488.6976f,318.96674f),
                new IBoundingArea[]
                {
                    new Triangle(new Vector2(-498.513f,325.187f),new Vector2(-430.987f,349.2f),new Vector2(-750.2f,403.228f)),
                },
                new Vector2(-487.57492f, 315.80978f)
            };


        }
    }

    public class NearestPointToTriangleGenerator : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {
            // a right angle triangle with edges of length 3, 4, and 5.
            // the right angled point is at the origin
            var triangle345 = new Triangle(
                    new Vector2(0.0f, 0.0f),
                    new Vector2(3.0f, 0.0f),
                    new Vector2(0.0f, 4.0f));

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
