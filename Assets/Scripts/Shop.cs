using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    public int BCost = 2;
    public int BUCOst = 2;
    public int LCost = 5;
    public int TCost = 10;

    public Text rbcost;
    public Text lbcost;
    public Text ubcost;
    public Text lcost;
    public Text tcost;
    public Button[] ShopButtons;
    private InventoryScript inventoryscript;
    

    void Awake ()
    {
        GameObject MainO = GameObject.Find("Main Object");
        inventoryscript = MainO.GetComponent<InventoryScript>();

        rbcost.text = BCost.ToString()+'$';
        lbcost.text = BCost.ToString()+'$';
        ubcost.text = BUCOst.ToString()+'$';
        lcost.text = LCost.ToString()+'$';
        tcost.text = TCost.ToString()+'$';
        
    }
    public bool Check_money(int price, int number)
    {
        if (price*number<= inventoryscript.Money)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BuyRBelt(){
        string nRB =  EventSystem.current.currentSelectedGameObject.name;
        nRB = nRB.Substring(5); 
        int numberRB = int.Parse(nRB);
        if (Check_money(BCost,numberRB) == true) {
            inventoryscript.Money = inventoryscript.Money - (BCost*numberRB);
            inventoryscript.BlockAmounts[0] += numberRB;
            Debug.Log("Got RBelt " + numberRB);
        }
    }
    public void BuyLBelt()
    {
        string nLB =  EventSystem.current.currentSelectedGameObject.name;
        nLB = nLB.Substring(5); 
        int numberLB = int.Parse(nLB);
        if (Check_money(BCost,numberLB) == true) {
            inventoryscript.Money = inventoryscript.Money - (BCost*numberLB);
            inventoryscript.BlockAmounts[1] += numberLB;
            Debug.Log("Got LBelt " + numberLB);
        }
    }
    public void BuyUBelt(){
        string nU =  EventSystem.current.currentSelectedGameObject.name;
        nU = nU.Substring(5); 
        int numberU = int.Parse(nU);
        if (Check_money(BCost,numberU) == true) {
            inventoryscript.Money = inventoryscript.Money - (BCost*numberU);
            inventoryscript.BlockAmounts[2] += numberU;
            Debug.Log("Got UBelt " + numberU);
        }
    }
    public void BuyLadder(){
        string nL =  EventSystem.current.currentSelectedGameObject.name;
        nL = nL.Substring(4); 
        int numberL = int.Parse(nL);
        if (Check_money(BCost,numberL) == true) {
            inventoryscript.Money = inventoryscript.Money - (BCost*numberL);
            inventoryscript.BlockAmounts[3] += numberL;
            Debug.Log("Got Ladder " + numberL);
        }
    }
    public void BuyTorch(){
        string nT =  EventSystem.current.currentSelectedGameObject.name;
        nT = nT.Substring(4); 
        int numberT = int.Parse(nT);
        if (Check_money(BCost,numberT) == true) {
            inventoryscript.Money = inventoryscript.Money - (BCost*numberT);
            inventoryscript.BlockAmounts[4] += numberT;
            Debug.Log("Got Torch " + numberT);
        }
    }
}
