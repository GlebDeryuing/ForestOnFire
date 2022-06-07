using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoReciever : MonoBehaviour
{
    public GameObject bgday, bgnight;
    public Light light;
    void Start()
    {
        int isDay = PlayerPrefs.GetInt("DaySetting");
        light.intensity = 1f;
        if (isDay == 1)
        {
            bgday.SetActive(true);
            bgnight.SetActive(false);
            light.color = Color.white;
            return;
        }
        light.color = new Color32(150, 150, 150, 255); 
        bgday.SetActive(false);
        bgnight.SetActive(true);
    }

}
