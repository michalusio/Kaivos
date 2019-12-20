using UnityEngine;
using UnityEngine.UI;

public class UpgradeObjectScript : MonoBehaviour
{
    public int[] Upgrade = new int[6];
    public float[] UpgradeValue = new float[6];
    public enum UpgradeType { MineS , MineA , MachineS, Jetpack, Teleporter, None };

    public UpgradeType CurrentUpgradeType = UpgradeType.None;
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;
    public Button Button5;
    private string Cost;
    public Text CostShow;

    private int currentCell = 0;
    
    private InventoryScript inventoryScript;
    private MiningScript miningScript;
    private MainScript mainScript;
    
    void Awake()
    {
        GameObject mainObject = GameObject.Find("Main Object");
        inventoryScript = mainObject.GetComponent<InventoryScript>();
        miningScript = mainObject.GetComponent<MiningScript>();
        mainScript = mainObject.GetComponent<MainScript>();

        Cost = Upgrade[0].ToString() + '$';
        
        Button2.interactable = false;
        Button3.interactable = false;
        Button4.interactable = false;
        Button5.interactable = false;
    }

    public void UpgradeButtons(Button buttonu1, Button buttonu2)
    {
        buttonu1.GetComponent<Image>().color = Color.green;
        buttonu1.interactable = false;
        buttonu1.enabled = false;
        Cost = "MAX";
        UpgradeLevel();
        currentCell += 1;
        if (Upgrade[currentCell] != 0)
        {
            buttonu2.GetComponent<Image>().color = Color.white;
            buttonu2.interactable = true;
            Cost = Upgrade[currentCell].ToString() + '$';
        }
    }

    public void ShowCost()
    {
        CostShow.text = Cost;
    }

    public void UpgradeLevel()
    {
        switch (CurrentUpgradeType)
        {
            case UpgradeType.MineS:
                miningScript.MineSpeed -= UpgradeValue[currentCell];
                break;
            case UpgradeType.MineA:
                miningScript.MineSize = (int)UpgradeValue[currentCell];
                break;
            case UpgradeType.MachineS:
                mainScript.MachineSpeed -= UpgradeValue[currentCell];
                break;
            case UpgradeType.Jetpack:

                break;
            case UpgradeType.Teleporter:

                break;
        }
    }
    public void BuyUpgrade()
    {
        if (inventoryScript.Money.AddAmount(-Upgrade[currentCell]))
        {
            switch (currentCell)
            {
                case 0:
                    UpgradeButtons(Button1, Button2);
                    break;
                case 1:
                    UpgradeButtons(Button2, Button3);
                    break;
                case 2:
                    UpgradeButtons(Button3, Button4);
                    break;
                case 3:
                    UpgradeButtons(Button4, Button5);
                    break;
                default:
                    UpgradeButtons(Button5, Button5);
                    break;
            }
        }
            
    }
}
