using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    int mapLevel = 1;
    public Hexagon[][] hexsArray;

    public Map(int level)
    {
        mapLevel = level;
        int tempLenght = level * 2 + 1;
        hexsArray = new Hexagon[tempLenght][];
        for (int i = 0; i < tempLenght; i++)
        {
            if (i <= mapLevel)
            {
                hexsArray[i] = new Hexagon[i + level + 1];
            }
            else
            {
                hexsArray[i] = new Hexagon[tempLenght - i + level];
            }
        }

        for (int i = 0; i < hexsArray.Length; i++)
        {
            for (int j = 0; j < hexsArray[i].Length; j++)
            {
                if (Random.Range(0f, 1.0f) > 0.6f)
                {
                    hexsArray[i][j] = new Hexagon(i, j, 1);
                }
                else
                {
                    hexsArray[i][j] = new Hexagon(i, j, Random.Range(2, Hexagon.typesList.Length));
                }
            }
        }
        hexsArray[level][level] = new Hexagon(level, level, 0);

        for (int i = 0; i < hexsArray.Length; i++)
        {
            for (int j = 0; j < hexsArray[i].Length; j++)
            {
                Hexagon hex = hexsArray[i][j];
                hex.CubeX = i - level;
                if (i <= level)
                {
                    hex.CubeZ = j - hexsArray[i].Length + (level + 1);// + tempLenght;                
                }
                else
                {
                    hex.CubeZ = j - level;
                }
                hex.CubeY = -hex.CubeX - hex.CubeZ;
            }
        }
    }

    public Hexagon findHexagon(int x, int y, int z)
    {
        for (int i = 0; i < hexsArray.Length; i++)
        {
            for (int j = 0; j < hexsArray[i].Length; j++)
            {
                Hexagon currentHex = hexsArray[i][j];
                if (currentHex.CubeX == x &&
                    currentHex.CubeY == y &&
                    currentHex.CubeZ == z) return currentHex;
            }
        }
        return null;
    }

    public Hexagon[] findNeighbors(Hexagon hex)
    {
        int x = hex.CubeX, y = hex.CubeY, z = hex.CubeZ;
        Hexagon[] temp =
        {
            this.findHexagon(x + 1, y - 1, z),
            this.findHexagon(x + 1, y, z - 1),
            this.findHexagon(x, y + 1, z - 1),
            this.findHexagon(x - 1, y + 1, z),
            this.findHexagon(x - 1, y, z + 1),
            this.findHexagon(x, y - 1, z + 1),
        };
        int counter = 0;
        List<Hexagon> list = new List<Hexagon>(); 
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] != null)
            {
                counter++;
                list.Add(temp[i]);
            }
        }
        return list.ToArray();        
    }
}
