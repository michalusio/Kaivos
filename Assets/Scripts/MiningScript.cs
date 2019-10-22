using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MainScript))]
public class MiningScript : MonoBehaviour
{
    private MainScript _mainScript;

    public Material MineTileMaterial;
    public Texture2D MineTileTexture;

    public ComputeShader MineTileShader;

    [Range(1, 5)]
    public int MineSize;
    [Range(0.1f, 1)]
    public float MineSpeed;
    public bool InstaMine;

    private (int, int)? heldTile;
    private float heldTime;

    void Start()
    {
        _mainScript = GetComponent<MainScript>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            (int, int) t = GetTileOnMouse(_mainScript);
            if (t != heldTile)
            {
                heldTile = t;
                heldTime = 0;
            }
            else
            {
                heldTime += Time.deltaTime;
                if (heldTime > MineSpeed * MineSize || InstaMine)
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
        }
    }

    public float GetMiningProgress()
    {
        return Mathf.Clamp01(heldTime / (MineSpeed * MineSize));
    }

    private void MineTile(int tX, int tY)
    {
        MineTileShader.SetInts("Position", new int[] { tX, tY });
        MineTileShader.SetTexture(0, "Result", _mainScript.mainTexturePrevFrame);
        MineTileShader.SetInts("Size", new int[] { MineSize * 2 - 1, MineSize * 2 - 1 });
        MineTileShader.Dispatch(0, MineSize * 2 - 1, MineSize * 2 - 1, 1);
    }

    public static (int, int) GetTileOnMouse(MainScript mainScript)
    {
        var screenHalfSize = new Vector2(Screen.width, Screen.height) / 2;
        var mapScale = 1 << mainScript.MAP_SCALING;
        Vector2 position = mainScript.transform.position;
        var mapScaledHalfSize = new Vector2(mainScript.mainTexturePrevFrame.width, mainScript.mainTexturePrevFrame.height) * mapScale / 2;

        var mouseP = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        var t = mouseP - screenHalfSize + mapScaledHalfSize - position * mapScale;
        t /= mapScale;
        return (Mathf.FloorToInt(t.x), Mathf.FloorToInt(t.y));
    }
}
