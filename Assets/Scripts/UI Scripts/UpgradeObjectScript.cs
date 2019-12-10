using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeObjectScript : MonoBehaviour
{
    public int[] Upgrade = new int[6];
    public float[] UpgradeValue = new float[6];
    public enum UpgradeType {MineS , MineA , MachineS, Jetpack, Teleporter, None};

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
    
    void Awake ()
    {
        GameObject mainObject = GameObject.Find("Main Object");
        inventoryScript = mainObject.GetComponent<InventoryScript>();
        miningScript = mainObject.GetComponent<MiningScript>();

        Cost = Upgrade[0].ToString() + '$';
        
        Button2.interactable = false;
        Button3.interactable = false;
        Button4.interactable = false;
        Button5.interactable = false;
    }
    public void upgradeButtons (Button buttonu1, Button buttonu2)
    {
        buttonu1.GetComponent<Image>().color=Color.green;
        buttonu1.interactable = false;
        buttonu1.enabled=false;
        Cost = "MAX";
        upgradeLevel();
        currentCell += 1;
        if (Upgrade[currentCell]!=0){
        buttonu2.GetComponent<Image>().color=Color.white;
        buttonu2.interactable = true;
        Cost = (Upgrade[currentCell]).ToString() + '$';
        }
    }
    public void ShowCost()
    {
        CostShow.text = Cost;
    }
    public void upgradeLevel ()
    {
        if (CurrentUpgradeType == UpgradeType.MineS)
        {
            miningScript.MineSpeed += UpgradeValue[currentCell];
            Debug.Log(miningScript.MineSpeed);
        }
        else if (CurrentUpgradeType == UpgradeType.MineA)
        {
            miningScript.MineSize = (int)UpgradeValue[currentCell];
            Debug.Log(miningScript.MineSize);
        }
        else if (CurrentUpgradeType == UpgradeType.MachineS)
        {
            //upgrade MachineS
        }
        else if (CurrentUpgradeType == UpgradeType.Jetpack)
        {
            //upgrade Jetpack
        }
        else if (CurrentUpgradeType == UpgradeType.Teleporter)
        {
            //upgrade Telep
        }
    }
    public void BuyUpgrade()
    {
        if (inventoryScript.Money.AddAmount(-Upgrade[currentCell]))
        {
            if(currentCell == 0)
            {
                upgradeButtons (Button1, Button2);
            }
            else if(currentCell == 1)
            {
                upgradeButtons (Button2, Button3);
            }
            else if(currentCell == 2)
            {
                upgradeButtons (Button3, Button4);
            }
            else if(currentCell == 3)
            {
                upgradeButtons (Button4, Button5);
            }
            else
            {
                upgradeButtons (Button5, Button5);
            }
        }
            
    }
}
