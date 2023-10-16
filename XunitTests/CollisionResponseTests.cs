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

        [Theory]
        [ClassData(typeof(CollisionResponseGenerator))]
        public void Response(Circle movingObject, Vector2 target, IBoundingArea[] obstacles, Vector2 destination)
        {
            var treeSize = 2000;
            var quadTree = new QuadTree(left: -1000, top: 1000, width: treeSize, height: treeSize);
            var success = quadTree.InsertRange(obstacles);
            Assert.True(success);

            var actualDestination = movingObject.Move(quadTree, target);

            var updatedObject = new Circle() { Radius = movingObject.Radius, Center = actualDestination };
            var distance = Vector2.Distance(destination, actualDestination);

            foreach (var obs in obstacles) 
            {
                Assert.False(updatedObject.Intersects(obs));
            }

            //the precision on this one is not very tight
            Assert.True(distance < 0.1, $"expected destination {destination} but got {actualDestination} instead");
        }
    }
}
