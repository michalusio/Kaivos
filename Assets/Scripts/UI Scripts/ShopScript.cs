using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ShopScript : MonoBehaviour
{
    public int BeltCost = 2;
    public int UpBeltCost = 2;
    public int LadderCost = 5;
    public int TorchCost = 10;

    public Text RightBeltCostText;
    public Text LeftBeltCostText;
    public Text UpBeltCostText;
    public Text LadderCostText;
    public Text TorchCostText;
    private InventoryScript inventoryScript;
    

    void Awake ()
    {
        GameObject mainObject = GameObject.Find("Main Object");
        inventoryScript = mainObject.GetComponent<InventoryScript>();

        RightBeltCostText.text = BeltCost.ToString() + '$';
        LeftBeltCostText.text = BeltCost.ToString() + '$';
        UpBeltCostText.text = UpBeltCost.ToString() + '$';
        LadderCostText.text = LadderCost.ToString() + '$';
        TorchCostText.text = TorchCost.ToString() + '$';
    }

    public void BuyRBelt()
    {
        string nRB = EventSystem.current.currentSelectedGameObject.name.Substring(5);
        int numberRB = int.Parse(nRB);
        int wholeCost = BeltCost * numberRB;
        if (inventoryScript.Money.AddAmount(-wholeCost))
        {
            inventoryScript.BlockAmounts[0] += numberRB;
            Debug.Log("Got RBelt " + numberRB);
        }
    }

    public void BuyLBelt()
    {
        string nLB = EventSystem.current.currentSelectedGameObject.name.Substring(5);
        int numberLB = int.Parse(nLB);
        int wholeCost = BeltCost * numberLB;
        if (inventoryScript.Money.AddAmount(-wholeCost))
        {
            inventoryScript.BlockAmounts[1] += numberLB;
            Debug.Log("Got LBelt " + numberLB);
        }
    }

    public void BuyUBelt()
    {
        string nU = EventSystem.current.currentSelectedGameObject.name.Substring(5);
        int numberU = int.Parse(nU);
        int wholeCost = UpBeltCost * numberU;
        if (inventoryScript.Money.AddAmount(-wholeCost))
        {
            inventoryScript.BlockAmounts[2] += numberU;
            Debug.Log("Got UBelt " + numberU);
        }
    }

    public void BuyLadder()
    {
        string nL = EventSystem.current.currentSelectedGameObject.name.Substring(4);
        int numberL = int.Parse(nL);
        int wholeCost = LadderCost * numberL;
        if (inventoryScript.Money.AddAmount(-wholeCost))
        {
            inventoryScript.BlockAmounts[3] += numberL;
            Debug.Log("Got Ladder " + numberL);
        }
    }

    public void BuyTorch()
    {
        string nT = EventSystem.current.currentSelectedGameObject.name.Substring(4);
        int numberT = int.Parse(nT);
        int wholeCost = TorchCost * numberT;
        if (inventoryScript.Money.AddAmount(-wholeCost))
        {
            inventoryScript.BlockAmounts[4] += numberT;
            Debug.Log("Got Torch " + numberT);
        }
    }
}
