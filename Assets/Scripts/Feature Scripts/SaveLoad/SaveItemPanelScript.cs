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
        SavePathText.text = ' ' + Ellipsis(path);
    }

    private const int ELLIPSIS_LENGTH = 40;
    private string Ellipsis(string path)
    {
        if (path.Length < ELLIPSIS_LENGTH) return path;
        return "..." + path.Substring(path.Length - ELLIPSIS_LENGTH + 3, ELLIPSIS_LENGTH - 3);
    }

    public void DeleteSave()
    {
        Directory.Delete(SavePath, true);
        Destroy(gameObject);
    }
}
