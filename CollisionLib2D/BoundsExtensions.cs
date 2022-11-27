using System;
using System.Numerics;

namespace LongHorse.CollisionLib2D
{
    public static class BoundsExtensions
    {
        /// <summary>Determines if one bounding area overlaps with another.</summary>
        /// <param name="b1">The first bounding area</param>
        /// <param name="b2">The second bounding area</param>
        /// <returns>true if the two bounding areas overlap</returns>
        public static bool Intersects(this IBoundingArea b1, IBoundingArea b2)
        {
            if(b1.BoundingType == BoundingType.Rectangle)
            {
                if(b2.BoundingType == BoundingType.Rectangle)
                {
                    return ((Rectangle)(b1)).Intersects((Rectangle)b2);
                }
                else
                {
                    return ((Rectangle)(b1)).Intersects((Circle)b2);
                }
            } 
            else
            {
                if (b2.BoundingType == BoundingType.Rectangle)
                {
                    return ((Circle)(b1)).Intersects((Rectangle)b2);
                }
                else
                {
                    return ((Circle)(b1)).Intersects((Circle)b2);
                }
            }
        }

        /// <summary>Determines if one rectangle overlaps with another.</summary>
        /// <param name="r1">The first rectangle</param>
        /// <param name="r2">The second rectangle</param>
        /// <returns>true if the two rectangles overlap</returns>
        public static bool Intersects(this Rectangle r1, Rectangle r2)
        {
            if (r1.Right <= r2.Left || r1.Left >= r2.Right) return false;
            if (r1.Top >= r2.Bottom || r1.Bottom <= r2.Top) return false;
            return true;
        }

        /// <summary>Determines if one circle overlaps with another.</summary>
        /// <param name="c1">The first circle</param>
        /// <param name="c2">The second circle</param>
        /// <returns>true if the two circles overlap</returns>
        public static bool Intersects(this Circle c1, Circle c2)
        {
            var delta = c1.Center - c2.Center;
            return ((c1.Radius + c2.Radius) * (c1.Radius + c2.Radius)) > ((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y));
        }

        public static Vector2 NearestPoint(this Vector2 p, Rectangle r) 
        {
            return new Vector2(
                    MathF.Min(MathF.Max(p.X, r.Left), r.Right),
                    MathF.Min(MathF.Max(p.Y, r.Top), r.Bottom)
                );
        }

        public static bool LineSegmentsIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2) 
        {
            Vector2 deltaA = a2 - a1;
            Vector2 deltaB = b2 - b1;
            var aDotBPerp = deltaA.X * deltaB.Y - deltaA.Y * deltaB.X;

            //lines are parallel 
            if (aDotBPerp == 0) return false;

            Vector2 c = b1 - a1;
            var t = (c.X * deltaB.Y - c.Y * deltaB.X) / aDotBPerp;
            if (t < 0 || t > 1) return false;

            var u = (c.X * deltaA.Y - c.Y * deltaA.X) / aDotBPerp;
            if (u < 0 || u > 1) return false;

            return true;
        }

        public static Vector2 Intersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2) 
        {
            Vector2 deltaA = a2 - a1;
            Vector2 deltaB = b2 - b1;
            var aDotBPerp = deltaA.X * deltaB.Y - deltaA.Y * deltaB.X;
            Vector2 c = b1 - a1;
            var t = (c.X * deltaB.Y - c.Y * deltaB.X) / aDotBPerp;
            return a1 + (deltaA * t);
        }
        public static Vector2 Intersection(this LineSegment a, LineSegment b)
        {
            return Intersection(a.Points[0], a.Points[1], b.Points[0], b.Points[1]);
        }
        public static bool Intersects(this LineSegment a, LineSegment b) 
        {
            return LineSegmentsIntersect(a.Points[0], a.Points[1], b.Points[0], b.Points[1]);
        }

        public static Vector2 NearestPoint(Vector2 p, Vector2 a1, Vector2 a2) 
        {
            var ab = a2 - a1;
            var t = Vector2.Dot(p - a1, ab) / Vector2.Dot(ab, ab);
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;
            return a1 + t * ab;
        }

        public static Vector2 NearestPoint(this Vector2 p, LineSegment a) 
        {
            return NearestPoint(p, a.Points[0], a.Points[1]);
        }

        public static Vector2 NearestPoint(this Vector2 p, Triangle r)
        {
            // check if P in vertex region outside A
            Vector2 ab = r.Points[1] - r.Points[0];
            Vector2 ac = r.Points[2] - r.Points[0];
            Vector2 ap = p - r.Points[0];
            float d1 = Vector2.Dot(ab, ap);
            float d2 = Vector2.Dot(ac, ap);
            if (d1 <= 0.0f && d2 <= 0.0f) return r.Points[0]; // barycentric coordinates (1,0,0)

            // check if P in vertex region outside B
            Vector2 bp = p - r.Points[1];
            float d3 = Vector2.Dot(ab, bp);
            float d4 = Vector2.Dot(ac, bp);
            if (d3 >= 0.0f && d4 <= d3) return r.Points[1]; // barycentric coordinates (0,1,0)

            // check if P in edge region of AB, if so return projection of P onto AB
            float vc = d1 * d4 - d3 * d2;
            float v;
            if (vc <= 0.0f && d1 >= 0.0f && d3 <= 0.0f) 
            {
                v = d1 / (d1 - d3);
                return r.Points[0] + v * ab;
            }

            // check if P in vertex region outside C
            Vector2 cp = p - r.Points[2];
            float d5 = Vector2.Dot(ab, cp);
            float d6 = Vector2.Dot(ac, cp);
            if(d6 >= 0.0f && d5 <= d6) return r.Points[2]; // barycentric coordinates (0,0,1)

            // check if P in edge region of AC, if so return projection of P onto AC
            float vb = d5 * d2 - d1 * d6;
            float w;
            if (vb <= 0.0f && d2 >= 0.0f && d6 <= 0.0f) 
            {
                w = d2 / (d2 - d6);
                return r.Points[0] + w * ac;
            }

            // check if P in edge region of BC, if so return projection of P onto BC
            float va = d3 * d6 - d5 * d4;
            if (va <= 0.0f && (d4 - d3) > 0.0f && (d5 - d6) >= 0.0f) 
            {
                w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return r.Points[1] + w * (r.Points[2] - r.Points[1]);
            }

            // P inside face region. Compute Q through its barycentric coordinates (u,v,w)
            float denom = 1.0f / (va + vb + vc);
            v = vb * denom;
            w = vc * denom;
            return r.Points[0] + ab * v + ac * w;
        }

        /// <summary>Determines if the circle overlaps with the rectangle</summary>
        /// <param name="c">The circle</param>
        /// <param name="r">The rectangle</param>
        /// <returns>true if the circle and rectangle overlap</returns>
        public static bool Intersects(this Rectangle r, Circle c)
        {
            return c.Intersects(r);
        }
        public static bool Intersects(this Circle c, Triangle t)
        {
            return t.Intersects(c);
        }
        public static bool Intersects(this Circle c, LineSegment ls)
        {
            return ls.Intersects(c);
        }

        /// <summary>Determines if the circle overlaps with the rectangle</summary>
        /// <param name="c">The circle</param>
        /// <param name="r">The rectangle</param>
        /// <returns>true if the circle and rectangle overlap</returns>
        public static bool Intersects(this Circle c, Rectangle r)
        {
            var p = c.Center.NearestPoint(r);
            var delta = p - c.Center;
            return (c.Radius * c.Radius) > Vector2.Dot(delta, delta);
        }
        public static bool Intersects(this Triangle t, Circle c)
        {
            var p = c.Center.NearestPoint(t);
            var delta = p - c.Center;
            return (c.Radius * c.Radius) > Vector2.Dot(delta, delta);
        }
        public static bool Intersects(this LineSegment ls, Circle c) 
        {
            var p = c.Center.NearestPoint(ls);
            var delta = p - c.Center;
            return (c.Radius * c.Radius) > Vector2.Dot(delta, delta);
        }

        public static LineSegment[] GetEdges(this Rectangle r) 
        {
            return new LineSegment[4]
                {
                new LineSegment(new Vector2(r.Left, r.Bottom), new Vector2(r.Left, r.Top)),
                new LineSegment(new Vector2(r.Left, r.Top), new Vector2(r.Right, r.Top)),
                new LineSegment(new Vector2(r.Right, r.Top), new Vector2(r.Right, r.Bottom)),
                new LineSegment(new Vector2(r.Right, r.Bottom), new Vector2(r.Left, r.Bottom))
                };
        }

        public static LineSegment[] GetEdges(this Triangle t)
        {
            return new LineSegment[3]
            {
                new LineSegment(t.Points[0], t.Points[1]),
                new LineSegment(t.Points[1], t.Points[2]),
                new LineSegment(t.Points[2], t.Points[0])
            };

        }
        public static bool Intersects(this Triangle t, Rectangle r) 
        {

            // If one shape is contained inside the other, they intersect.
            var tCenter = t.Center;
            var p = r.Center.NearestPoint(t);
            if ((p - r.Center).LengthSquared() < 0.0001f) return true;
            p = tCenter.NearestPoint(r);
            if ((p - tCenter).LengthSquared() < 0.0001f) return true;

            var triangleEdges = t.GetEdges();
            var rectangleEdges = r.GetEdges();


            // If any 2 edges intersect, they intersect
            foreach (var e1 in triangleEdges) 
            {
                foreach (var e2 in rectangleEdges) 
                {
                    if (e1.Intersects(e2)) return true;
                }
            }

            return false;
        }

        public static bool Intersects(this Triangle t1, Triangle t2)
        {

            // If one shape is contained inside the other, they intersect.
            var t1Center = t1.Center;
            var t2Center = t2.Center;
            var p = t2Center.NearestPoint(t1);
            if ((p - t2Center).LengthSquared() < 0.0001f) return true;
            p = t1Center.NearestPoint(t2);
            if ((p - t1Center).LengthSquared() < 0.0001f) return true;

            var t1Edges = t1.GetEdges();
            var t2Edges = t2.GetEdges();


            // If any 2 edges intersect, they intersect
            foreach (var e1 in t1Edges)
            {
                foreach (var e2 in t2Edges)
                {
                    if (e1.Intersects(e2)) return true;
                }
            }

            return false;
        }

        public static bool Intersects(this LineSegment l, Triangle t)
        {
            foreach (var e in t.GetEdges())
            {
                if (l.Intersects(e)) return true;
            }

            return false;
        }

        public static bool Intersects(this LineSegment l, Rectangle r)
        {
            foreach (var e in r.GetEdges())
            {
                if (l.Intersects(e)) return true;
            }

            return false;
        }
    }
}
