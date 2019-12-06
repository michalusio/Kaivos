using System.IO;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class InventoryScript : MonoBehaviour
{
    public TabScript InventoryTab;

    public ComputeShader PlaceTileShader;
    public Text MoneyText;

    [SerializeField] public RawImageMult[] BlockSlots;
    [SerializeField] public TextMult[] BlockSlotAmounts;
    
    public MoneyController Money { get; private set; } = 1000;

    private (int Tab, int Slot) chosenSlot;
    public (int Tab, int Slot) ChosenSlot
    {
        get
        {
            return chosenSlot;
        }
        set
        {
            foreach(var tab in BlockSlots)
            {
                foreach(var img in tab)
                {
                    img.color = Color.gray;
                }
            }
            chosenSlot = value;
            if (value.Tab >= 0 && BlockSlots.Length > value.Tab && BlockSlots[value.Tab].Length > value.Slot && BlockSlots[value.Tab][value.Slot].enabled)
            {
                BlockSlots[value.Tab][value.Slot].color = Color.white;
            }
        }
    }

    private readonly KeyCode[] blockCodes = new[]
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9
    };

    public int[] BlockAmounts;
    public bool InfiniteInventory;

    private MainScript mainScript;
    private ComputeBuffer wasTilePlaced;

    void Start()
    {
        ChosenSlot = (0, 0);
        BlockAmounts = new int[BlockSlotAmounts.Sum(tab => tab.Length)];
        mainScript = FindObjectOfType<MainScript>();
        wasTilePlaced = new ComputeBuffer(1, 4, ComputeBufferType.Default);
        PlaceTileShader.SetBuffer(0, "WasPlaced", wasTilePlaced);

        if (MainScript.LoadPath != null)
        {
            var saveState = LoadScript.LoadSaveState(Path.Combine(MainScript.LoadPath, "stats.json"));
            Money = saveState.Money;
            for(int i = 0; i < Mathf.Min(BlockAmounts.Length, saveState.BlockAmounts.Length); i++)
            {
                BlockAmounts[i] = saveState.BlockAmounts[i];
            }
        }
    }

    private void UpdateBlockAmounts()
    {
        int tab = 0;
        int slot = 0;
        for (int i = 0; i < BlockAmounts.Length; i++)
        {
            BlockSlotAmounts[tab][slot].text = BlockAmounts[i].ToString();
            slot++;
            if (BlockSlotAmounts[tab].Length <= slot)
            {
                slot = 0;
                tab++;
            }
        }
    }

    void Update()
    {
        MoneyText.text = Money.ToString();
        UpdateBlockAmounts();

        for(int i = 0; i < blockCodes.Length; i++)
        {
            if (Input.GetKeyUp(blockCodes[i]))
            {
                if (ChosenSlot == (InventoryTab.PanelID, i))
                {
                    ChosenSlot = (-1, 0);
                }
                else
                {
                    ChosenSlot = (InventoryTab.PanelID, i);
                }
            }
        }
        
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject() && ChosenSlot.Tab >= 0)
        {
            int index = GetBlockIndex();
            if (BlockAmounts[index] > 0 || InfiniteInventory)
            {
                var mousePosition = MiningScript.GetTileOnMouse(mainScript);

                PlaceTileShader.SetTexture(0, "Result", mainScript.mainTexturePrevFrame);
                PlaceTileShader.SetInt("TileToPlace", index);
                PlaceTileShader.SetInts("WhereToPlace", new []{ mousePosition.Item1, mainScript.mainTexturePrevFrame.height - 1 - mousePosition.Item2 });
                PlaceTileShader.Dispatch(0, 1, 1, 1);
                var wasPlaced = new int[1];
                wasTilePlaced.GetData(wasPlaced);
                if (wasPlaced[0] != 0)
                {
                    BlockAmounts[index] = Mathf.Max(0, BlockAmounts[index] - 1);
                }
            }
        }
    }

    private int GetBlockIndex()
    {
        int tab = 0;
        int slot = 0;
        int index = 0;
        while (true)
        {
            if (ChosenSlot.Tab == tab && ChosenSlot.Slot == slot) return index;
            index++;
            slot++;
            if (BlockSlotAmounts[tab].Length <= slot)
            {
                slot = 0;
                tab++;
            }
        }
    }

    void OnDestroy()
    {
        wasTilePlaced?.Release();
    }

    public SaveStateStats GetSaveState()
    {
        return new SaveStateStats
        {
            Money = Money.GetAmount(),
            BlockAmounts = BlockAmounts
        };
    }
}
