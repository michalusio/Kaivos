using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MenuPanel;

    void Start()
    {
        QualitySettings.vSyncCount = 1;
        var panelImage = MenuPanel.GetComponent<Image>();
        panelImage.material.SetVector("_Sizes", new Vector4(Screen.width, Screen.height, panelImage.material.mainTexture.width, panelImage.material.mainTexture.height));
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
