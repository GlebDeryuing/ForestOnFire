using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridGenerator : MonoBehaviour
{
    public Map currentMap;
    public int gridLevel = 1;
    int maxLenght;
    int minLenght;
    float hexWidth = 1.732f;
    float hexHeight = 2.0f;
    public float gap = 0.0f;

    static public Transform[] hexsPrefabs;
    public Transform[] hexsTempLinks;

    Vector3 startPos;

    void Start()
    {
        hexsPrefabs = hexsTempLinks;
        currentMap = new Map(gridLevel);
        maxLenght = 1 + gridLevel * 2;
        minLenght = 1 + gridLevel;
        AddGap();
        CalcStartPos();
        CreateGrid();
    }

    void AddGap()
    {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    void CalcStartPos()
    {
        float offset = 0;
        float x = -hexWidth * (maxLenght / 2) - offset;
        float z = hexHeight * 0.75f * (maxLenght / 2);

        startPos = new Vector3(x, 0, z);
    }

    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        offset = Mathf.Abs(minLenght - gridPos.y - 1) * hexWidth / 2;
        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    void CreateGrid()
    {
        int currentLenght = minLenght - 1;
        for (int y = 0; y < maxLenght; y++)
        {
            if (y < minLenght)
            {
                currentLenght++;
            }
            else
            {
                currentLenght--;
            }
            for (int x = 0; x < currentLenght; x++)
            {
                Hexagon currentHex = currentMap.hexsArray[y][x];
                Transform[]type = currentHex.Type.CurrentPrefabs.ToArray();
                Transform hex = Instantiate(type[Random.Range(0, type.Length)]) as Transform;
                hex.GetComponent<GameInfo>().hex = currentHex;
                currentHex.model = hex;
                hex.Rotate(0, Random.Range(0, 5) * 60, 0);
                Vector2 gridPos = new Vector2(x, y);
                hex.position = CalcWorldPos(gridPos);
                hex.parent = this.transform;
                hex.name = "Hex, X:" + currentHex.CubeX + ", Y:" + currentHex.CubeY + ", Z:" + currentHex.CubeZ;
            }

        }
        currentMap.hexsArray[gridLevel][gridLevel].model.rotation = new Quaternion(0, 0, 0, 0);
}
} 






