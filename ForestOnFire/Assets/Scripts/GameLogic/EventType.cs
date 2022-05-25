using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventType
{
    public string name;
    public int id;
    public int steps;
    public Transform model;

    public EventType(int id, string name, int steps, Transform model)
    {
        this.id = id;
        this.name = name;
        this.steps = steps;
        this.model = model;
    }
}
