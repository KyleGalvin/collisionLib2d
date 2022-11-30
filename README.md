# Table Of Contents
- [Introduction](#introduction)
- [Installation](#installation)
- [Contents](#contents)
	- [Primitives](#primitives)
		- [IBoundingArea](#bounding)
		- [Rectangle](#rectangle)
		- [Circle](#circle)
		- [Triangle](#triangle)
		- [Line Segment](#line)
	- [NearestPoint](#nearest)
	- [Intersections](#intersections)
	- [Spatial Partitioning](#partitioning)
- [References](#references)
	- [Theory](#theory)
	- [Implementations](#implementations)

# <a id="introduction"></a>Introduction 

This is a collision detection toolkit written in C#.

It provides geometry primitives for Circles, Rectangles, Triangles, and Line Segments, as well as a QuadTree structure for spatial partitioning.

Each of these primitives implements a standardized interface allowing them to be placed in the QuadTree for broad-phase quick culling.

These primitives also have a complete set of pairwise collision detection methods. That is to say this library can determine if any two given primitives are intersecting, regardless of which of the primitives are provided.

# <a id="installation"></a>Installation 

Setup this registry from the command line:
```
dotnet nuget add source --name LongHorse https://gitea.longhorse.studio/api/packages/LongHorse/nuget/index.json
```
To install the package using NuGet, run the following command:
```
dotnet add package --source LongHorse --version <VERSION> LongHorse.CollisionLib2D
```
# <a id="contents"></a>Contents

The root of the repository contains the project solution, this readme, and git/gitlab configuration files.

The CollisionLib2D folder contains all source code, and the XunitTests folder contains some unit tests to validate basic functionality.

## <a id="primitives"></a>Primitives

### <a id="bounding"></a>IBoundingArea

IBoundingArea is the common interface for all our shape primitives. This allows us to manage lists of mixed types while being able to query for basic information like global X and Y boundaries, and the ability to translate the objects global coordinates. There is also a BoundingType enum which allows us to down-cast to the underlying primitive to access more concrete geometrical properties.

X and Y boundaries (Top, Bottom, Left, Right) are read-only on the interface. Methods to resize shapes are left to the concrete implementation.

### <a id="rectangle"></a>Rectangle

The rectangle class is the implementation of IBoundingArea that is most true to the interface. 

The intersecting spaces between IBoundingArea and Rectangle are identical. 

There are methods to allow you to move the Left, Top, Right, and Bottom of the shape to a position without changing the dimensions of the shape.

### <a id="circle"></a>Circle

The circle is the simplest implementation of IBoundingArea, with a Center and a Radius.

### <a id="triangle"></a>Triangle

Internally, the triangle is represented by three connected points.

The Top, Left, Right, and Bottom are the minimum and maximum X and Y values of the three points, creating a bounding square around the shape.

The center is the triangle Centroid, calculated on every access. Because of this, reducing unneeded calls to the Center property will help performance.

### <a id="line"></a>LineSegment

Similar to the triangle, the LineSegment is represented by an array of points.

The center is the midpoint of the line, and is also calculated on each access.

## <a id="nearest"></a>Nearest Point

For each of the IBoundingArea implementations, the NearestPoint method will determine the point within the given shape that is closest to the input vector. Each shape uses a different algorithm, most of them taken from [[1]](#rtcd)

## <a id="intersections"></a>Intersections

## <a id="partitioning"></a>Spatial Partitioning

Basic QuadTree usage involves creating a new QuadTree and filling its space with IBoundingArea implementations.

Once the tree contains the objects we wish to query, we can call FindObjects() to search a local area for IBoundingArea neighbors in the vicinity of the search object.

```csharp
//create a 500x500 quadTree
var treeSizeX = 500;
var treeSizeY = 500;
var quadTree = new QuadTree(treeSizeX, treeSizeY);

//add 50 random 1x1 rectangles to the structure.
Random rnd = new Random();
for (var i = 0; i < 50; i++)
{
	//Size.X and Size.Y are width and height of the rectangle.
	//randomly position the center of the rectangle.
	var rect = new Rectangle() { Size = new Vector2(1.0f, 1.0f), Center = new Vector2(rnd.Next(0,500), rnd.Next(0,500)) };
	quadTree.Insert(rect);
}

//search a random 10x10 area to determine which rectangles are 'in the neighborhood' of the search area.
//Note they may not necessarily overlap the rectangle. 
//They are simply close enough that the QuadTree flags them as overlapping candidates.
var searchRect = new Rectangle() { Size = new Vector2(10.0f, 10.0f), Center = new Vector2(rnd.Next(0,500), rnd.Next(0,500))};
var neighbors = quadTree.FindObjects(searchRect);
```

A QuadTree does not tell us with certainty if items overlap, only which are nearest the search area.

To find which items in the neighbors set overlap, we can iterate over them in a second pass and see which intersect.

```csharp
//make a list to store items that are actually colliding
List<IBoundingArea> collisions = new List<IBoundingArea>();

//populate the list from the neighbor candidates
foreach (var candidate in neighbors)
{
	//various definitions for Intersects() are defined for circle/circle, rect/rect, rect/circle, and circle/rect.
	if(searchRect.Intersects(candidate))
	{
		collisions.Add(candidate);
	}
}
```

# <a id="references"></a> References 

## <a id="theory"></a> Theory
- <a id="rtcd"></a>[1][Ericson, Christer (2005) Real-Time Collision Detection](http://www.r-5.org/files/books/computers/algo-list/realtime-3d/Christer_Ericson-Real-Time_Collision_Detection-EN.pdf)
- <a id="wikiquadtree"></a>[2][Wikipedia QuadTree](https://en.wikipedia.org/wiki/Quadtree)

## <a id="implementations"></a> Implementations
- [3][Connor 'Auios' Andrew Ngo (2020) Auios.QuadTree](https://github.com/Auios/Auios.QuadTree)
- [4][Igor 'Leonidovia' (2017) UltimateQuadTree](https://github.com/leonidovia/UltimateQuadTree)

Portions of the QuadTree implementation were taken and modified directly from the Auios project.