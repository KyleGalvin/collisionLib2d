﻿using System;
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
        public static bool Intersects(this Circle c, Rectangle r)
        {
            var p = c.Center.NearestPoint(r);
            var delta = p - c.Center;
            return (c.Radius * c.Radius) > Vector2.Dot(delta, delta);
        }

        /// <summary>Determines if the circle overlaps with the rectangle</summary>
        /// <param name="c">The circle</param>
        /// <param name="r">The rectangle</param>
        /// <returns>true if the circle and rectangle overlap</returns>
        public static bool Intersects(this Rectangle r, Circle c)
        {
            return c.Intersects(r);
        }

        public static bool Intersects(this Triangle t, Circle c)
        {
            var p = c.Center.NearestPoint(t);
            var delta = p - c.Center;
            return (c.Radius * c.Radius) > Vector2.Dot(delta, delta);
        }

        public static bool Intersects(this Circle c, Triangle t)
        {
            return t.Intersects(c);
        }

        public static bool Intersects(this Triangle t, Rectangle r) 
        {
            //translate triangle as conceptually moving rectangle to origin
            var v0 = t.Points[0] - r.Center;
            var v1 = t.Points[1] - r.Center;
            var v2 = t.Points[2] - r.Center;

            //compute edge vectors for triangle
            var f0 = v1 - v0;
            var f1 = v2 - v1;
            var f2 = v0 - v2;
            throw new NotImplementedException();
        }
    }
}
