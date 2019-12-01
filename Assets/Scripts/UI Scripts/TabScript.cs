using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabScript : MonoBehaviour
{
    public Button[] Buttons;
    public GameObject[] Panels;

    void Start()
    {
        int maxPanels = Mathf.Min(Buttons.Length, Panels.Length);
        foreach(var button in Buttons.Skip(maxPanels))
        {
            button.gameObject.SetActive(false);
        }
        for (int i = 0; i < Buttons.Length; i++)
        {
            int closureIndex = i;
            Buttons[closureIndex].onClick.AddListener(() => { SetActivePanel(closureIndex); });
        }
        SetActivePanel(0);
    }

    void SetActivePanel(int panelID)
    {
        foreach(var panel in Panels)
        {
            panel.SetActive(false);
        }
        Panels[panelID].SetActive(true);
        foreach(var button in Buttons)
        {
            button.transform.SetAsFirstSibling();
        }
        Buttons[panelID].transform.SetAsLastSibling();
    }
}
