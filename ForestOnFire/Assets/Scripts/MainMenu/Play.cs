using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public GameObject playmenu;
    private int gameDifficulty = 2;

    public GameObject diff;
    public GameObject diffbg;
    public Button[] allDiffs = new Button[3];
    void Start()
    {      
        PlayerPrefs.SetInt("Difficulty", 2);
        playmenu.SetActive(false);
    }

    public void setDiff(int val)
    {
        gameDifficulty = 2 + val;
        for (int i = 0; i < allDiffs.Length; i++)
        {
            allDiffs[i].GetComponentInChildren<Text>().color = new Color32(255, 128, 0, 255);
        }
        allDiffs[val].GetComponentInChildren<Text>().color = Color.white;
        diffbg.transform.localPosition = new Vector3(214 * (val - 1), 0);
    }

    public void saveAndStart()
    {
        PlayerPrefs.SetInt("Difficulty", gameDifficulty);
        SceneManager.LoadScene("SampleScene");
    }
}
