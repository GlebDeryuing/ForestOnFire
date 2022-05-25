using UnityEngine;
using UnityEngine.EventSystems;

public class clickReciever : MonoBehaviour, IPointerClickHandler
{
    static Hexagon last = null;
    public Transform secondaryGrid;
    static public Transform[] eventsPrefabs;
    public Transform[] eventsTempLinks;
    public Material material;

    public void OnPointerClick(PointerEventData eventData)
    {
        Map map = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridGenerator>().currentMap;
        Hexagon clicked = eventData.pointerCurrentRaycast.gameObject.GetComponent<GameInfo>().hex;
        if (clicked.isBusy == false && last == null)
        {
            return;
        }
        if (last == null)
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
            last = clicked;
        }
        else if (last != clicked)
        {
            Hexagon[] hexs = Cube.FindPath(last, clicked);
            Vector3[] positions = new Vector3[hexs.Length];
            Transform prev = null;
            for (int i = 0; i < hexs.Length; i++)
            {
                Vector3 pos = hexs[i].model.position;
                Transform step = Instantiate(eventsPrefabs[0]) as Transform;
                step.position = pos + new Vector3(0, 1, 0);
                positions[i] = step.position;
                step.parent = secondaryGrid;
                if (prev != null)
                {
                    GameObject line = new GameObject("Line");
                    line.transform.parent = secondaryGrid;
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;
                    lineRenderer.positionCount = 2;
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.material = material;
                    lineRenderer.generateLightingData = true;
                    //For drawing line in the world space, provide the x,y,z values
                    lineRenderer.SetPosition(0, step.position); //x,y and z position of the starting point of the line
                    lineRenderer.SetPosition(1, prev.position); //x,y and z position of the starting point of the line
                }
                prev = step;
            }
            last = null;

            stepController controller = GameObject.FindGameObjectWithTag("controller").GetComponent<stepController>();
            controller.listSteps(positions, hexs);
        }


    }

    
    // Start is called before the first frame update
    void Start()
    {
        eventsPrefabs = eventsTempLinks;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
