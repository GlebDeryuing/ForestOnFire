using System.Collections;
using System.Collections.Generic;
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
}
