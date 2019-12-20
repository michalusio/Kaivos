using System.IO;
using System.IO.Compression;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SaveScript : MonoBehaviour
{
    public GameObject ContentPanel;
    public InputField SaveNameInput;

    public GameObject SavePanelAsset;
    
    private int contentPanelItems;

    void Update()
    {
        if (ContentPanel.transform.childCount != contentPanelItems)
        {
            contentPanelItems = ContentPanel.transform.childCount;
            for (int saveIndex = 0; saveIndex < contentPanelItems; saveIndex++)
            {
                ContentPanel
                    .transform
                    .GetChild(saveIndex)
                    .GetComponent<RectTransform>()
                    .anchoredPosition = new Vector2(0, -70 * saveIndex);
            }
        }
    }

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
            panel.GetComponent<Button>().onClick.AddListener(() => { SetSaveGameNameFromButton(panel.GetComponent<SaveItemPanelScript>()); });
        }
    }

    private void SetSaveGameNameFromButton(SaveItemPanelScript button)
    {
        Debug.Log(button.SaveName);
        SaveNameInput.text = button.SaveName;
    }

    public void SaveGameFromInput()
    {
        SaveGame(SaveNameInput.text);
        Populate();
    }

    private void SaveGame(string saveName)
    {
        if (!SaveUtilities.IsValidName(saveName))
        {
            Debug.Log($"Error saving {saveName}");
            return;
        }
        var savePath = Path.Combine(SaveUtilities.GetSaveFolderPath(), saveName + Path.DirectorySeparatorChar);
        Directory.CreateDirectory(savePath);
        var mapPath = Path.Combine(savePath, SaveUtilities.MapFileName);
        var statPath = Path.Combine(savePath, SaveUtilities.StatFileName);

        SaveTextureToFile(ClassManager.MainScript.mainTexture, mapPath);
        File.WriteAllText(statPath, JsonUtility.ToJson(ClassManager.InventoryScript.GetSaveState(), true));
        
        Debug.Log($"Game {saveName} saved!");
    }

    private void SaveTextureToFile(RenderTexture mainTexture, string mapPath)
    {
        var mapTexture = ToTexture2D(mainTexture);
        var byteData = mapTexture.GetRawTextureData();
        using (var inputStream = File.Open(mapPath, FileMode.Create))
        using (var gzip = new GZipStream(inputStream, CompressionMode.Compress))
        {
            
            gzip.Write(byteData, 0, byteData.Length);
        }

        Texture2D ToTexture2D(RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBAFloat, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}
