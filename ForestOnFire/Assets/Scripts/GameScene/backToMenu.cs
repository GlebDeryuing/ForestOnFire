using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backToMenu : MonoBehaviour
{
    public void goBack()
    {
        LocalizedText.getAll(false);
        SceneManager.LoadScene("MainMenu");
    }
}
