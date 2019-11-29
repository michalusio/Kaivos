using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MenuPanel;

    void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        QualitySettings.vSyncCount = 1;
        var panelImage = MenuPanel.GetComponent<Image>();
        panelImage.material.SetVector("_Sizes", new Vector4(Screen.width, Screen.height, panelImage.material.mainTexture.width, panelImage.material.mainTexture.height));
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "Menu")
        {
            MainScript.LoadPath = null;
            Debug.Log("load path cleared");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
