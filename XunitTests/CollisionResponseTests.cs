using LongHorse.CollisionLib2D;
using LongHorse.CollisionLib2D.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XunitTests
{
    [Trait("Module", "Response")]
    [Trait("Category", "Integration")]
    public class CollisionResponseTests
    {

        [Fact]
        public void Response()
        {
            //set up a quadtree with a single collidable rect in it
            var treeSize = 500;
            var quadTree = new QuadTree(treeSize, treeSize);
            var collisionObject = new Rectangle() { Center = new System.Numerics.Vector2(2, 0), Size = new System.Numerics.Vector2(1, 1) };
            quadTree.Insert(collisionObject);

            //create a moving object we will collide into the rect
            var movingObject = new Circle() { Radius = 0.5f, Center = new System.Numerics.Vector2(0,0)};
            var destination = movingObject.Move(quadTree, new System.Numerics.Vector2(2, 0));

            var expectedDestination = new Vector2(1, 0);
            var updatedObject = new Circle() { Radius = 0.5f, Center = destination };
            var distance = Vector2.Distance(destination, expectedDestination);

            Assert.False(updatedObject.Intersects(collisionObject));
            Assert.True(distance < 0.00001);
        }
    }
}
