using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(MainScript), typeof(CharacterMovementScript), typeof(MiningScript))]
public class DrawingScript : OrderedMonoBehaviour
{
    public Material TileSetMapMaterial;

    protected override int Order => 2;

    private Camera _guiCamera;
    private SlotCursorParams _slotCursor;

    private (int, int) prevInvSlot;

    protected override void Initialize()
    {
        _guiCamera = GameObject.Find("GuiCamera").GetComponent<Camera>();
        var tileTexture = TileSetMapMaterial.GetTexture("_TileTex");
        var machineTexture = TileSetMapMaterial.GetTexture("_MachineTex");
        TileSetMapMaterial.SetVector("_Sizes", new Vector4(ClassManager.MainScript.mainTexture.width, ClassManager.MainScript.mainTexture.height, tileTexture.width, tileTexture.height));
        TileSetMapMaterial.SetVector("_Sizes2", new Vector4(machineTexture.width, machineTexture.height, 0, 0));

        _slotCursor = new SlotCursorParams();
        UpdateInventorySlot();

        ClassManager.CharacterMovementScript.MinerAnimationMaterial.SetTexture("_ShadowTex", ClassManager.MainScript.shadowTexture);

        ClassManager.DrawingScript = this;
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.Repaint && Initialized)
        {
            var screenHalfSize = new Vector2(Screen.width, Screen.height) / 2f;
            var mapScale = 1 << ClassManager.MainScript.MAP_SCALING;
            Vector2 position = transform.position;
            var mapScaledHalfSize = new Vector2(ClassManager.MainScript.mainTexturePrevFrame.width, ClassManager.MainScript.mainTexturePrevFrame.height) * mapScale / 2f;
            
            TileSetMapMaterial.SetTexture("_ShadowTex", ClassManager.MainScript.shadowTexture);
            DrawMainMap(screenHalfSize, mapScale, position, mapScaledHalfSize);

            DrawCharacter(mapScale, screenHalfSize);

            DrawMouseTilePointer(screenHalfSize, mapScale, position, mapScaledHalfSize);

            DrawMouseTileGhost(screenHalfSize, mapScale, position, mapScaledHalfSize);

            DebugCollisionBoxShow();

            _guiCamera.Render();
        }
    }

    private void DrawMainMap(Vector2 screenHalfSize, int mapScale, Vector2 position, Vector2 mapScaledHalfSize)
    {
        TileSetMapMaterial.SetVector("_PlayerPos", new Vector4(transform.position.x, transform.position.y, 0, 0));
        TileSetMapMaterial.SetTexture("_ShadowTex", ClassManager.MainScript.shadowTexture);
        Graphics.DrawTexture(new Rect(screenHalfSize - mapScaledHalfSize + position * mapScale, mapScaledHalfSize * 2), ClassManager.MainScript.mainTexturePrevFrame, ClassManager.MainScript.TileSetOn ? TileSetMapMaterial : null);
    }

    private void DrawCharacter(int mapScale, Vector2 screenHalfSize)
    {
        var charHalfSize = new Vector2(2 * mapScale, 2 * mapScale);
        Graphics.DrawTexture(new Rect(screenHalfSize - charHalfSize, 2 * charHalfSize), ClassManager.CharacterMovementScript.MinerIdleTexture, ClassManager.CharacterMovementScript.MinerAnimationMaterial);
    }

    private void DrawMouseTilePointer(Vector2 screenHalfSize, int mapScale, Vector2 position, Vector2 mapScaledHalfSize)
    {
        var mouseP = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        var t = mouseP - screenHalfSize + mapScaledHalfSize - position * mapScale;
        t /= mapScale;
        t = new Vector2(Mathf.Floor(t.x), Mathf.Floor(t.y));

        var tileSize = new Vector2(mapScale, mapScale);
        ClassManager.MiningScript.MineTileMaterial.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, ClassManager.MiningScript.GetMiningProgress());
        ClassManager.MiningScript.MineTileMaterial.SetInt("_Size", ClassManager.MiningScript.ChosenMineSize * 2 - 1);
        var mineHalfSize = new Vector2(ClassManager.MiningScript.ChosenMineSize - 1, ClassManager.MiningScript.ChosenMineSize - 1);
        Graphics.DrawTexture(new Rect(t * mapScale + (position - mineHalfSize) * mapScale - mapScaledHalfSize + screenHalfSize, tileSize * (ClassManager.MiningScript.ChosenMineSize * 2 - 1)), ClassManager.MiningScript.MineTileTexture, ClassManager.MiningScript.MineTileMaterial);
    }

    private void DrawMouseTileGhost(Vector2 screenHalfSize, int mapScale, Vector2 position, Vector2 mapScaledHalfSize)
    {
        if (ClassManager.InventoryScript.ChosenSlot.Tab < 0)
        {
            return;
        }
        
        if (prevInvSlot != ClassManager.InventoryScript.ChosenSlot)
        {
            UpdateInventorySlot();
        }

        var blockSize = new Vector2(1, 1);
        if (prevInvSlot.Item1 == 1) blockSize = new Vector2(3, 3);

        var mouseP = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        var t = mouseP - screenHalfSize + mapScaledHalfSize - position * mapScale;
        t /= mapScale;
        t = new Vector2(Mathf.Floor(t.x), Mathf.Floor(t.y));

        var tileSize = new Vector2(mapScale, mapScale);
        
        Graphics.DrawTexture(new Rect(t * mapScale + (position - blockSize * 0.5f + Vector2.one * 0.5f) * mapScale - mapScaledHalfSize + screenHalfSize, tileSize * blockSize), _slotCursor.texture, _slotCursor.uvRect, 0, 0, 0, 0, _slotCursor.material);
    }

    private void UpdateInventorySlot()
    {
        prevInvSlot = ClassManager.InventoryScript.ChosenSlot;
        _slotCursor.material = Instantiate(ClassManager.InventoryScript.BlockSlots[prevInvSlot.Item1][prevInvSlot.Item2].material);
        _slotCursor.texture = ClassManager.InventoryScript.BlockSlots[prevInvSlot.Item1][prevInvSlot.Item2].mainTexture;
        _slotCursor.uvRect = ClassManager.InventoryScript.BlockSlots[prevInvSlot.Item1][prevInvSlot.Item2].uvRect;
    }

    private void DebugCollisionBoxShow()
    {
        if (ClassManager.CharacterMovementScript.ShowCollisionBox)
        {
            foreach ((var tex, var rect) in ClassManager.CharacterMovementScript.GetCollisionDebug())
            {
                Graphics.DrawTexture(rect, tex);
            }
        }
    }

    protected override void UpdateAction()
    {
    }

    class SlotCursorParams
    {
        public Material material;
        public Texture texture;
        public Rect uvRect;
    }
}