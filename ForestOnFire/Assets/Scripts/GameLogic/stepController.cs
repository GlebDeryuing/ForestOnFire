using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stepController : MonoBehaviour
{
    static public int SCORE = 0;
    public Text steps;
    static int stepCounter = 0;
    public GameObject winScreen;
    public Text winText;
    public static bool canRotate = false;

    static int canBurn = 0;
    static int canFire = 0;
    static int burned = 0;
    static int inFire = 0;
    public Text canFireText, burnedText, inFireText;

    public Animator infoPanelAnimator;
    public Text infoText;
    static bool hasChanged = true;

    static bool isAvalible = true;
    public Transform player;
    Vector3 position;
    public float speed;
    Queue<Vector3> nextList = new Queue<Vector3>();
    Queue<Hexagon> hexNextList = new Queue<Hexagon>();
    Hexagon lastHex = null;
    Map map;

    public static int wait = 0;
    public static bool fired = false;

    public Transform effects;
    public Transform fire;
    public Material burned_material;

    public Transform[] models;

    public Transform secondaryGrid;

    string[] eventsReasons = {"peat spontaneous combustion",
        "unextracted cigarette",
        "child's pranks",
        "family picnic with BBQ",
        "intentional arson",
        "careless burning of garbage" };

    private void Start()
    {
        canBurn = 0;
        canFire = 0;
        burned = 0;
        inFire = 0;
        fired = false;
        wait = 0;
        stepCounter = 0;
        canRotate = false;
        statisticUpdate(canFireText, burnedText, inFireText);
        map = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridGenerator>().currentMap;
        position = player.position;

        Transform[] models = GameObject.FindGameObjectWithTag("controller").GetComponent<stepController>().models;
        EventState.typesList = new EventType[]{
                new EventType(0, "fire", 10, models[0]),
                new EventType(1, "extincting", 2, models[1]),
                new EventType(2, "burnt", 3, models[2])
        };
        Hexagon hex = null;
        while (hex == null)
        {
            int i = Random.Range(0, map.hexsArray.Length - 1);
            int j = Random.Range(0, map.hexsArray[i].Length - 1);
            if (map.hexsArray[i][j].Type.TickFireChanse > 0)
            {
                hex = map.hexsArray[i][j];
            }
        }
        for (int i = 0; i < map.hexsArray.Length; i++)
        {
            for (int j = 0; j< map.hexsArray[i].Length; j++)
            {
                if (map.hexsArray[i][j].Type.TickFireChanse > 0)
                {
                    canBurn++;
                    infoSay("HEX " + i + "/" + j + " caught fired due to an " + eventsReasons[Random.Range(0, eventsReasons.Length - 1)] + ".", infoText, infoPanelAnimator);
                }
            }
        }
        canFire = canBurn;
        inFire++;
        canFire--;
        hex.eventState.setFire();
        Vector3 pos = hex.model.position;
        Transform model = Instantiate(hex.eventState.currentType.model) as Transform;
        hex.effect = model;
        model.parent = effects;
        model.position = pos;

        statisticUpdate(canFireText, burnedText, inFireText);
    }
    public void listSteps(Vector3[] positions, Hexagon[] hexs)
    {
        nextList.Clear();
        hexNextList.Clear();
        for (int i = 0; i < positions.Length; i++)
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
        if (hex.eventState.currentType != null && hex.eventState.currentType.name == "fire")
        {
            SCORE += 100*hex.eventState.currentType.steps+100;
            Destroy(hex.effect.gameObject);
            hex.eventState.setExtincting();
            Vector3 pos = hex.model.position;
            Transform model = Instantiate(hex.eventState.currentType.model) as Transform;
            hex.effect = model;
            model.parent = effects;
            model.position = pos;
            wait = 2;
        }
        fireStep();
    }

    public void fireStep()
    {
        hasChanged = true;
        stepCounter++;

        List<Hexagon> potentialBurn = new List<Hexagon>();
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
                        if (chanse > 0 && !map.hexsArray[i][j].isBusy)
                        {
                            potentialBurn.Add(map.hexsArray[i][j]);
                        }
                    }
                    if (Random.Range(0, 1.0f) < chanse && !map.hexsArray[i][j].isBusy)
                    {
                        inFire++;
                        canFire--;
                        map.hexsArray[i][j].eventState.setFire();
                        Vector3 pos = map.hexsArray[i][j].model.position;
                        Transform model = Instantiate(map.hexsArray[i][j].eventState.currentType.model) as Transform;
                        map.hexsArray[i][j].effect = model;
                        model.parent = effects;
                        model.position = pos;
                        infoSay("HEX " + i + "/" + j + " caught fired due to an " + eventsReasons[Random.Range(0, eventsReasons.Length - 1)] + ".", infoText, infoPanelAnimator);
                    }
                }
                else if (map.hexsArray[i][j].effect != null&&map.hexsArray[i][j].eventState.currentType.name == "fire")
                {
                    map.hexsArray[i][j].eventState.step -= 1;
                    if (map.hexsArray[i][j].eventState.step < 1)
                    {
                        SCORE -= 100;
                        canBurn--;
                        inFire--;
                        burned++;
                        Destroy(map.hexsArray[i][j].effect.gameObject);
                        map.hexsArray[i][j].eventState.setBurnt();
                        Vector3 pos = map.hexsArray[i][j].model.position;
                        Transform model = Instantiate(map.hexsArray[i][j].eventState.currentType.model) as Transform;
                        map.hexsArray[i][j].effect = model;
                        model.parent = effects;
                        model.position = pos;
                        for (int k = 0; k < map.hexsArray[i][j].model.childCount; k++)
                        {
                            Transform child = map.hexsArray[i][j].model.GetChild(k);
                            Material[] temp = new Material[1];
                            temp[0] = burned_material;
                            try
                            {
                                child.gameObject.GetComponent<Renderer>().sharedMaterials = temp;
                            }
                            catch (MissingComponentException ex)
                            {
                                try
                                {
                                    for (int m = 0; m < child.childCount; m++)
                                    {
                                        child.GetChild(m).gameObject.GetComponent<Renderer>().sharedMaterials = temp;
                                    }
                                }
                                catch (MissingComponentException ex2)
                                {
                                    return;
                                }
                                
                            }
                        }

                    }
                }
            }
        }
        if (inFire < 1)
        {

            Hexagon[] arrayPotential = potentialBurn.ToArray();
            Hexagon hex = arrayPotential[Random.Range(0, arrayPotential.Length - 1)];
            if (hex != null)
            {                
                hex.eventState.setFire();
                Vector3 pos = hex.model.position;
                Transform model = Instantiate(hex.eventState.currentType.model) as Transform;
                hex.effect = model;
                model.parent = effects;
                model.position = pos;
                inFire++;
                canFire--;
            }
        }
        isAvalible = false;
    }
    private void Update()
    {
        if (hasChanged)
        {
            steps.text = stepCounter + " STEPS";
            hasChanged = false;
        }
        if (canRotate&&(position + Vector3.up - player.position).magnitude > 0.01)
        {
            player.rotation = Quaternion.LookRotation(position + Vector3.up - player.position);
        }
        player.position = Vector3.Lerp(player.position, position + Vector3.up, speed);

        if (nextList.Count == 0)
        {
            int i = 0;
            GameObject[] allChildren = new GameObject[secondaryGrid.childCount];
            foreach (Transform child in secondaryGrid)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }
            foreach (GameObject child in allChildren)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        if (isAvalible)
        {

            for (int i = 0; i<map.hexsArray.Length; i++)
            {
                for(int j = 0; j < map.hexsArray[i].Length; j++)
                {
                    if (map.hexsArray[i][j].effect!=null && map.hexsArray[i][j].effect.name == "smoke(Clone)")
                    {
                        for (int k = 0; k< map.hexsArray[i][j].effect.childCount; k++)
                        {
                            Destroy(map.hexsArray[i][j].effect.GetChild(k).gameObject);
                        }
                    }
                }
            }
            if (canBurn < 2)
            {
                winScreen.SetActive(true);
                winText.text = SCORE + " POINTS";
                return;
            }
            if (fired && lastHex != null)
            {
                lastHex.eventState.currentType = null;
                Destroy(lastHex.effect.gameObject);
                lastHex.effect = null;
                fired = false;
                inFire--;
                canFire++;
            }

            if (nextList.Count > 0)
            {
                if (lastHex != null) lastHex.isBusy = false;
                nextStep(nextList.Dequeue(), hexNextList.Dequeue());

            }
            else
            {
                fireStep();
            }
            statisticUpdate(canFireText, burnedText, inFireText);
        }
    }

    static public void CanGoNext()
    {
        canRotate = true;
        if (wait > 0)
        {
            hasChanged = true;
            stepCounter++;
            wait = wait - 1;
            if (wait == 0)
            {
                fired = true;
            }
            return;
        }
        isAvalible = true;
    }

    static void infoSay(string info, Text text, Animator anim)
    {
        text.text = info;
        anim.Play("popup-panel", -1, 0f);
    }

    static void statisticUpdate(Text a, Text b, Text c)
    {
        a.text = canFire.ToString();
        b.text = burned.ToString();
        c.text = inFire.ToString();

    }
}
