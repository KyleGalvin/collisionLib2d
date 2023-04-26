using LongHorse.CollisionLib2D.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LongHorse.CollisionLib2D
{
    public static class CollisionResponse
    {

        /*
         * can the object move to the destination?
         * If no, binary search tree between src and dest to approximate closest allowed point
         * After finding nearest point, is the collision object 'sticky' or 'slippery'?
         * if slippery, translate the objects remaining trajectory orthogonal to the contact point
         * can the orthogonal move reach the desination?
         * if no, binary search to place as close as possible (just like before)
         */
        public static Vector2 Move(this Circle circle, QuadTree map, Vector2 targetDestination)
        {

            //move as close as we can to the targetDesitnation without causing a collision.
            var (linearProjection, firstCollisionObjects) = circle.NearestLinearProjection(map, targetDestination);
            if (firstCollisionObjects == null) return linearProjection; //There was no collision preventing us from reaching targetDestination

            //we have one or more objects that our circle will collide with on the way to targetDestination.
            //Lets find the nearest one to us, and find the edge of it that is nearest to us. This gives us the specific point of collision.
            var nearestEdge = circle.NearestEdge(firstCollisionObjects);

            var bounceVector = RedirectAlongCollisionEdge(circle, nearestEdge, targetDestination, linearProjection);
            var newTarget = linearProjection + bounceVector;

            var midpointCircle = new Circle() { Radius = circle.Radius, Center = linearProjection };
            var (finalDestination, _) = midpointCircle.NearestLinearProjection(map, newTarget);

            return finalDestination;
        }

        private static Vector2 RedirectAlongCollisionEdge(Circle circle, LineSegment nearestEdge, Vector2 targetDestination, Vector2 linearProjection)
        {
            var normalizedEdge = Vector2.Normalize(nearestEdge.Points[0] - nearestEdge.Points[1]);
            var successfulDistanceSquared = Vector2.DistanceSquared(circle.Center, linearProjection);
            var targetDistanceSquared = Vector2.DistanceSquared(circle.Center, targetDestination);
            var remainingDistance = Math.Sqrt(targetDistanceSquared) - Math.Sqrt(successfulDistanceSquared);
            var remainingDirection = Vector2.Normalize(circle.Center - targetDestination);

            var remainingDistanceOrtho = Vector2.Dot(Vector2.Multiply((float)remainingDistance, remainingDirection), normalizedEdge);
            var newTarget = Vector2.Multiply(remainingDistanceOrtho, normalizedEdge);
            return newTarget;
        }

        private static (Vector2, IEnumerable<IBoundingArea>) NearestLinearProjection(this Circle circle, QuadTree map, Vector2 targetDestination, int precision = 6)
        {
            var proposedDestination = targetDestination;
            var farthestFailure = targetDestination;
            var nearestSuccess = circle.Center;

            IEnumerable<IBoundingArea> firstCollisionObjects = null;
            for (var i = 0; i < precision; i++)
            {
                var testCircle = new Circle() { Radius = circle.Radius, Center = proposedDestination };
                var potentialCollisions = map.FindObjects(testCircle);//broad phase collision detection
                firstCollisionObjects = potentialCollisions.Where(c => c.Intersects(testCircle));//narrow phase collision detection
                if (!firstCollisionObjects.Any() && i == 0)
                {
                    return (proposedDestination, null);//No contest; The initial desired position can be reached without collision.
                }
                else if (!firstCollisionObjects.Any())
                {
                    nearestSuccess = proposedDestination;
                }
                else
                {
                    farthestFailure = proposedDestination;
                }
                proposedDestination = (nearestSuccess + farthestFailure) / 2.0f;
            }

            return (nearestSuccess, firstCollisionObjects);
        }

        private static LineSegment NearestEdge(this Circle circle, IEnumerable<IBoundingArea> firstCollisionObjects)
        {
            IBoundingArea nearestShape = null;
            float distanceSquaredToNearestShape = float.MaxValue;
            foreach (var obj in firstCollisionObjects)
            {
                var distanceSquaredFromCircle = Vector2.DistanceSquared(obj.NearestPoint(circle.Center), circle.Center);
                if (distanceSquaredFromCircle < distanceSquaredToNearestShape)
                {

                    //TODO: If you hit a corner shared by two objects, the one chosen here is arbitrary
                    // but the choice matters.
                    nearestShape = obj;
                    distanceSquaredToNearestShape = distanceSquaredFromCircle;
                }
            }

            var edges = nearestShape.BoundingType switch
            {
                BoundingType.Triangle => ((Triangle)nearestShape).GetEdges(),
                BoundingType.Rectangle => ((Rectangle)nearestShape).GetEdges(),
                BoundingType.LineSegment => new LineSegment[1] { ((LineSegment)nearestShape) },
                _ => throw new NotImplementedException() //TODO: Circles TBD
            };
            LineSegment nearestEdge = null;
            float distanceSquaredToNearestEdge = float.MaxValue;
            foreach (var e in edges)
            {
                var distanceSquaredFromCircle = Vector2.DistanceSquared(e.NearestPoint(circle.Center), circle.Center);
                if (distanceSquaredFromCircle < distanceSquaredToNearestEdge)
                {
                    //TODO: If you hit a corner, the one chosen here is arbitrary
                    // but the choice matters.
                    nearestEdge = e;
                    distanceSquaredToNearestEdge = distanceSquaredFromCircle;
                }
            }

            return nearestEdge;
        }
    }
}
