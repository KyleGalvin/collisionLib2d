using LongHorse.CollisionLib2D;
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

}
