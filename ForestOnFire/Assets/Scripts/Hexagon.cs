using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon
{

    public static HexType[] typesList = new HexType[]{
                new HexType(0, "spawn", 0, 0.5f, true),
                new HexType(1, "forest", 0.0075f, 1, true),
                new HexType(2, "mountain", 0, 0.5f, false),
                new HexType(3, "empty", 0.0025f, 0.75f, true),
                new HexType(4, "field", 0.0075f, 1.25f, true),
                new HexType(5, "water", 0, 0.5f, true),
                new HexType(6, "house", 0.05f, 1.5f, true)
    };
    int x, y;
    int cubeX, cubeY, cubeZ;
    HexType type;
    public Transform model;
    public EventState eventState;
    public bool isBusy = false;
    public Transform effect = null;
    public EventState currentEvent;
    public int g, h;
    public int F => g + h;
    public Hexagon(int x, int y, int typeId)
    {
        this.x = x;
        this.y = y;
        this.type = typesList[typeId];
        this.eventState = new EventState(this);
    }

    public int X { get => x; }
    public int Y { get => y; }
    public HexType Type { get => type; }
    public int CubeX { get => cubeX; set => cubeX = value; }
    public int CubeY { get => cubeY; set => cubeY = value; }
    public int CubeZ { get => cubeZ; set => cubeZ = value; }
}
