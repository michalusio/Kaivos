using System.IO;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public static string LoadPath = null;

    [Range(1, 6)]
    public int MAP_SCALING = 4;

    public bool TileSetOn = true;

    public ComputeShader ParticleShader;
    public ComputeShader BeltShader;
    public ComputeShader PlaceShader;
    public ComputeShader ShadowShader;

    public RenderTexture mainTexture, mainTexturePrevFrame, shadowTexture;
    
    private const int MAP_SIZE = 1024;

    private float timeSinceLastPhysics;

    void Start()
    {
        if (LoadPath == null)
        {
            mainTexture = new RenderTexture(MAP_SIZE, MAP_SIZE, 0, RenderTextureFormat.ARGBFloat)
            {
                anisoLevel = 0,
                enableRandomWrite = true,
                autoGenerateMips = false,
                filterMode = FilterMode.Point
            };
            mainTexture.Create();

            mainTexturePrevFrame = new RenderTexture(mainTexture.width, mainTexture.height, 0, RenderTextureFormat.ARGBFloat)
            {
                anisoLevel = 0,
                enableRandomWrite = true,
                autoGenerateMips = false,
                filterMode = FilterMode.Point
            };
            mainTexturePrevFrame.Create();
        }
        else
        {
            var loadTexture = LoadScript.LoadPNG(Path.Combine(LoadPath, "map.png"));
            mainTexture = LoadScript.FromTexture2D(loadTexture);
            mainTexturePrevFrame = LoadScript.FromTexture2D(loadTexture);
        }

        shadowTexture = new RenderTexture(mainTexture.width, mainTexture.height, 0, RenderTextureFormat.ARGBFloat)
        {
            anisoLevel = 0,
            enableRandomWrite = true,
            autoGenerateMips = false,
            filterMode = FilterMode.Bilinear
        };
        shadowTexture.Create();

        int seed = Random.Range(0, 10000);
        PlaceShader.SetInt("seed", seed);
        ParticleShader.SetInt("seed", seed);
        BeltShader.SetInt("seed", seed);

        SetupShaderTextures();

        if (LoadPath == null)
        {
            GenerateMap();
        }

        ShadowShader.Dispatch(1, 1, 1, 1);
    }

    private void SetupShaderTextures()
    {
        ParticleShader.SetTexture(0, "NewFrame", mainTexture);
        ParticleShader.SetTexture(0, "FrameBefore", mainTexturePrevFrame);
        ParticleShader.SetTexture(1, "NewFrame", mainTexture);
        ParticleShader.SetTexture(1, "FrameBefore", mainTexturePrevFrame);
        ParticleShader.SetTexture(2, "NewFrame", mainTexture);
        ParticleShader.SetTexture(2, "FrameBefore", mainTexturePrevFrame);
        BeltShader.SetTexture(0, "NewFrame", mainTexture);
        BeltShader.SetTexture(0, "FrameBefore", mainTexturePrevFrame);
        BeltShader.SetTexture(1, "NewFrame", mainTexture);
        BeltShader.SetTexture(1, "FrameBefore", mainTexturePrevFrame);
        BeltShader.SetTexture(2, "NewFrame", mainTexture);
        BeltShader.SetTexture(2, "FrameBefore", mainTexturePrevFrame);
        ShadowShader.SetTexture(0, "Frame", mainTexture);
        ShadowShader.SetTexture(0, "Result", shadowTexture);
        ShadowShader.SetTexture(1, "Frame", mainTexture);
        ShadowShader.SetTexture(1, "Result", shadowTexture);
    }

    private void GenerateMap()
    {
        PlaceShader.SetTexture(0, "NewFrame", mainTexturePrevFrame);

        PlaceShader.SetTexture(1, "NewFrame", mainTexturePrevFrame);
        PlaceShader.SetTexture(1, "FrameBefore", mainTexture);
        PlaceShader.SetTexture(2, "NewFrame", mainTexturePrevFrame);
        PlaceShader.SetTexture(2, "FrameBefore", mainTexture);

        PlaceShader.Dispatch(0, mainTexture.width / 16, mainTexture.height / 16, 1);
        PlaceShader.Dispatch(1, 1, 1, 1);
        PlaceShader.Dispatch(2, 1, 1, 1);
    }

    void Update()
    {
        timeSinceLastPhysics += Time.deltaTime;
        while (timeSinceLastPhysics > 0.1f)
        {
            timeSinceLastPhysics -= 0.1f;
            
            SetupUpdateShaders();
            DispatchUpdateShaders();
            SwitchTextures();
        }

        ShadowShader.SetFloat("Time", Time.realtimeSinceStartup);
        ShadowShader.SetTexture(0, "Frame", mainTexturePrevFrame);
        ShadowShader.Dispatch(0, mainTexture.width / 16, mainTexture.height / 16, 1);
    }

    private void SetupUpdateShaders()
    {
        ParticleShader.SetFloat("Time", Time.realtimeSinceStartup);
        ParticleShader.SetTexture(0, "NewFrame", mainTexture);
        ParticleShader.SetTexture(0, "FrameBefore", mainTexturePrevFrame);
        ParticleShader.SetTexture(1, "NewFrame", mainTexture);
        ParticleShader.SetTexture(1, "FrameBefore", mainTexturePrevFrame);
        ParticleShader.SetTexture(2, "NewFrame", mainTexture);
        ParticleShader.SetTexture(2, "FrameBefore", mainTexturePrevFrame);

        BeltShader.SetFloat("Time", Time.realtimeSinceStartup);
        BeltShader.SetTexture(0, "NewFrame", mainTexture);
        BeltShader.SetTexture(0, "FrameBefore", mainTexturePrevFrame);
        BeltShader.SetTexture(1, "NewFrame", mainTexture);
        BeltShader.SetTexture(1, "FrameBefore", mainTexturePrevFrame);
        BeltShader.SetTexture(2, "NewFrame", mainTexture);
        BeltShader.SetTexture(2, "FrameBefore", mainTexturePrevFrame);
    }

    private void DispatchUpdateShaders()
    {
        ParticleShader.Dispatch(0, mainTexture.width / 256, 1, 1);
        ParticleShader.Dispatch(1, mainTexture.width / 16, mainTexture.height / 16, 1);
        ParticleShader.Dispatch(2, mainTexture.width / 16, mainTexture.height / 16, 1);

        BeltShader.Dispatch(0, 1, mainTexture.height / 256, 1);
        BeltShader.Dispatch(1, 1, mainTexture.height / 256, 1);
        BeltShader.Dispatch(2, mainTexture.width / 256, 1, 1);
    }

    private void SwitchTextures()
    {
        var pom = mainTexture;
        mainTexture = mainTexturePrevFrame;
        mainTexturePrevFrame = pom;
    }
}
