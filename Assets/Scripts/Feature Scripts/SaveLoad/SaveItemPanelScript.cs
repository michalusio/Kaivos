using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveItemPanelScript : MonoBehaviour
{
    public Text SaveNameText;
    public Text SavePathText;

    public string SaveName { get; private set; }
    public string SavePath { get; private set; }
    
    public void Init(string name, string path)
    {
        SaveName = name;
        SavePath = path;
        SaveNameText.text = ' ' + name;
        SavePathText.text = ' ' + path;
    }

    public void DeleteSave()
    {
        Directory.Delete(SavePath, true);
        Destroy(gameObject);
    }
}
