using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class CheatScript : MonoBehaviour
{
    public GameObject[] DebugObjects;

    private GameObject canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        foreach(var d in DebugObjects)
        {
            d.SetActive(Debug.isDebugBuild);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            canvas.SetActive(!canvas.activeSelf);
        }
    }

    public void ToggleInstaMine(Toggle change)
    {
        ClassManager.MiningScript.InstaMine = change.isOn;
    }

    public void ToggleMaxView(Toggle change)
    {
        var mainScript = FindObjectOfType<MainScript>();
        if (change.isOn)
        {
            mainScript.MAP_SCALING = 1;
        }
        else
        {
            mainScript.MAP_SCALING = 4;
        }
    }

    public void ToggleInfiniteInventory(Toggle change)
    {
        ClassManager.InventoryScript.InfiniteInventory = change.isOn;
    }

    public void ToggleTileSet(Toggle change)
    {
        ClassManager.MainScript.TileSetOn = !change.isOn;
    }
}