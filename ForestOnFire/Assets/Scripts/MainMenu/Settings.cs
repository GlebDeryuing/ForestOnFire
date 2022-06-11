using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public GameObject settings;
    private bool isDay = true, isHigh = true, isEng = true;
    public GameObject day, graphic, lang;
    public GameObject daybg, graphicbg, langbg;
    private Button[] allDays = new Button[2], allGrahpic = new Button[2], allLang = new Button[2];

    private float lastSave = 1f, newValue = 1f;
    public AudioSource[] sounds;
    public Slider slider;

    public Light light;
    public Image skybg;

    public GameObject howtoplay;

    public GameObject[] allDaysGO, allGraphicsGO, allLangGO;

    void OnEnable()
    {
        updateLang();
    }

    void Start()
    {
        updateLang();
        howtoplay.SetActive(false);

        for (int i = 0; i < allDaysGO.Length; i++)
        {
            allDays.SetValue(allDaysGO[i].GetComponent<Button>(), i);
            allGrahpic.SetValue(allGraphicsGO[i].GetComponent<Button>(), i);
            allLang.SetValue(allLangGO[i].GetComponent<Button>(), i);
        }
        settings.SetActive(false);

        int tempgraphic = PlayerPrefs.GetInt("GraphicSetting", -1);
        if (tempgraphic != -1)
        {
            setGraphic(tempgraphic == 1 ? true : false);
        }
        else
        {
            PlayerPrefs.SetInt("GraphicSetting", 1);
        }
        int templang = PlayerPrefs.GetInt("LangSetting", -1);
        if (templang != -1)
        {
            setLang(templang == 1 ? true : false);
        }
        else
        {
            PlayerPrefs.SetInt("LangSetting", 1);
        }
        int tempday = PlayerPrefs.GetInt("DaySetting", -1);
        if (tempday != -1)
        {
            setDay(tempday == 1 ? true : false);
        }
        else
        {
            PlayerPrefs.SetInt("DaySetting", 1);
        }
        float tempvolume = PlayerPrefs.GetFloat("Volume", -1);
        if (tempvolume != -1)
        {
            slider.value = tempvolume;
        }
        else
        {
            PlayerPrefs.SetFloat("Volume", 1);
        }
        saveSettings();

        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].volume = lastSave;
        }
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

    public void setLang (bool val)
    {
        if (val)
        {
            isEng = true;
            allLang[0].GetComponentInChildren<Text>().color = Color.white;
            allLang[1].GetComponentInChildren<Text>().color = new Color32(255, 128, 0, 255);
            langbg.transform.localPosition = new Vector3(-160f, 0);         
            return;
        }
        else
        {
            isEng = false;
            allLang[1].GetComponentInChildren<Text>().color = Color.white;
            allLang[0].GetComponentInChildren<Text>().color = new Color32(255, 128, 0, 255);
            langbg.transform.localPosition = new Vector3(160f, 0);
            return;
        }        
    }

    public void saveSettings()
    {
        Debug.Log(newValue);
        PlayerPrefs.SetInt("GraphicSetting", isHigh ? 1 : 0);
        PlayerPrefs.SetInt("DaySetting",  isDay? 1 : 0);
        newValue = slider.value;
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].volume = newValue;
        }
        lastSave = newValue;
        PlayerPrefs.SetFloat("Volume", newValue);

        PlayerPrefs.SetInt("LangSetting", isEng ? 1 : 0);
        LocalizedText[] allLocalized = LocalizedText.getAll();
        for (int i = 0; i < allLocalized.Length; i++)
        {
            allLocalized[i].translateToEng(isEng);
        }

    }

    public void updateLang()
    {
        
        int temp = PlayerPrefs.GetInt("LangSetting", -1);
        Debug.Log(temp+" lang");
        if (temp != -1)
        {
            bool isTempEng = temp == 1 ? true : false;
            LocalizedText[] allLocalized = LocalizedText.getAll();
            for (int i = 0; i < allLocalized.Length; i++)
            {
                allLocalized[i].translateToEng(isTempEng);
            }
        }
    }

    public void exit()
    {
        setDay(PlayerPrefs.GetInt("DaySetting") ==1 ? true : false);
        setLang(PlayerPrefs.GetInt("LangSetting") == 1 ? true : false);
        setGraphic(PlayerPrefs.GetInt("GraphicSetting") ==1 ? true : false);
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].volume = lastSave;
        }
        settings.SetActive(false);
    }

}
