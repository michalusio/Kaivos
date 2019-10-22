using UnityEngine;
using UnityEngine.UI;

public class CheatScript : MonoBehaviour
{
    private GameObject canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
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
}
