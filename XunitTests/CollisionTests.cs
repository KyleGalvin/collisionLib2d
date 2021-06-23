using LongHorse.CollisionLib2D;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XunitTests
{
    [Trait("Module", "Collision")]
    [Trait("Category", "Unit")]
    public class CollisionTests
    {
        Rectangle _unitRectangle = new Rectangle() { Width = 1.0f, Height = 1.0f, CenterX = 0.0f, CenterY = 0.0f };
        Circle _unitCircle = new Circle() { Radius = 0.5f, CenterX = 0, CenterY = 0 };

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

        [Fact]
        public void QuadTree_Overlapping_Neighbors()
        {
            var numberOfObjects = 10;
            var treeSize = 500;
            var quadTree = new QuadTree(treeSize, treeSize);

            for (var i = 0; i < numberOfObjects; i++)
            {
                var _unitRectCopy = new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = _unitRectangle.CenterX, CenterY = _unitRectangle.CenterY };
                quadTree.Insert(_unitRectCopy);
            }

            var neighbors = quadTree.FindObjects(_unitRectangle);
            Assert.Equal(numberOfObjects, neighbors.Length);
        }

        [Fact]
        public void QuadTree_MaxObjects_Exceeded()
        {
            var numberOfObjects = 10;
            var treeSize = 500;
            var quadTree = new QuadTree(treeSize, treeSize, maxObjects: 1, maxLevel: 1);

            for (var i = 0; i < numberOfObjects; i++)
            {
                var _unitRectCopy = new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = _unitRectangle.CenterX, CenterY = _unitRectangle.CenterY };
                quadTree.Insert(_unitRectCopy);
            }

            //maxObjects is a 'best effort' at diving to a deeper level to subdivide neighbors.
            //when put in a scenario where both maxObjects and maxLevel cannot both be satisfied,
            //maxObjects loses out.
            var neighbors = quadTree.FindObjects(_unitRectangle);
            Assert.Equal(numberOfObjects, neighbors.Length);
        }

        [Fact]
        public void QuadTree_Distant_Neighbors_Excluded()
        {
            var numberOfObjects = 10;
            var treeSize = 500;
            var quadTree = new QuadTree(treeSize, treeSize);

            var position1 = 125;
            var position2 = 375;

            //20 objects in 2 distant piles of 10.
            for (var i = 0; i < numberOfObjects; i++)
            {
                quadTree.Insert(new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = position1, CenterY = position1 });
                quadTree.Insert(new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = position2, CenterY = position2 });
            }

            var neighbors1 = quadTree.FindObjects(new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = position1, CenterY = position1 });
            var neighbors2 = quadTree.FindObjects(new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = position2, CenterY = position2 });

            var n1List = new List<IBoundingArea>();
            n1List.AddRange(neighbors1);
            var n2List = new List<IBoundingArea>();
            n2List.AddRange(neighbors2);

            //each FindObjects call gets just the local neighbors, and not the 'other' set we added.
            Assert.Equal(numberOfObjects, neighbors1.Length);
            Assert.Equal(numberOfObjects, neighbors2.Length);
            Assert.Empty(n1List.Intersect(n2List));
        }

        [Fact]
        public void QuadTree_Big_Size_Overlapping_Small()
        {
            var numberOfObjects = 10;
            var treeSize = 500;
            var quadTree = new QuadTree(treeSize, treeSize);

            var position1 = 125;
            var position2 = 375;

            //20 objects in 2 distant piles of 10.
            for (var i = 0; i < numberOfObjects; i++)
            {
                quadTree.Insert(new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = position1, CenterY = position1 });
                quadTree.Insert(new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = position2, CenterY = position2 });
            }

            //this big item covers both piles. (The entire tree area)
            var bigRect = new Rectangle() { Width = treeSize, Height = treeSize, CenterX = 250, CenterY = 250 };
            quadTree.Insert(bigRect);

            var neighbors1 = quadTree.FindObjects(new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = position1, CenterY = position1 });
            var neighbors2 = quadTree.FindObjects(new Rectangle() { Width = _unitRectangle.Width, Height = _unitRectangle.Height, CenterX = position2, CenterY = position2 });

            var bigNeighbors = quadTree.FindObjects(bigRect);

            var n1List = new List<IBoundingArea>();
            n1List.AddRange(neighbors1);
            var n2List = new List<IBoundingArea>();
            n2List.AddRange(neighbors2);

            //each FindObjects call gets just the local neighbors, and not the 'other' set we added.
            Assert.Equal(numberOfObjects + 1, neighbors1.Length);
            Assert.Equal(numberOfObjects + 1, neighbors2.Length);
            //querying the entire map should give us all objects, plus the 'big neighbor'
            Assert.Equal((numberOfObjects * 2) + 1, bigNeighbors.Length);

            //the two neighborhood sets both have the 'big neighbor' in common
            Assert.Single(n1List.Intersect(n2List));
        }

    }
}
