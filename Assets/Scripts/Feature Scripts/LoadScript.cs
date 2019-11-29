using System.IO;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScript : MonoBehaviour
{
    public GameObject ContentPanel;

    public GameObject SavePanelAsset;

    public void Populate()
    {
        SaveUtilities.EnsureSaveFolderPath();
        var potentialSaves = Directory.GetDirectories(SaveUtilities.GetSaveFolderPath());
        var saves = potentialSaves.Where(s => SaveUtilities.IsValidSave(s)).ToList();
        Debug.Log($"Got {saves.Count} saves.");
        foreach (Transform child in ContentPanel.transform)
        {
            Destroy(child.gameObject);
        }
        for (int saveIndex = 0; saveIndex < saves.Count; saveIndex++)
        {
            string s = saves[saveIndex];
            var panel = Instantiate(SavePanelAsset, ContentPanel.transform);
            panel.GetComponent<SaveItemPanelScript>().Init(new DirectoryInfo(s).Name, s);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -70 * saveIndex);
            panel.GetComponent<Button>().onClick.AddListener(() => { SetLoadGameNameFromButton(panel.GetComponent<SaveItemPanelScript>()); });
            foreach(var button in panel.GetComponentsInChildren<Button>().Skip(1))
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    private void SetLoadGameNameFromButton(SaveItemPanelScript button)
    {
        Debug.Log(button.SaveName);
        MainScript.LoadPath = button.SavePath;
        SceneManager.LoadScene("SampleScene");
    }

    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;
 
        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        return tex;
    }

    public static SaveStateStats LoadSaveState(string filePath)
    {
        return JsonUtility.FromJson<SaveStateStats>(File.ReadAllText(filePath));
    }

    public static RenderTexture FromTexture2D(Texture2D tex)
    {
        RenderTexture rTex = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGBFloat)
        {
            anisoLevel = 0,
            enableRandomWrite = true,
            autoGenerateMips = false,
            filterMode = FilterMode.Point
        };
        rTex.Create();
        RenderTexture.active = rTex;

        Graphics.Blit(tex, rTex);

        RenderTexture.active = null;
        return rTex;
    }
}
