using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SaveScript : MonoBehaviour
{
    public MainScript mainScript;
    public InventoryScript inventoryScript;
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
        EnsureSaveFolderPath();
        var potentialSaves = Directory.GetDirectories(GetSaveFolderPath());
        var saves = potentialSaves.Where(s => IsValidSave(s)).ToList();
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

    public void SetSaveGameNameFromButton(SaveItemPanelScript button)
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
        if (!IsValidName(saveName))
        {
            Debug.Log($"Error saving {saveName}");
            return;
        }
        var savePath = Path.Combine(GetSaveFolderPath(), saveName + Path.DirectorySeparatorChar);
        Directory.CreateDirectory(savePath);
        var mapPath = Path.Combine(savePath, "map.png");
        var statPath = Path.Combine(savePath, "stats.json");

        byte[] bytes = ToTexture2D(mainScript.mainTexture).EncodeToPNG();
        File.WriteAllBytes(mapPath, bytes);

        string json = JsonUtility.ToJson(inventoryScript.GetSaveState(), true);
        File.WriteAllText(statPath, json);

        Texture2D ToTexture2D(RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.ARGB32, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }

        Debug.Log($"Game {saveName} saved!");
    }


    private readonly Regex validSaveNameRegex = new Regex(@"(-|\w|\s|\.|\(|\))+", RegexOptions.Compiled);
    private bool IsValidName(string saveName)
    {
        return validSaveNameRegex.IsMatch(saveName);
    }

    private void EnsureSaveFolderPath()
    {
        Directory.CreateDirectory(GetSaveFolderPath());
    }

    private bool IsValidSave(string savePath)
    {
        var filesInFolder = Directory.GetFiles(savePath);
        return filesInFolder.Length == 2 && filesInFolder.Any(file => Path.GetFileName(file) == "map.png") && filesInFolder.Any(file => Path.GetFileName(file) == "stats.json");
    }

    private string GetSaveFolderPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"My Games{Path.DirectorySeparatorChar}Kaivos{Path.DirectorySeparatorChar}");
    }
}
