using UnityEngine;
using UnityEngine.UI;

public class CheatScript : MonoBehaviour
{
    public GameObject[] DebugObjects;

    private GameObject canvas;
    

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        if (Debug.isDebugBuild)
        {
            foreach(var d in DebugObjects)
            {
                d.SetActive(true);
            }
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
        FindObjectOfType<MiningScript>().InstaMine = change.isOn;
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
        FindObjectOfType<InventoryScript>().InfiniteInventory = change.isOn;
    }

    public void ToggleTileSet(Toggle change)
    {
        FindObjectOfType<MainScript>().TileSetOn = !change.isOn;
    }
}