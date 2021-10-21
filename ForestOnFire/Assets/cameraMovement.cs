using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int level = GameObject.FindGameObjectsWithTag("Grid")[0].GetComponent<GridGenerator>().gridLevel;
        transform.position=new Vector3(0, 6+Mathf.Pow(level, 1.1f)*2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
