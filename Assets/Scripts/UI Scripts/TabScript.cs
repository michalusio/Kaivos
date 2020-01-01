using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class TabScript : OrderedMonoBehaviour
{
    public Button[] Buttons;
    public GameObject[] Panels;

    public bool MainPanel;

    public int PanelID { get; private set; }

    protected override int Order => MainPanel ? 0 : 2;

    public void SetActivePanel(int panelID)
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
        PanelID = panelID;
    }

    protected override void Initialize()
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
        if (MainPanel)
        {
            ClassManager.InventoryPanel = this;
        }
    }

    protected override void UpdateAction()
    {
        if (MainPanel)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SetActivePanel((PanelID + 1) % Panels.Length);
            }
        }
    }
}
