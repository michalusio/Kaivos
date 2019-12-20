using System.IO;
using Assets.Scripts;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public static string LoadPath = null;
    public SaveStateStats LoadSaveState;

    public int Seed { get; private set; }

    [Range(1, 6)]
    public int MAP_SCALING = 4;

    [Range(0.1f, 1)]
    public float MachineSpeed = 1;

    public bool TileSetOn = true;

    public ComputeShader ParticleShader;
    public ComputeShader BeltShader;
    public ComputeShader PlaceShader;
    public ComputeShader ShadowShader;

    public RenderTexture mainTexture, mainTexturePrevFrame, shadowTexture;
    
    public const int MAP_SIZE = 1024;

    private float timeSinceLastPhysics;
    private float timeSinceLastMachine;

    void Start()
    {
        if (LoadPath == null)
        {
            Seed = Random.Range(0, 10000);
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
            LoadSaveState = LoadScript.LoadSaveState(Path.Combine(LoadPath, SaveUtilities.StatFileName));
            var loadTexture = LoadScript.LoadTextureData(Path.Combine(LoadPath, SaveUtilities.MapFileName));
            mainTexture = LoadScript.FromTexture2D(loadTexture);
            mainTexturePrevFrame = LoadScript.FromTexture2D(loadTexture);
            Seed = LoadSaveState.Seed;
        }

        shadowTexture = new RenderTexture(mainTexture.width, mainTexture.height, 0, RenderTextureFormat.ARGBFloat)
        {
            anisoLevel = 0,
            enableRandomWrite = true,
            autoGenerateMips = false,
            filterMode = FilterMode.Bilinear
        };
        shadowTexture.Create();
        
        PlaceShader.SetInt("seed", Seed);
        ParticleShader.SetInt("seed", Seed);
        BeltShader.SetInt("seed", Seed);

        SetupShaderTextures();

        if (LoadPath == null)
        {
            GenerateMap();
        }

        ShadowShader.Dispatch(1, 1, 1, 1);

        ClassManager.MainScript = this;
        ClassManager.MapReadService = new MapReadService();
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
        timeSinceLastMachine += Time.deltaTime;
        while (timeSinceLastPhysics >= 0.1f)
        {
            timeSinceLastPhysics -= 0.1f;
            
            SetupUpdateShaders();
            DispatchUpdateShaders();
            SwitchTextures();
        }

        while (timeSinceLastMachine >= MachineSpeed)
        {
            timeSinceLastMachine -= MachineSpeed;

            SetupMachineShader();
            DispatchMachineShader();
        }

        ShadowShader.SetFloat("Time", Time.realtimeSinceStartup);
        ShadowShader.SetTexture(0, "Frame", mainTexturePrevFrame);
        ShadowShader.Dispatch(0, mainTexture.width / 16, mainTexture.height / 16, 1);
    }

    private void SetupMachineShader()
    {
        ParticleShader.SetFloat("Time", Time.realtimeSinceStartup);
        ParticleShader.SetTexture(2, "NewFrame", mainTexturePrevFrame);
        ParticleShader.SetTexture(2, "FrameBefore", mainTexture);
    }

    private void DispatchMachineShader()
    {
        ParticleShader.Dispatch(2, mainTexture.width / 16, mainTexture.height / 16, 1);
    }

    private void SetupUpdateShaders()
    {
        ParticleShader.SetFloat("Time", Time.realtimeSinceStartup);
        ParticleShader.SetTexture(0, "NewFrame", mainTexture);
        ParticleShader.SetTexture(0, "FrameBefore", mainTexturePrevFrame);
        ParticleShader.SetTexture(1, "NewFrame", mainTexture);
        ParticleShader.SetTexture(1, "FrameBefore", mainTexturePrevFrame);

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
