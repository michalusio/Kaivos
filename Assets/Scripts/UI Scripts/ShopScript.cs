using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopScript : MonoBehaviour
{
    readonly Dictionary<ShopItemType, int> ShopItemCost = new Dictionary<ShopItemType, int>
    {
        {ShopItemType.RIGHT_BELT, 2},
        {ShopItemType.LEFT_BELT, 2},
        {ShopItemType.UP_BELT, 2},
        {ShopItemType.LADDER, 5},
        {ShopItemType.TORCH, 10},
        {ShopItemType.JUNCTION, 100},
        {ShopItemType.FORGE, 1000}
    };
    readonly Dictionary<ShopItemType, int> ShopItemInvAmounts = new Dictionary<ShopItemType, int>
    {
        {ShopItemType.RIGHT_BELT, 0},
        {ShopItemType.LEFT_BELT, 1},
        {ShopItemType.UP_BELT, 2},
        {ShopItemType.LADDER, 3},
        {ShopItemType.TORCH, 4},
        {ShopItemType.JUNCTION, 9},
        {ShopItemType.FORGE, 10}
    };

    public Text RightBeltCostText;
    public Text LeftBeltCostText;
    public Text UpBeltCostText;
    public Text LadderCostText;
    public Text TorchCostText;
    public Text JunctionCostText;
    public Text ForgeCostText;
    private InventoryScript _inventoryScript;
    

    void Awake()
    {
        GameObject mainObject = GameObject.Find("Main Object");
        _inventoryScript = mainObject.GetComponent<InventoryScript>();

        RightBeltCostText.text = ShopItemCost[ShopItemType.RIGHT_BELT].ToString() + '$';
        LeftBeltCostText.text = ShopItemCost[ShopItemType.LEFT_BELT].ToString() + '$';
        UpBeltCostText.text = ShopItemCost[ShopItemType.UP_BELT].ToString() + '$';
        LadderCostText.text = ShopItemCost[ShopItemType.LADDER].ToString() + '$';
        TorchCostText.text = ShopItemCost[ShopItemType.TORCH].ToString() + '$';
        JunctionCostText.text = ShopItemCost[ShopItemType.JUNCTION].ToString() + '$';
        ForgeCostText.text = ShopItemCost[ShopItemType.FORGE].ToString() + '$';
    }

    void BuyObject(ShopItemType type, int amount)
    {
        int wholeCost = ShopItemCost[type] * amount;
        if (_inventoryScript.Money.AddAmount(-wholeCost))
        {
            _inventoryScript.BlockAmounts[ShopItemInvAmounts[type]] += amount;
            Debug.Log($"Bought {amount} of {type} for {wholeCost}");
        }
    }

    public void BuyRightBelt(int amount) => BuyObject(ShopItemType.RIGHT_BELT, amount);
    public void BuyLeftBelt(int amount) => BuyObject(ShopItemType.LEFT_BELT, amount);
    public void BuyUpBelt(int amount) => BuyObject(ShopItemType.UP_BELT, amount);
    public void BuyLadder(int amount) => BuyObject(ShopItemType.LADDER, amount);
    public void BuyTorch(int amount) => BuyObject(ShopItemType.TORCH, amount);
    public void BuyJunction(int amount) => BuyObject(ShopItemType.JUNCTION, amount);
    public void BuyForge(int amount) => BuyObject(ShopItemType.FORGE, amount);
}

public enum ShopItemType
{
    RIGHT_BELT,
    LEFT_BELT,
    UP_BELT,
    LADDER,
    TORCH,
    JUNCTION,
    FORGE

}