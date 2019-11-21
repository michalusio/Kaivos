using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopSellScript : MonoBehaviour
{
    public ComputeShader ShopSellShader;

    private MainScript mainScript;
    private InventoryScript inventoryScript;

    private Texture2D sellTexture;

    void Start()
    {
        mainScript = GetComponent<MainScript>();
        inventoryScript = GetComponent<InventoryScript>();
        InvokeRepeating("SellItemsInShopArea", 1f, 1f);

        sellTexture = new Texture2D(8, 8, TextureFormat.RGBAFloat, false, false);
        sellTexture.Apply();
    }

    void SellItemsInShopArea()
    {
        Color[] items = GetSoldItemsInShopArea();
        ClearItemsInShopArea();
        int price = items.Sum(i => Pricing.FirstOrDefault(p => ColorNear(i, p.Item1)).Item2);
        inventoryScript.Money.AddAmount(price);
    }

    private Color[] GetSoldItemsInShopArea()
    {
        var rectReadTexture = new Rect(new Vector2Int(499, mainScript.mainTexture.height - 1008), new Vector2(8, 8));
        
        RenderTexture.active = mainScript.mainTexturePrevFrame;
        
        sellTexture.ReadPixels(rectReadTexture, 0, 0);

        var pixels = sellTexture.GetPixels();
        GL.Flush();
        RenderTexture.active = null;

        return pixels;
    }

    private void ClearItemsInShopArea()
    {
        ShopSellShader.SetTexture(0, "NewFrame", mainScript.mainTexturePrevFrame);
        ShopSellShader.Dispatch(0, 1, 1, 1);
    }

    private static bool ColorNear(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b) + Mathf.Abs(a.a - b.a) < 0.01f;
    }

    private static readonly List<(Color, int)> Pricing = new List<(Color, int)>
    {
        (new Color(0, 0.5f, 0.5f, 1.0f), 10),
        (new Color(0.1f, 0.4f, 0.4f, 1.0f), 20),
        (new Color(0.2f, 0.4f, 0.4f, 1.0f), 15),
        (new Color(0.3f, 0.4f, 0.4f, 1.0f), 30),
    };
}
