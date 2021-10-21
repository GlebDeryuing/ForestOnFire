using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stepController : MonoBehaviour
{
    static int stepCount=0;
    static bool isAvalible = true;
    public Transform player;
    Vector3 position;
    public float speed;
    Queue<Vector3> nextList = new Queue<Vector3>();
    Queue<Hexagon> hexNextList = new Queue<Hexagon>();
    Hexagon lastHex = null;
    Map map;

    public Transform effects;
    public Transform fire;

    public Transform[] models;



    private void Start()
    {
        map = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridGenerator>().currentMap;
        position = player.position;

        Transform[] models = GameObject.FindGameObjectWithTag("controller").GetComponent<stepController>().models;
        EventState.typesList = new EventType[]{
                new EventType(0, "fire", 10, models[0]),
                new EventType(1, "extincting", 2, models[1]),
                new EventType(2, "burnt", 3, models[2])
        };
    }
    public void listSteps(Vector3[] positions, Hexagon[] hexs)
    {
        nextList.Clear();
        hexNextList.Clear();
        for (int i=0; i<positions.Length; i++)
        {
            nextList.Enqueue(positions[i]);
            hexNextList.Enqueue(hexs[i]);
        }
        nextStep(nextList.Dequeue(), hexNextList.Dequeue());
    }

    public void nextStep(Vector3 position, Hexagon hex)
    {
        this.position = position;
        hex.isBusy = true;
        lastHex = hex;
        if (hex.eventState.currentType != null && hex.eventState.currentType.name=="fire")
        {
            Destroy(hex.effect.gameObject);
            hex.eventState.setExtincting();
            Vector3 pos = hex.model.position;
            Transform model = Instantiate(hex.eventState.currentType.model) as Transform;
            hex.effect = model;
            model.parent = effects;
            model.position = pos;
        }
        fireStep();
    }

    public void fireStep()
    {
        stepCount++;
        Debug.Log(stepCount);
        for (int i = 0; i < map.hexsArray.Length; i++)
        {
            for (int j = 0; j < map.hexsArray[i].Length; j++)
            {
                if (map.hexsArray[i][j].effect == null)
                {

                    float chanse = map.hexsArray[i][j].Type.TickFireChanse;
                    foreach (Hexagon neighbor in map.findNeighbors(map.hexsArray[i][j]))
                    {
                        chanse *= neighbor.Type.FactorFire;
                        if (neighbor.effect != null) chanse *= 3;
                    }
                    if (Random.Range(0, 1.0f) < chanse)
                    {
                        map.hexsArray[i][j].eventState.setFire();
                        Vector3 pos = map.hexsArray[i][j].model.position;
                        Transform model = Instantiate(map.hexsArray[i][j].eventState.currentType.model) as Transform;
                        map.hexsArray[i][j].effect = model;
                        model.parent = effects;
                        model.position = pos;
                    }
                }
            }
        }

        isAvalible = false;
    }
    private void FixedUpdate()
    {
        player.position = Vector3.Lerp(player.position, position, speed);
        
        if (isAvalible)
        {
            if (nextList.Count > 0)
            {
                if (lastHex != null) lastHex.isBusy = false;
                nextStep(nextList.Dequeue(), hexNextList.Dequeue());
            }
            else
            {
                fireStep();
            }
        }
    }

    static public void CanGoNext()
    {
        isAvalible = true;
    }
}
