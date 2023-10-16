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

            (linearProjection, firstCollisionObjects) = projectedCircle.NearestLinearProjection(map, newTarget);
            if (firstCollisionObjects == null) return linearProjection; //There was no collision preventing us from reaching targetDestination

            projectedCircle = new Circle() { Radius = circle.Radius, Center = linearProjection };
            nearestPoint = projectedCircle.NearestPoint(firstCollisionObjects);

            bounceVector = RedirectAlongCollisionEdge(circle, nearestPoint, targetDestination, linearProjection);
            newTarget = linearProjection + bounceVector;

            var (finalDestination, _) = projectedCircle.NearestLinearProjection(map, newTarget);

            return finalDestination;
        }

        private static Vector2 RedirectAlongCollisionEdge(Circle circle, Vector2 nearestPointToCircle, Vector2 targetDestination, Vector2 linearProjection)
        {
            //find the direction of the obstruction
            var collisionDirection = nearestPointToCircle - linearProjection;
            var normalizedCollisionDirection = Vector2.Normalize(collisionDirection);

            //rotate 90 degrees away from the obstruction towards our new destination
            var collisionOrthoVector = new Vector2(collisionDirection.Y * -1, collisionDirection.X);//Todo: determine CW vs CCW?


            //if we are touching a wall, and move perfectly parallel to it, floating point rounding error means we might trigger collision again.
            //to compensate, lets move the deflection target the smallest distance we can in the opposite direction of our collision direction
            collisionOrthoVector += Vector2.Multiply( normalizedCollisionDirection, -0.0001f);
            var deflectionDirection = Vector2.Normalize(collisionOrthoVector);


            var successfulDistance = Vector2.Distance(circle.Center, linearProjection);
            var targetDistance = Vector2.Distance(circle.Center, targetDestination);
            var remainingDistance = targetDistance - successfulDistance;
            var remainingDirection = Vector2.Normalize(targetDestination - linearProjection);

            var remainingDistanceOrtho = Vector2.Dot(Vector2.Multiply((float)remainingDistance, remainingDirection), deflectionDirection);
            var newTarget = Vector2.Multiply(remainingDistanceOrtho, deflectionDirection);
            return newTarget;
        }

        private static (Vector2, IEnumerable<IBoundingArea>) NearestLinearProjection(this Circle circle, QuadTree map, Vector2 targetDestination, int precision = 16)
        {
            var proposedDestination = targetDestination;
            var nearestFailure = targetDestination;
            var farthestSuccess = circle.Center;

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
                    farthestSuccess = proposedDestination;
                }
                else
                {
                    if (firstCollisionObjects == null)
                    {
                        //first time here, so we set the trouble objects
                        firstCollisionObjects = currentCollisionObjects;
                    }
                    else
                    {
                        //if an object was only a problem in certain steps,
                        //it should be excluded on the basis that we have found a destination in our direction where it is not colliding
                        var updatedObjects = firstCollisionObjects.Intersect(currentCollisionObjects);
                        if (updatedObjects.Any())
                        {
                            firstCollisionObjects = updatedObjects;
                        }

                    }

                    nearestFailure = proposedDestination;
                }

                //if we start getting too precise, floating point error breaks our comparator operators
                if (Vector2.DistanceSquared(farthestSuccess, nearestFailure) < 0.0000000001) break;
                proposedDestination = (farthestSuccess + nearestFailure) / 2.0f;
            }

            return (farthestSuccess, firstCollisionObjects);
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
