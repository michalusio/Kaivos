using System.IO;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public static string LoadPath = null;

    [Range(1, 6)]
    public int MAP_SCALING = 4;

    public bool TileSetOn = true;

    public ComputeShader ParticleShader;
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
        
        ParticleShader.SetInt("seed", Random.Range(0, 10000));

        ParticleShader.SetTexture(0, "NewFrame", mainTexturePrevFrame);
        ParticleShader.SetTexture(5, "NewFrame", mainTexturePrevFrame);
        ParticleShader.SetTexture(5, "FrameBefore", mainTexture);
        ParticleShader.SetTexture(7, "NewFrame", mainTexturePrevFrame);
        ParticleShader.SetTexture(7, "FrameBefore", mainTexture);


        ParticleShader.SetTexture(1, "NewFrame", mainTexture);
        ParticleShader.SetTexture(1, "FrameBefore", mainTexturePrevFrame);
        ParticleShader.SetTexture(2, "NewFrame", mainTexture);
        ParticleShader.SetTexture(2, "FrameBefore", mainTexturePrevFrame);
        ParticleShader.SetTexture(3, "NewFrame", mainTexture);
        ParticleShader.SetTexture(3, "FrameBefore", mainTexturePrevFrame);
        ParticleShader.SetTexture(4, "NewFrame", mainTexture);
        ParticleShader.SetTexture(4, "FrameBefore", mainTexturePrevFrame);
        ParticleShader.SetTexture(6, "NewFrame", mainTexture);
        ParticleShader.SetTexture(6, "FrameBefore", mainTexturePrevFrame);
        
        ShadowShader.SetTexture(0, "Frame", mainTexture);
        ShadowShader.SetTexture(0, "Result", shadowTexture);
        ShadowShader.SetTexture(1, "Frame", mainTexture);
        ShadowShader.SetTexture(1, "Result", shadowTexture);

        if (LoadPath == null)
        {
            ParticleShader.Dispatch(0, mainTexture.width / 16, mainTexture.height / 16, 1);
            ParticleShader.Dispatch(5, 1, 1, 1);
            ParticleShader.Dispatch(7, 1, 1, 1);
        }
        
        ShadowShader.Dispatch(1, 1, 1, 1);
    }
    
    void Update()
    {
        timeSinceLastPhysics += Time.deltaTime;
        while (timeSinceLastPhysics > 0.1f)
        {
            timeSinceLastPhysics -= 0.1f;
            ParticleShader.SetFloat("Time", Time.realtimeSinceStartup);
            ParticleShader.SetTexture(1, "NewFrame", mainTexture);
            ParticleShader.SetTexture(1, "FrameBefore", mainTexturePrevFrame);
            ParticleShader.SetTexture(2, "NewFrame", mainTexture);
            ParticleShader.SetTexture(2, "FrameBefore", mainTexturePrevFrame);
            ParticleShader.SetTexture(3, "NewFrame", mainTexture);
            ParticleShader.SetTexture(3, "FrameBefore", mainTexturePrevFrame);
            ParticleShader.SetTexture(4, "NewFrame", mainTexture);
            ParticleShader.SetTexture(4, "FrameBefore", mainTexturePrevFrame);
            ParticleShader.SetTexture(6, "NewFrame", mainTexture);
            ParticleShader.SetTexture(6, "FrameBefore", mainTexturePrevFrame);
            ParticleShader.Dispatch(1, mainTexture.width / 256, 1, 1);
            ParticleShader.Dispatch(2, 1, mainTexture.height / 256, 1);
            ParticleShader.Dispatch(3, 1, mainTexture.height / 256, 1);
            ParticleShader.Dispatch(4, mainTexture.width / 256, 1, 1);
            ParticleShader.Dispatch(6, mainTexture.width / 16, mainTexture.height / 16, 1);
            SwitchTextures();
        }

        ShadowShader.SetFloat("Time", Time.realtimeSinceStartup);
        ShadowShader.SetTexture(0, "Frame", mainTexturePrevFrame);
        ShadowShader.Dispatch(0, mainTexture.width / 16, mainTexture.height / 16, 1);
    }

    private void SwitchTextures()
    {
        var pom = mainTexture;
        mainTexture = mainTexturePrevFrame;
        mainTexturePrevFrame = pom;
    }
}
