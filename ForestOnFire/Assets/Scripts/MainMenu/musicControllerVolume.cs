using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicControllerVolume : MonoBehaviour
{
    private float defaultVol = 1f;
    public AudioSource[] sounds;
    void Start()
    {
        Debug.Log(PlayerPrefs.GetFloat("Volume"));
        if (PlayerPrefs.GetFloat("Volume", -1f) == -1f)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].volume = sounds[i].volume*defaultVol;
            }
        }
        else
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].volume = PlayerPrefs.GetFloat("Volume");
            }
        }
    }

}
