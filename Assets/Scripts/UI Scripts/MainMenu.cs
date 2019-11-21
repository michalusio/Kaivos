using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 1;
    }
    public void PlayGame ()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit ();
    }
}
