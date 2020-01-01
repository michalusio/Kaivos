using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MainScript), typeof(InventoryScript))]
public class MiningScript : MonoBehaviour
{
    public Material MineTileMaterial;
    public Texture2D MineTileTexture;

    public ComputeShader MineTileShader;

    [Range(1, 5)]
    public int MineSize;
    [Range(0.1f, 0.5f)]
    public float MineSpeed;
    public bool InstaMine;

    private (int, int)? heldTile;
    private float heldTime;
    public int ChosenMineSize { get; private set; }

    void Start()
    {
        ChosenMineSize = 1;
        ClassManager.MiningScript = this;
    }

    void Update()
    {
        ChosenMineSize = Mathf.Max(1, Mathf.Min(MineSize, ChosenMineSize + Mathf.RoundToInt(Input.mouseScrollDelta.y)));
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            (int, int) t = GetTileOnMouse();
            if (t != heldTile)
            {
                heldTile = t;
                heldTime = 0;
            }
            else
            {
                ClassManager.SoundScript.MiningSound(InstaMine ? 2.5f : (0.25f / (ChosenMineSize * MineSpeed)));
                heldTime += Time.deltaTime;
                if (heldTime > MineSpeed * ChosenMineSize || InstaMine)
                {
                    MineTile(heldTile.Value.Item1, heldTile.Value.Item2);
                    heldTime = 0;
                }
            }
        }
        else
        {
            heldTile = null;
            heldTime = 0;
            ClassManager.SoundScript.StopMining();
        }
    }

    public float GetMiningProgress()
    {
        return Mathf.Clamp01(heldTime / (MineSpeed * ChosenMineSize));
    }

    private void MineTile(int tX, int tY)
    {
        var tiles = ClassManager.MapReadService.GetFromTexture(new Vector2(tX + 0.5f, tY + 0.5f), new Vector2Int(ChosenMineSize * 2 - 1, ChosenMineSize * 2 - 1));
        foreach(var tile in tiles)
        {
            var find = PlacableIds.FirstOrDefault(x => MapReadService.ColorNear(x.Item1, tile));
            ClassManager.InventoryScript.BlockAmounts[find.Item2] += find.Item3;
        }
        MineTileShader.SetInts("Position", new int[] { tX, tY });
        MineTileShader.SetTexture(0, "Result", ClassManager.MainScript.mainTexturePrevFrame);
        MineTileShader.SetInts("Size", new int[] { ChosenMineSize * 2 - 1, ChosenMineSize * 2 - 1 });
        MineTileShader.Dispatch(0, ChosenMineSize * 2 - 1, ChosenMineSize * 2 - 1, 1);
    }

    public static (int, int) GetTileOnMouse()
    {
        var screenHalfSize = new Vector2(Screen.width, Screen.height) / 2;
        var mapScale = 1 << ClassManager.MainScript.MAP_SCALING;
        Vector2 position = ClassManager.MainScript.transform.position;
        var mapScaledHalfSize = new Vector2(ClassManager.MainScript.mainTexturePrevFrame.width, ClassManager.MainScript.mainTexturePrevFrame.height) * mapScale / 2;

        var mouseP = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        var t = mouseP - screenHalfSize + mapScaledHalfSize - position * mapScale;
        t /= mapScale;
        return (Mathf.FloorToInt(t.x), Mathf.FloorToInt(t.y));
    }

    private static readonly List<(Color, int, int)> PlacableIds = new List<(Color, int, int)>
    {
        (new Color(0.1f, 0.6f, 0.6f, 1.0f), 0, 1),
        (new Color(0f, 0.6f, 0.6f, 1.0f), 1, 1),
        (new Color(0.2f, 0.6f, 0.6f, 1.0f), 2, 1),
        (new Color(0f, 0.3f, 0.3f, 1.0f), 3, 1),
        (new Color(0.1f, 0.3f, 0.3f, 1.0f), 4, 1),

        (new Color(4 / 9.0f, 0.2f, 0.1f, 1.0f), 9, 1),
        (new Color(4 / 9.0f, 0.3f, 0.1f, 1.0f), 10, 1),



        (new Color(0.0f, 0.5f, 0.2f, 1.0f), 3, 10)
    };
}
