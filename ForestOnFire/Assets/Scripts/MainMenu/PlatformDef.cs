using UnityEngine;
using System.Collections;

public class PlatformDef : MonoBehaviour
{
    public GameObject pic1, pic2;
    void Start()
    {
        #if UNITY_ANDROID          
            
            pic1.SetActive(false);
            pic2.SetActive(false);            
        #endif
    }
}