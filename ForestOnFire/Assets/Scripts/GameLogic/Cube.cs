using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cube
{
    static public int Distance(Hexagon A, Hexagon B)
    {
        return (Mathf.Abs(A.CubeX - B.CubeX) + Mathf.Abs(A.CubeY - B.CubeY) + Mathf.Abs(A.CubeZ - B.CubeZ)) / 2;
    }
    
    static public float Lerp (int a, int b, float t)
    {
        return a + (b - a) * t;
    }

    static public float[] CubeLerp (Hexagon A, Hexagon B, float t)
    {
        return new float[]
        {
            Lerp(A.CubeX, B.CubeX, t),
            Lerp(A.CubeY, B.CubeY, t),
            Lerp(A.CubeZ, B.CubeZ, t)
        };
    }

    static public Hexagon[] LineList(Hexagon A, Hexagon B)
    {
        Map map = GameObject.FindGameObjectsWithTag("Grid")[0].GetComponent<GridGenerator>().currentMap;
        int N = Distance(A, B);
        Hexagon[] results = new Hexagon[N+1];
        for (int i = 0; i <= N; i++) 
        {
            float[] coords = CubeLerp(A, B, 1.0f / N * i);
            Hexagon hex = map.findHexagon(Mathf.RoundToInt(coords[0]), Mathf.RoundToInt(coords[1]), Mathf.RoundToInt(coords[2]));
            results[i] = hex;
        }
        return results;
    }

    public static Hexagon[] FindPath(Hexagon startPoint, Hexagon endPoint)
    {
        Map map = GameObject.FindGameObjectsWithTag("Grid")[0].GetComponent<GridGenerator>().currentMap;
        List<Hexagon> openPathTiles = new List<Hexagon>();
        List<Hexagon> closedPathTiles = new List<Hexagon>();

        // Prepare the start tile.
        Hexagon currentTile = startPoint;

        currentTile.g = 0;
        currentTile.h = Distance(startPoint, endPoint);

        // Add the start tile to the open list.
        openPathTiles.Add(currentTile);

        while (openPathTiles.Count != 0)
        {
            // Sorting the open list to get the tile with the lowest F.
            openPathTiles = openPathTiles.OrderBy(x => x.F).ThenByDescending(x => x.g).ToList();
            currentTile = openPathTiles[0];

            // Removing the current tile from the open list and adding it to the closed list.
            openPathTiles.Remove(currentTile);
            closedPathTiles.Add(currentTile);

            int g = currentTile.g + 1;

            // If there is a target tile in the closed list, we have found a path.
            if (closedPathTiles.Contains(endPoint))
            {
                break;
            }

            // Investigating each adjacent tile of the current tile.
            List<Hexagon> adjacentTiles = map.findNeighbors(currentTile);
            foreach(Hexagon adjacentTile in adjacentTiles)
            {
                // Ignore not walkable adjacent tiles.
                if (!adjacentTile.Type.CanGoThrough)
                {
                    continue;
                }

                // Ignore the tile if it's already in the closed list.
                if (closedPathTiles.Contains(adjacentTile))
                {
                    continue;
                }

                // If it's not in the open list - add it and compute G and H.
                if (!(openPathTiles.Contains(adjacentTile)))
                {
                    adjacentTile.g = g;
                    adjacentTile.h = Distance(adjacentTile, endPoint);
                    openPathTiles.Add(adjacentTile);
                }
                // Otherwise check if using current G we can get a lower value of F, if so update it's value.
                else if (adjacentTile.F > g + adjacentTile.h)
                {
                    adjacentTile.g = g;
                }
            }
        }

        List<Hexagon> finalPathTiles = new List<Hexagon>();

        // Backtracking - setting the final path.
        if (closedPathTiles.Contains(endPoint))
        {
            currentTile = endPoint;
            finalPathTiles.Add(currentTile);

            for (int i = endPoint.g - 1; i >= 0; i--)
            {
                List<Hexagon> adjacentTiles = map.findNeighbors(currentTile);
                currentTile = closedPathTiles.Find(x => x.g == i && adjacentTiles.Contains(x));
                finalPathTiles.Add(currentTile);
            }

            finalPathTiles.Reverse();
        }

        return finalPathTiles.ToArray();
    }
}
