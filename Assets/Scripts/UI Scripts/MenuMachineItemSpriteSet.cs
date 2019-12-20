using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MachineInfo = System.ValueTuple<int, int, int, int>;

[ExecuteAlways]
public class MenuMachineItemSpriteSet : MonoBehaviour
{
    public MachineType SetTo;

    public enum MachineType
    {
        SHOP,
        FORGE,
        JUNCTION
    }

    private static readonly Dictionary<MachineType, MachineInfo> MachineTypeParams = new Dictionary<MachineType, MachineInfo>
    {
        {MachineType.SHOP, (0, 12, 15, 5)},
        {MachineType.JUNCTION, (20, 12, 12, 8)},
        {MachineType.FORGE, (32, 12, 12, 5)},
    };

    void Start()
    {
        var drawingScript = FindObjectOfType<DrawingScript>();
        var image = GetComponent<RawImage>();
        var tileTexture = drawingScript.TileSetMapMaterial.GetTexture("_MachineTex");
        var machineParams = MachineTypeParams[SetTo];
        image.uvRect = new Rect(0, (tileTexture.height - machineParams.Item3 - (float)machineParams.Item1)/tileTexture.height, machineParams.Item2/(float)tileTexture.width, machineParams.Item3/(float)tileTexture.height);
        
        var material = Instantiate(image.material);
        material.SetInt("_AnimationSize", machineParams.Item4);
        image.material = material;
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            Start();
        }
    }
}

