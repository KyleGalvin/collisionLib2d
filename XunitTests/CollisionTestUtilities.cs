using LongHorse.CollisionLib2D;
using System.Collections;
using System.Collections.Generic;

namespace XunitTests
{
    public class CircleGenerator : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {

            //the trailing boolean indicates whether we expect this to intersect with a unit circle or not.

            //unit circle
            yield return new object[] { new Circle() { Radius = 0.5f, CenterX = 0, CenterY = 0 }, true };

            //inside small
            yield return new object[] { new Circle() { Radius = 0.25f, CenterX = 0, CenterY = 0 }, true };

            //inside big
            yield return new object[] { new Circle() { Radius = 10.0f, CenterX = 0, CenterY = 0 }, true };

            //intersecting ( 8 edges and corners )
            yield return new object[] { new Circle() { Radius = 2.0f, CenterX = 1, CenterY = 1 }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, CenterX = 1, CenterY = 0 }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, CenterX = 1, CenterY = -1 }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, CenterX = 0, CenterY = 1 }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, CenterX = 0, CenterY = -1 }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, CenterX = -1, CenterY = 1 }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, CenterX = -1, CenterY = 0 }, true };
            yield return new object[] { new Circle() { Radius = 2.0f, CenterX = -1, CenterY = -1 }, true };

            //not intersecting ( 8 edges and corners )
            yield return new object[] { new Circle() { Radius = 0.9f, CenterX = 2, CenterY = 2 }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, CenterX = 2, CenterY = 0 }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, CenterX = 2, CenterY = -2 }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, CenterX = 0, CenterY = 2 }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, CenterX = 0, CenterY = -2 }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, CenterX = -2, CenterY = 2 }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, CenterX = -2, CenterY = 0 }, false };
            yield return new object[] { new Circle() { Radius = 0.9f, CenterX = -2, CenterY = -2 }, false };
        }
    }

    public class RectangleGenerator : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator()
        {

            //the trailing boolean indicates whether we expect this to intersect with a unit rectangle or not.

            //unit rectangle 
            yield return new object[] { new Rectangle() { Width = 1.0f, Height = 1.0f, CenterX = 0.0f, CenterY = 0.0f }, true };

            //inside small
            yield return new object[] { new Rectangle() { Width = 0.5f, Height = 0.5f, CenterX = 0.0f, CenterY = 0.0f }, true };

            //inside big
            yield return new object[] { new Rectangle() { Width = 10.0f, Height = 10.0f, CenterX = 0.0f, CenterY = 0.0f }, true };


            //intersecting ( 8 edges and corners )
            yield return new object[] { new Rectangle() { Width = 2.0f, Height = 2.0f, CenterX = 1.0f, CenterY = 1.0f }, true };
            yield return new object[] { new Rectangle() { Width = 2.0f, Height = 2.0f, CenterX = 1.0f, CenterY = -0.5f }, true };
            yield return new object[] { new Rectangle() { Width = 2.0f, Height = 2.0f, CenterX = 1.0f, CenterY = -1.0f }, true };
            yield return new object[] { new Rectangle() { Width = 2.0f, Height = 2.0f, CenterX = 0.0f, CenterY = 1.0f }, true };
            yield return new object[] { new Rectangle() { Width = 2.0f, Height = 2.0f, CenterX = 0.0f, CenterY = -1.0f }, true };
            yield return new object[] { new Rectangle() { Width = 2.0f, Height = 2.0f, CenterX = -1.0f, CenterY = 1.0f }, true };
            yield return new object[] { new Rectangle() { Width = 2.0f, Height = 2.0f, CenterX = -1.0f, CenterY = -0.5f }, true };
            yield return new object[] { new Rectangle() { Width = 2.0f, Height = 2.0f, CenterX = -1.0f, CenterY = -1.0f }, true };

            //not intersecting ( 8 edges and corners )
            yield return new object[] { new Rectangle() { Width = 0.9f, Height = 0.9f, CenterX = 1.0f, CenterY = 1.0f }, false };
            yield return new object[] { new Rectangle() { Width = 0.9f, Height = 0.9f, CenterX = 1.0f, CenterY = -0.0f }, false };
            yield return new object[] { new Rectangle() { Width = 0.9f, Height = 0.9f, CenterX = 1.0f, CenterY = -1.5f }, false };
            yield return new object[] { new Rectangle() { Width = 0.9f, Height = 0.9f, CenterX = -0.0f, CenterY = 1.0f }, false };
            yield return new object[] { new Rectangle() { Width = 0.9f, Height = 0.9f, CenterX = -0.0f, CenterY = -1.0f }, false };
            yield return new object[] { new Rectangle() { Width = 0.9f, Height = 0.9f, CenterX = -1.0f, CenterY = 1.0f }, false };
            yield return new object[] { new Rectangle() { Width = 0.9f, Height = 0.9f, CenterX = -1.0f, CenterY = -0.0f }, false };
            yield return new object[] { new Rectangle() { Width = 0.9f, Height = 0.9f, CenterX = -1.0f, CenterY = -1.5f }, false };
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
