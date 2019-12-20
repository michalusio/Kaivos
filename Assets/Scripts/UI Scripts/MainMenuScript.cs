using System.Collections;
using Assets.Scripts;
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
        StartCoroutine(PlayGameCoroutine());
    }

    private IEnumerator PlayGameCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        ClassManager.Clear();
        SceneManager.LoadScene("MainScene");
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameCoroutine());
    }

    private IEnumerator QuitGameCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
