using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public GameObject settings;
    private bool isDay = true, isHigh = true;
    public GameObject day, graphic;
    public GameObject daybg, graphicbg;
    private Button[] allDays = new Button[2], allGrahpic = new Button[2];

    public Light light;
    public Image skybg;
    void Start()
    {
        GameObject[] allDaysGO = GameObject.FindGameObjectsWithTag("daysettings");
        GameObject[] allGraphicsGO = GameObject.FindGameObjectsWithTag("graphicsettings");
        for (int i = 0; i < allDaysGO.Length; i++)
        {
            allDays.SetValue(allDaysGO[i].GetComponent<Button>(),i);
            allGrahpic.SetValue(allGraphicsGO[i].GetComponent<Button>(), i);
        }
        PlayerPrefs.SetInt("GraphicSetting", isHigh ? 1 : 0);
        PlayerPrefs.SetInt("DaySetting", isDay ? 1 : 0);
        settings.SetActive(false);
    }

    public void setDay(bool val)
    {
        if (val)
        {
            isDay = true;
            allDays[0].GetComponentInChildren<Text>().color = Color.white;
            allDays[1].GetComponentInChildren<Text>().color = new Color32(255, 128, 0, 255);
            daybg.transform.localPosition = new Vector3(-160f, 0);
            light.color = new Color32(255, 244, 214, 255);
            skybg.color = Color.white;
            return;
        }
        else
        {
            isDay = false;
            allDays[1].GetComponentInChildren<Text>().color = Color.white;
            allDays[0].GetComponentInChildren<Text>().color = new Color32(255, 128, 0, 255);
            daybg.transform.localPosition = new Vector3(160f, 0);
            light.color = new Color32(100, 100, 100, 255);
            skybg.color = new Color32(150, 150, 150, 255);
            return;
        }
    }

    public void setGraphic(bool val)
    {
        if (val)
        {
            isHigh = true;
            allGrahpic[0].GetComponentInChildren<Text>().color = Color.white;
            allGrahpic[1].GetComponentInChildren<Text>().color = new Color32(255, 128, 0, 255);
            graphicbg.transform.localPosition = new Vector3(-160f, 0);
            QualitySettings.SetQualityLevel(5);
            Debug.Log(QualitySettings.GetQualityLevel());
            return;
        }
        else
        {
            isHigh = false;
            allGrahpic[1].GetComponentInChildren<Text>().color = Color.white;
            allGrahpic[0].GetComponentInChildren<Text>().color = new Color32(255, 128, 0, 255);
            graphicbg.transform.localPosition = new Vector3(160f, 0);
            QualitySettings.SetQualityLevel(0);
            Debug.Log(QualitySettings.GetQualityLevel());
            return;
        }
    }

    public void saveSettings()
    {
        PlayerPrefs.SetInt("GraphicSetting", isHigh ? 1 : 0);
        PlayerPrefs.SetInt("DaySetting",  isDay? 1 : 0);
    }

    public void exit()
    {
        setDay(PlayerPrefs.GetInt("DaySetting") ==1 ? true : false);
        setGraphic(PlayerPrefs.GetInt("GraphicSetting") ==1 ? true : false);
        settings.SetActive(false);
    }

}
