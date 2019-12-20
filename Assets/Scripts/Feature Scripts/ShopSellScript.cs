using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class ShopSellScript : MonoBehaviour
{
    public ComputeShader ShopSellShader;

    private Texture2D sellTexture;

    void Start()
    {
        InvokeRepeating("SellItemsInShopArea", 1f, 1f);

        sellTexture = new Texture2D(8, 8, TextureFormat.RGBAFloat, false, false);
        sellTexture.Apply();
    }

    void SellItemsInShopArea()
    {
        Color[] items = GetSoldItemsInShopArea();
        ClearItemsInShopArea();
        int price = items.Sum(i => Pricing.FirstOrDefault(p => MapReadService.ColorNear(i, p.Item1)).Item2);
        if (price != 0) Debug.Log($"Sold items for {price}$");
        ClassManager.InventoryScript.Money.AddAmount(price);
    }

    private Color[] GetSoldItemsInShopArea()
    {
        var rectReadTexture = new Rect(new Vector2Int(499, ClassManager.MainScript.mainTexture.height - 1008), new Vector2(8, 8));
        
        RenderTexture.active = ClassManager.MainScript.mainTexturePrevFrame;
        
        sellTexture.ReadPixels(rectReadTexture, 0, 0);

        var pixels = sellTexture.GetPixels();
        GL.Flush();
        RenderTexture.active = null;

        return pixels;
    }

    private void ClearItemsInShopArea()
    {
        ShopSellShader.SetTexture(0, "NewFrame", ClassManager.MainScript.mainTexturePrevFrame);
        ShopSellShader.Dispatch(0, 1, 1, 1);
    }

    private static readonly List<(Color, int)> Pricing = new List<(Color, int)>
    {
        (new Color(0, 0.5f, 0.5f, 1.0f), 10),
        (new Color(0.1f, 0.4f, 0.4f, 1.0f), 20),
        (new Color(0.2f, 0.4f, 0.4f, 1.0f), 15),
        (new Color(0.3f, 0.4f, 0.4f, 1.0f), 30),

        (new Color(0.0f, 0.0f, 0.9f, 1.0f), 50),
        (new Color(0.1f, 0.0f, 0.9f, 1.0f), 70),
        (new Color(0.2f, 0.0f, 0.9f, 1.0f), 110),
        (new Color(0.3f, 0.0f, 0.9f, 1.0f), 150),
    };
}
