using UnityEngine;
using UnityEngine.EventSystems;

public class clickReciever : MonoBehaviour, IPointerClickHandler
{
    static Hexagon last = null;
    public void OnPointerClick(PointerEventData eventData)
    {
        Map map = GameObject.FindGameObjectsWithTag("Grid")[0].GetComponent<GridGenerator>().currentMap;
        Hexagon clicked = eventData.pointerCurrentRaycast.gameObject.GetComponent<GameInfo>().hex;
        if (last == null)
        {
            last = clicked;
        }
        else
        {
            Hexagon[] hexs = Cube.LineList(last, clicked);
            foreach (Hexagon hex in hexs)
            {
                hex.model.position = hex.model.position + new Vector3(0, -1, 0);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
