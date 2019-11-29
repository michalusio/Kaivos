using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryScript : MonoBehaviour
{
    public ComputeShader PlaceTileShader;
    public Text MoneyText;
    public RawImage[] BlockSlots;
    public Text[] BlockSlotAmounts;
    
    public readonly MoneyController Money = 1000;

    private int chosenSlot;
    public int ChosenSlot
    {
        get
        {
            return chosenSlot;
        }
        set
        {
            if (BlockSlots != null && BlockSlots.Length > value)
            {
                if (BlockSlots[value].enabled)
                {
                    foreach(var img in BlockSlots)
                    {
                        img.color = Color.gray;
                    }
                    BlockSlots[value].color = Color.white;
                    chosenSlot = value;
                }
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
        ChosenSlot = 0;
        BlockAmounts = new int[BlockSlotAmounts.Length];
        mainScript = FindObjectOfType<MainScript>();
        wasTilePlaced = new ComputeBuffer(1, 4, ComputeBufferType.Default);
        PlaceTileShader.SetBuffer(0, "WasPlaced", wasTilePlaced);
    }

    private void UpdateBlockAmounts()
    {
        for(int i = 0; i < BlockSlotAmounts.Length; i++)
        {
            BlockSlotAmounts[i].text = BlockAmounts[i].ToString();
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
                ChosenSlot = i;
            }
        }
        
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (BlockAmounts[ChosenSlot] > 0 || InfiniteInventory)
            {
                var mousePosition = MiningScript.GetTileOnMouse(mainScript);

                PlaceTileShader.SetTexture(0, "Result", mainScript.mainTexturePrevFrame);
                PlaceTileShader.SetInt("TileToPlace", ChosenSlot);
                PlaceTileShader.SetInts("WhereToPlace", new []{ mousePosition.Item1, mainScript.mainTexturePrevFrame.height - 1 - mousePosition.Item2});
                PlaceTileShader.Dispatch(0, 1, 1, 1);
                var wasPlaced = new int[1];
                wasTilePlaced.GetData(wasPlaced);
                if (wasPlaced[0] != 0)
                {
                    BlockAmounts[ChosenSlot] = Mathf.Max(0, BlockAmounts[ChosenSlot] - 1);
                }
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
