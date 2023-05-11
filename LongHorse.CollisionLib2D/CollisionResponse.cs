using LongHorse.CollisionLib2D.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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
            var projectedCircle = new Circle() { Radius = circle.Radius, Center = linearProjection };
            var nearestPoint = projectedCircle.NearestPoint(firstCollisionObjects);

            var bounceVector = RedirectAlongCollisionEdge(circle, nearestPoint, targetDestination, linearProjection);
            var newTarget = linearProjection + bounceVector;

            var (finalDestination, _) = projectedCircle.NearestLinearProjection(map, newTarget);

            return finalDestination;
        }

        private static Vector2 RedirectAlongCollisionEdge(Circle circle, Vector2 nearestPointToCircle, Vector2 targetDestination, Vector2 linearProjection)
        {
            var collisionDirection = nearestPointToCircle - linearProjection;
            var normalizedCollisionDirection = Vector2.Normalize(collisionDirection);
            var collisionOrthoVector = new Vector2(collisionDirection.Y * -1, collisionDirection.X);
            var normalizedCollisionOrthoVector = Vector2.Normalize(collisionOrthoVector);
            var successfulDistance = Vector2.Distance(circle.Center, linearProjection);
            var targetDistance = Vector2.Distance(circle.Center, targetDestination);
            var remainingDistance = targetDistance - successfulDistance;
            var remainingDirection = Vector2.Normalize(targetDestination - linearProjection);

            var remainingDistanceOrtho = Vector2.Dot(Vector2.Multiply((float)remainingDistance, remainingDirection), normalizedCollisionOrthoVector);
            var newTarget = Vector2.Multiply(remainingDistanceOrtho, normalizedCollisionOrthoVector);
            return newTarget;
        }

        private static (Vector2, IEnumerable<IBoundingArea>) NearestLinearProjection(this Circle circle, QuadTree map, Vector2 targetDestination, int precision = 16)
        {
            var proposedDestination = targetDestination;
            var farthestFailure = targetDestination;
            var nearestSuccess = circle.Center;

            IEnumerable<IBoundingArea> firstCollisionObjects = null;
            for (var i = 0; i < precision; i++)
            {
                var testCircle = new Circle() { Radius = circle.Radius, Center = proposedDestination };
                var potentialCollisions = map.FindObjects(testCircle);//broad phase collision detection
                var currentCollisionObjects = potentialCollisions.Where(c => c.Intersects(testCircle));//narrow phase collision detection
                if (!currentCollisionObjects.Any() && i == 0)
                {
                    return (proposedDestination, null);//No contest; The initial desired position can be reached without collision.
                }
                else if (!currentCollisionObjects.Any())
                {
                    nearestSuccess = proposedDestination;
                }
                else
                {
                    firstCollisionObjects = currentCollisionObjects;
                    farthestFailure = proposedDestination;
                }
                proposedDestination = (nearestSuccess + farthestFailure) / 2.0f;
            }

            return (nearestSuccess, firstCollisionObjects);
        }

        private static Vector2 NearestPoint(this Circle circle, IEnumerable<IBoundingArea> firstCollisionObjects)
        {
            float distanceSquaredToNearestShape = float.MaxValue;
            Vector2 nearestPoint = circle.Center;
            foreach (var obj in firstCollisionObjects)
            {
                var nearestPointToObj = obj.NearestPoint(circle.Center);
                var distanceSquaredFromCircle = Vector2.DistanceSquared(nearestPointToObj, circle.Center);
                if (distanceSquaredFromCircle < distanceSquaredToNearestShape)
                {
                    nearestPoint = nearestPointToObj;
                    distanceSquaredToNearestShape = distanceSquaredFromCircle;
                }
            }

            return nearestPoint;
        }
    }
}
