using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{    
    void Start()
    {
        int level = GameObject.FindGameObjectsWithTag("Grid")[0].GetComponent<GridGenerator>().gridLevel;
        transform.position = new Vector3(0, 7 + Mathf.Pow(level, 1.15f) * 2f, -Mathf.Pow(level, 1.1f)*1.7f - 3);
    }
    private void Update()
    {
        transform.Rotate(Mathf.Sin(Time.time)*0.005f, Mathf.Sin(Time.time*0.7f) * 0.003f, 0);
    }

}
