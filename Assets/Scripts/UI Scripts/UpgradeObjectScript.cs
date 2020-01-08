using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeObjectScript : OrderedMonoBehaviour
{
    public int[] Upgrade = new int[6];
    public float[] UpgradeValue = new float[6];
    public enum UpgradeType
    {
        None = 0,
        MineS = 1,
        MineA = 2,
        MachineS = 3,
        Jetpack = 4,
        Teleporter = 5,
    };

    public UpgradeType CurrentUpgradeType = UpgradeType.None;
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;
    public Button Button5;
    private string Cost;
    public Text CostShow;

    public int CurrentCell { get; private set; } = 0;

    protected override int Order => 3;

    public void UpgradeButtons(Button buttonu1, Button buttonu2)
    {
        buttonu1.GetComponent<Image>().color = Color.green;
        buttonu1.interactable = false;
        buttonu1.enabled = false;
        Cost = "MAX";
        UpgradeLevel();
        CurrentCell += 1;
        if (Upgrade[CurrentCell] != 0)
        {
            buttonu2.GetComponent<Image>().color = Color.white;
            buttonu2.interactable = true;
            Cost = Upgrade[CurrentCell].ToString() + '$';
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
                ClassManager.MiningScript.MineSpeed -= UpgradeValue[CurrentCell];
                break;
            case UpgradeType.MineA:
                ClassManager.MiningScript.MineSize = (int)UpgradeValue[CurrentCell];
                break;
            case UpgradeType.MachineS:
                ClassManager.MainScript.MachineSpeed -= UpgradeValue[CurrentCell];
                break;
            case UpgradeType.Jetpack:

                break;
            case UpgradeType.Teleporter:

                break;
        }
    }
    
    public void BuyUpgrade()
    {
        if (ClassManager.InventoryScript.Money.AddAmount(-Upgrade[CurrentCell]))
        {
            switch (CurrentCell)
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

    private void UnlockUpgrade()
    {
        switch (CurrentCell)
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

    protected override void Initialize()
    {
        Cost = Upgrade[0].ToString() + '$';
        
        Button2.interactable = false;
        Button3.interactable = false;
        Button4.interactable = false;
        Button5.interactable = false;
    }

    public void LoadSave()
    {
        if (MainScript.LoadPath != null)
        {
            var save = ClassManager.MainScript.LoadSaveState;
            int times = save.Upgrades[(int)CurrentUpgradeType];
            Debug.Log($"Upgrading {CurrentUpgradeType} times {times}");
            for(int i = 0; i < times; i++)
            {
                UnlockUpgrade();
            }
        }
    }

    protected override void UpdateAction()
    {
    }
}
