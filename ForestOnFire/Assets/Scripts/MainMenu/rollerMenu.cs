using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollerMenu : MonoBehaviour
{

    public Transform[] hexsPrefabs;
    Transform hex;
    int rotation = 0;
    void Start()
    {
        hex = Instantiate(hexsPrefabs[Random.Range(0, hexsPrefabs.Length)]) as Transform;
        hex.position = this.transform.position;
        hex.localScale = new Vector3(100, 100, 100);
    }

    public float speed;

    void Update()
    {
        hex.Rotate(Vector3.up * speed * Time.deltaTime);
    
    }
}
