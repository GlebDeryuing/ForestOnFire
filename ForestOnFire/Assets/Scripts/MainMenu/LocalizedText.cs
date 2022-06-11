using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string ruText = "";
    public string enText = "";

    static List<LocalizedText> allText = new List<LocalizedText>(); 
    void Start()
    {
        int temp = PlayerPrefs.GetInt("LangSetting", -1);
        if (temp != -1)
        {
            bool isTempEng = temp == 1 ? true : false;
            translateToEng(isTempEng);
        }
        else
        {
            translateToEng(true);
        }
        allText.Add(transform.GetComponent<LocalizedText>());
    }

    public void translateToEng(bool isEng)
    {
        transform.GetComponent<Text>().text = isEng ? enText:ruText;
    }

    public static LocalizedText[] getAll(bool Get = true)
    {
        if (Get)
        {
            return allText.ToArray();
        }
        else
        {
            allText.Clear();
            return allText.ToArray();
        }
    }
}
