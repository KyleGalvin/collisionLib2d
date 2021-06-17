## Introduction

This is a C# QuadTree implementation derived from others I've seen and modified to suit my own style and needs.

Also included are primitive geometry methods to determine if various shapes (Circles, Rectangles) are intersecting.

# Reference links

## Theory
- [Ericson, Christer (2005) Real-Time Collision Detection](http://www.r-5.org/files/books/computers/algo-list/realtime-3d/Christer_Ericson-Real-Time_Collision_Detection-EN.pdf)
- [Wikipedia QuadTree](https://en.wikipedia.org/wiki/Quadtree)

## Implementations
- [Connor 'Auios' Andrew Ngo (2020) Auios.QuadTree](https://github.com/Auios/Auios.QuadTree)
- [Igor 'Leonidovia' (2017) UltimateQuadTree](https://github.com/leonidovia/UltimateQuadTree)

Portions of the QuadTree implementation were taken and modified directly from the Auios project.

## What's contained in this project

The root of the repository contains the project solution, this readme, and git/gitlab configuration files.

The CollisionLib2D folder contains all source code, and the XunitTests folder contains some unit tests to validate basic functionality.


# Example

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
	var rect = new Rectangle() { Width = 1.0f, Height = 1.0f, CenterX = rnd.Next(0,500), CenterY = rnd.Next(0,500) };
	quadTree.Insert(rect);
}

//search a random 10x10 area to determine which rectangles are 'in the neighborhood' of the search area.
//Note they may not necessarily overlap the rectangle. They are simply close enough that the QuadTree flags them as overlapping candidates.
var searchRect = new Rectangle() { Width = 10.0f, Height = 10.0f, CenterX = rnd.Next(0,500), CenterY = rnd.Next(0,500) };
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
	if(searchRect.Intersects(candidate))
	{
		collisions.Add(candidate);
	}
}
```