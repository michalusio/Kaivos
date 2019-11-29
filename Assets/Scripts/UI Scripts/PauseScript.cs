using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject PauseOverlay;
    public GameObject PausePanel;
    public GameObject SavePanel;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        Time.timeScale = 1 - Time.timeScale;
        PausePanel.SetActive(true);
        SavePanel.SetActive(false);
        PauseOverlay.SetActive(!PauseOverlay.activeSelf);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void ToggleSaveGame()
    {
        PausePanel.SetActive(!PausePanel.activeSelf);
        SavePanel.SetActive(!SavePanel.activeSelf);
        if (SavePanel.activeSelf)
        {
            SavePanel.GetComponent<SaveScript>().Populate();
        }
    }
}
