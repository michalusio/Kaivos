using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveScript : MonoBehaviour
{
    public MainScript mainScript;
    public InventoryScript inventoryScript;

    public List<string> Saves;

    void Start()
    {
        Saves = new List<string>();
    }
    
    public void Populate()
    {
        EnsureSaveFolderPath();
        var potentialSaves = Directory.GetDirectories(GetSaveFolderPath());
        var saves = potentialSaves.Where(s => IsValidSave(s)).ToList();
        Debug.Log($"Got {saves.Count} saves.");
        Saves = saves;
        SaveGame("save01");
    }

    private void SaveGame(string saveName)
    {
        var savePath = Path.Combine(GetSaveFolderPath(), saveName + "/");
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
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games/Kaivos/");
    }
}
