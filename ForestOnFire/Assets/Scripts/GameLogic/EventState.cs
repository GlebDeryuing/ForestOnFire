using System.Collections.Generic;
using UnityEngine;

public class EventState
{    
    public Hexagon hex;
    public int step;

    public EventType currentType;
    public Transform[] models;
    public static EventType[] typesList = new EventType[3];

   
    public EventState(Hexagon hex)
    {
        this.hex = hex;
        currentType = null;
        step = 0;
    }

    public void setFire() {
        step = 10;
        currentType = typesList[0];
    }

    public void setExtincting()
    {
        currentType = typesList[1];
    }

    public void setBurnt()
    {
        currentType = typesList[2];
    }
}