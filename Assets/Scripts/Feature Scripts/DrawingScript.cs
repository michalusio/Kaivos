using UnityEngine;

[RequireComponent(typeof(MainScript), typeof(CharacterMovementScript), typeof(MiningScript))]
public class DrawingScript : MonoBehaviour
{
    public Camera GuiCamera;
    public Material TileSetMapMaterial;

    private MainScript _mainScript;
    private CharacterMovementScript _characterMovementScript;
    private MiningScript _miningScript;

    void Start()
    {
        _mainScript = GetComponent<MainScript>();
        _characterMovementScript = GetComponent<CharacterMovementScript>();
        _miningScript = GetComponent<MiningScript>();
        var tileTexture = TileSetMapMaterial.GetTexture("_TileTex");
        var machineTexture = TileSetMapMaterial.GetTexture("_MachineTex");
        TileSetMapMaterial.SetVector("_Sizes", new Vector4(_mainScript.mainTexture.width, _mainScript.mainTexture.height, tileTexture.width, tileTexture.height));
        TileSetMapMaterial.SetVector("_Sizes2", new Vector4(machineTexture.width, machineTexture.height, 0, 0));
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            var screenHalfSize = new Vector2(Screen.width, Screen.height) / 2;
            var mapScale = 1 << _mainScript.MAP_SCALING;
            Vector2 position = transform.position;
            var mapScaledHalfSize = new Vector2(_mainScript.mainTexturePrevFrame.width, _mainScript.mainTexturePrevFrame.height) * mapScale / 2;
            
            TileSetMapMaterial.SetTexture("_ShadowTex", _mainScript.shadowTexture);
            DrawMainMap(screenHalfSize, mapScale, position, mapScaledHalfSize);

            DrawCharacter(mapScale, screenHalfSize);

            DrawMouseTilePointer(screenHalfSize, mapScale, position, mapScaledHalfSize);

            DebugCollisionBoxShow();

            GuiCamera.Render();
        }
    }

    private void DrawMainMap(Vector2 screenHalfSize, int mapScale, Vector2 position, Vector2 mapScaledHalfSize)
    {
        TileSetMapMaterial.SetVector("_PlayerPos", new Vector4(transform.position.x, transform.position.y, 0, 0));
        TileSetMapMaterial.SetTexture("_ShadowTex", _mainScript.shadowTexture);
        Graphics.DrawTexture(new Rect(screenHalfSize - mapScaledHalfSize + position * mapScale, mapScaledHalfSize * 2), _mainScript.mainTexturePrevFrame, _mainScript.TileSetOn ? TileSetMapMaterial : null);
    }

    private void DrawCharacter(int mapScale, Vector2 screenHalfSize)
    {
        var charHalfSize = new Vector2(2 * mapScale, 2 * mapScale);
        Graphics.DrawTexture(new Rect(screenHalfSize - charHalfSize, 2 * charHalfSize), _characterMovementScript.MinerIdleTexture, _characterMovementScript.MinerAnimationMaterial);
    }

    private void DrawMouseTilePointer(Vector2 screenHalfSize, int mapScale, Vector2 position, Vector2 mapScaledHalfSize)
    {
        var mouseP = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        var t = mouseP - screenHalfSize + mapScaledHalfSize - position * mapScale;
        t /= mapScale;
        t = new Vector2(Mathf.Floor(t.x), Mathf.Floor(t.y));

        var tileSize = new Vector2(mapScale, mapScale);
        _miningScript.MineTileMaterial.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, _miningScript.GetMiningProgress());
        _miningScript.MineTileMaterial.SetInt("_Size", _miningScript.ChosenMineSize * 2 - 1);
        var mineHalfSize = new Vector2(_miningScript.ChosenMineSize - 1, _miningScript.ChosenMineSize - 1);
        Graphics.DrawTexture(new Rect(t * mapScale + (position - mineHalfSize) * mapScale - mapScaledHalfSize + screenHalfSize, tileSize * (_miningScript.ChosenMineSize * 2 - 1)), _miningScript.MineTileTexture, _miningScript.MineTileMaterial);
    }

    private void DebugCollisionBoxShow()
    {
        if (_characterMovementScript.ShowCollisionBox)
        {
            foreach ((var tex, var rect) in _characterMovementScript.GetCollisionDebug())
            {
                Graphics.DrawTexture(rect, tex);
            }
        }
    }
}
