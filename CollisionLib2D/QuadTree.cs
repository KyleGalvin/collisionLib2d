using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LongHorse.CollisionLib2D
{
    /// <summary>
    /// A tree data structure in which each node has exactly four children. 
    /// Used to partition a two-dimensional space by recursively subdividing it into four quadrants.
    /// Allows to efficiently find objects spatially relative to each other.
    /// </summary>
    public class QuadTree
    {
        /// <summary>The area of this quadrant.</summary>
        public Rectangle Area;
        /// <summary>Objects in this quadrant.</summary>
        private readonly HashSet<IBoundingArea> _objects;
        /// <summary>If this quadrant has sub quadrants. Objects only exist on quadrants without children.</summary>
        private bool _hasChildren;

        /// <summary>Top left quadrant.</summary>
        private QuadTree _quad_TL;
        /// <summary>Top right quadrant.</summary>
        private QuadTree _quad_TR;
        /// <summary>Bottom left quadrant.</summary>
        private QuadTree _quad_BL;
        /// <summary>Bottom right quadrant.</summary>
        private QuadTree _quad_BR;

        /// <summary>Gets the current depth level of this quadrant.</summary>
        public int CurrentLevel { get; }
        /// <summary>Gets the max depth level.</summary>
        public int MaxLevel { get; }
        /// <summary>Gets the max number of objects in this quadrant.</summary>
        public int MaxObjects { get; }

        /// <summary>Initializes a new instance of the QuadTree class.</summary>
        /// <param name="left">The x-coordinate of the upper-left corner of the boundary rectangle.</param>
        /// <param name="top">The y-coordinate of the upper-left corner of the boundary rectangle.</param>
        /// <param name="width">The width of the boundary rectangle.</param>
        /// <param name="height">The height of the boundary rectangle.</param>
        /// <param name="maxObjects">The max number of elements in one rectangle.</param>
        /// <param name="maxLevel">The max depth level.</param>
        /// <param name="currentLevel">The current depth level. Leave default if this is the root QuadTree.</param>
        public QuadTree(float left, float top, float width, float height, int maxObjects = 10, int maxLevel = 5, int currentLevel = 0)
        {
            Area = new Rectangle (left, top, width, height);
            _objects = new HashSet<IBoundingArea>();

            CurrentLevel = currentLevel;
            MaxLevel = maxLevel;
            MaxObjects = maxObjects;

            _hasChildren = false;
        }

        /// <summary>Initializes a new instance of the QuadTree class.</summary>
        /// <param name="width">The width of the boundary rectangle.</param>
        /// <param name="height">The height of the boundary rectangle.</param>
        /// <param name="maxObjects">The max number of elements in one rectangle.</param>
        /// <param name="maxLevel">The max depth level.</param>
        /// <param name="currentLevel">The current depth level. Leave default if this is the root QuadTree.</param>
        public QuadTree(float width, float height, int maxObjects = 10, int maxLevel = 5, int currentLevel = 0)
            : this(0, 0, width, height, maxObjects, maxLevel, currentLevel) { }

        /// <summary>Initializes a new instance of the QuadTree class.</summary>
        /// <param name="size">The size of the boundary rectangle.</param>
        /// <param name="maxObjects">The max number of elements in one rectangle.</param>
        /// <param name="maxLevel">The max depth level.</param>
        /// <param name="currentLevel">The current depth level. Leave default if this is the root QuadTree.</param>
        public QuadTree(Vector2 size, int maxObjects = 10, int maxLevel = 5, int currentLevel = 0)
            : this(0, 0, size.X, size.Y, maxObjects, maxLevel, currentLevel) { }

        /// <summary>Initializes a new instance of the QuadTree class.</summary>
        /// <param name="position">The position of the boundary rectangle.</param>
        /// <param name="size">The size of the boundary rectangle.</param>
        /// <param name="maxObjects">The max number of elements in one rectangle.</param>
        /// <param name="maxLevel">The max depth level.</param>
        /// <param name="currentLevel">The current depth level. Leave default if this is the root QuadTree.</param>
        public QuadTree(Vector2 position, Vector2 size, int maxObjects = 10, int maxLevel = 5, int currentLevel = 0)
            : this(position.X, position.Y, size.X, size.Y, maxObjects, maxLevel, currentLevel) { }

        /// <summary>Splits the current quadrant into four new quadrants and drops all objects to the lower quadrants.</summary>
        private void Quarter()
        {
            if (CurrentLevel >= MaxLevel) return;

            int nextLevel = CurrentLevel + 1;
            _hasChildren = true;
            _quad_TL = new QuadTree(Area.Left, Area.Top, Area.HalfWidth, Area.HalfHeight, MaxObjects, MaxLevel, nextLevel);
            _quad_TR = new QuadTree(Area.CenterX, Area.Top, Area.HalfWidth, Area.HalfHeight, MaxObjects, MaxLevel, nextLevel);
            _quad_BL = new QuadTree(Area.Left, Area.CenterY, Area.HalfWidth, Area.HalfHeight, MaxObjects, MaxLevel, nextLevel);
            _quad_BR = new QuadTree(Area.CenterX, Area.CenterY, Area.HalfWidth, Area.HalfHeight, MaxObjects, MaxLevel, nextLevel);

            foreach (var obj in _objects)
            {
                Insert(obj);
            }
            _objects.Clear();
        }

        /// <summary> Removes all elements from the QuadTree.</summary>
        public void Clear()
        {
            if(_hasChildren)
            {
                _quad_TL.Clear();
                _quad_TL = null;
                _quad_TR.Clear();
                _quad_TR = null;
                _quad_BL.Clear();
                _quad_BL = null;
                _quad_BR.Clear();
                _quad_BR = null;
            }

            _objects.Clear();
            _hasChildren = false;
        }

        /// <summary> Inserts an object into the QuadTree.</summary>
        /// <param name="obj">The object to insert.</param>
        /// <returns>true if the object is successfully added to the QuadTree; false if object is not added to the QuadTree.</returns>
        public bool Insert(IBoundingArea obj)
        {
            if(obj == null) throw new ArgumentNullException(nameof(obj));

            if (!Area.Intersects(obj)) return false;

            if (_hasChildren)
            {
                //place the object in all children the object overlaps
                //this allows for objects of dramatically differing sizes to be in the same neighborhood
                var insertSuccessful = false;
                insertSuccessful |= _quad_TL.Insert(obj);
                insertSuccessful |= _quad_TR.Insert(obj);
                insertSuccessful |= _quad_BL.Insert(obj);
                insertSuccessful |= _quad_BR.Insert(obj);
                return insertSuccessful;
            }
            else
            {
                _objects.Add(obj);
                if(_objects.Count > MaxObjects)
                {
                    Quarter();
                }
                return true;
            }
        }

        /// <summary> Inserts a collection of objects into the QuadTree.</summary>
        /// <param name="objects">The collection of objects to insert.</param>
        public void InsertRange(IEnumerable<IBoundingArea> objects)
        {
            foreach(var obj in objects)
            {
                Insert(obj);
            }
        }

        /// <summary>Returns the total number of obejcts in the QuadTree and its children.</summary>
        /// <returns>the total number of objects in this instance.</returns>
        public int Count()
        {
            int count = 0;
            if (_hasChildren)
            {
                count += _quad_TL.Count();
                count += _quad_TR.Count();
                count += _quad_BL.Count();
                count += _quad_BR.Count();
            }
            else
            {
                count = _objects.Count;
            }

            return count;
        }

        /// <summary> Returns every container Rectangle from the QuadTree.</summary>
        /// <returns> an array of Rectangles from the QuadTree.</returns>
        public Rectangle[] GetGrid()
        {
            List<Rectangle> grid = new List<Rectangle> {Area};
            if (_hasChildren)
            {
                grid.AddRange(_quad_TL.GetGrid());
                grid.AddRange(_quad_TR.GetGrid());
                grid.AddRange(_quad_BL.GetGrid());
                grid.AddRange(_quad_BR.GetGrid());
            }
            return grid.ToArray();
        }

        /// <summary>Searches for objects in any quadrants which the passed region overlaps, but not specifically within that region.</summary>
        /// <param name="searchArea">The search region.</param>
        /// <returns>an array of objects.</returns>
        public IBoundingArea[] FindObjects(IBoundingArea searchArea)
        {
            List<IBoundingArea> foundObjects = new List<IBoundingArea>();
            if(_hasChildren)
            {
                foundObjects.AddRange(_quad_TL.FindObjects(searchArea));
                foundObjects.AddRange(_quad_TR.FindObjects(searchArea));
                foundObjects.AddRange(_quad_BL.FindObjects(searchArea));
                foundObjects.AddRange(_quad_BR.FindObjects(searchArea));
            }
            else
            {
                if(Area.Intersects(searchArea))
                {
                    foundObjects.AddRange(_objects);
                }
            }

            HashSet<IBoundingArea> result = new HashSet<IBoundingArea>();
            result.UnionWith(foundObjects);

            return result.ToArray();
        }
    }
}
