using LongHorse.CollisionLib2D;
using LongHorse.CollisionLib2D.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Xunit;

namespace XunitTests
{
    [Trait("Module", "SpatialPartitioning")]
    [Trait("Category", "Integration")]
    public class QuadTreeTests
    {
        public static Rectangle _unitRectangle = new Rectangle() { Size = new Vector2(1.0f, 1.0f), Center = new Vector2(0.0f, 0.0f) };
        public static Circle _unitCircle = new Circle() { Radius = 0.5f, Center = new Vector2(0.0f, 0.0f) };

        [Fact]
        public void QuadTree_Overlapping_Neighbors()
        {
            var numberOfObjects = 10;
            var treeSize = 500;
            var quadTree = new QuadTree(treeSize, treeSize);

            for (var i = 0; i < numberOfObjects; i++)
            {
                var _unitRectCopy = new Rectangle() { Size = _unitRectangle.Size, Center = _unitRectangle.Center };
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
                var _unitRectCopy = new Rectangle() { Size = _unitRectangle.Size, Center = _unitRectangle.Center };
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
                quadTree.Insert(new Rectangle() { Size = _unitRectangle.Size, Center = new Vector2(position1, position1) });
                quadTree.Insert(new Rectangle() { Size = _unitRectangle.Size, Center = new Vector2(position2, position2) });
            }

            var neighbors1 = quadTree.FindObjects(new Rectangle() { Size = _unitRectangle.Size, Center = new Vector2(position1, position1) });
            var neighbors2 = quadTree.FindObjects(new Rectangle() { Size = _unitRectangle.Size, Center = new Vector2(position2, position2) });

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
                quadTree.Insert(new Rectangle() { Size = _unitRectangle.Size, Center = new Vector2(position1, position1) });
                quadTree.Insert(new Rectangle() { Size = _unitRectangle.Size, Center = new Vector2(position2, position2) });
            }

            //this big item covers both piles. (The entire tree area)
            var bigRect = new Rectangle() { Size = new Vector2(treeSize, treeSize), Center = new Vector2(250, 250) };
            quadTree.Insert(bigRect);

            var neighbors1 = quadTree.FindObjects(new Rectangle() { Size = _unitRectangle.Size, Center = new Vector2(position1, position1) });
            var neighbors2 = quadTree.FindObjects(new Rectangle() { Size = _unitRectangle.Size, Center = new Vector2(position2, position2) });

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
