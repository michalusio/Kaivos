using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public AudioClip[] MiningClips;
    public AudioClip[] OverworldClips;
    public AudioClip[] UndergroundClips;
    public AudioClip[] MachineClips;
    
    public GameObject MiningSource;
    public GameObject BackgroundSource;

    private AudioSource[] MiningAudios;
    private AudioSource[] BackgroundAudios;

    private AudioLowPassFilter LowPass;

    public int MachineBlocks, OverBlocks, UnderBlocks;
    private float timeSinceColorRefresh;

    void Start()
    {
        MiningAudios = MiningSource.GetComponents<AudioSource>();
        BackgroundAudios = BackgroundSource.GetComponents<AudioSource>();

        LowPass = Camera.main.GetComponent<AudioLowPassFilter>();

        ClassManager.SoundScript = this;
        UpdateColorsAround();
    }
    
    public void MiningSound(float speed)
    {
        if ((MiningClips?.Length ?? 0) == 0) return;
        foreach(var ms in MiningAudios)
        {
            if (!ms.isPlaying)
            {
                ms.pitch = speed * Random.Range(0.7f, 1.3f);
                ms.PlayOneShot(MiningClips[Random.Range(0, MiningClips.Length)]);
                break;
            }
        }
    }

    public void StopMining()
    {
        foreach(var ms in MiningAudios)
        {
            ms.Stop();
        }
    }

    void Update()
    {
        if (ClassManager.MainScript == null) return;
        timeSinceColorRefresh += Time.deltaTime;

        LowPass.cutoffFrequency = 1000 + 10000 * (ClassManager.MainScript.transform.position.y + 512) / 1024f;

        if ((BackgroundAudios?.Length ?? 0) == 0) return;

        foreach(var bs in BackgroundAudios)
        {
            if (!bs.isPlaying)
            {
                var clip = GetClipFromAround();
                bs.PlayOneShot(clip.Item1, clip.Item2);
                break;
            }
        }
    }

    private (AudioClip, float) GetClipFromAround()
    {
        if (timeSinceColorRefresh > 1.0f)
        {
            UpdateColorsAround();
        }

        var randomV = Random.Range(0, MachineBlocks + OverBlocks + UnderBlocks);

        int chosenBlocks = OverBlocks;
        AudioClip[] clipTable = OverworldClips;

        if (randomV < MachineBlocks)
        {
            chosenBlocks = MachineBlocks;
            clipTable = MachineClips;
        }
        if (randomV < UnderBlocks + MachineBlocks)
        {
            chosenBlocks = UnderBlocks;
            clipTable = UndergroundClips;
        }
        
        return (clipTable[Random.Range(0, clipTable.Length)], 1 + chosenBlocks * (0.3f / (MachineBlocks + OverBlocks + UnderBlocks)));
    }

    private void UpdateColorsAround()
    {
        timeSinceColorRefresh = 0;

        var playerPos = new Vector2(ClassManager.MainScript.mainTexture.width / 2 - ClassManager.MainScript.transform.position.x, ClassManager.MainScript.mainTexture.height / 2 - ClassManager.MainScript.transform.position.y);

        var colorsAround = ClassManager.MapReadService.GetFromTexture(playerPos - new Vector2(10, 10), new Vector2Int(20, 20));

        MachineBlocks = 0;
        OverBlocks = 0;
        UnderBlocks = 0;

        foreach(var col in colorsAround)
        {
            if (MachineColors.Any(c => MapReadService.ColorNear(c, col)))
            {
                MachineBlocks++;
            }
            else if (OverColors.Any(c => MapReadService.ColorNear(c, col)))
            {
                OverBlocks++;
            }
            else if (UnderColors.Any(c => MapReadService.ColorNear(c, col)))
            {
                UnderBlocks++;
            }
        }

        if (MachineBlocks == 0 && OverBlocks == 0 && UnderBlocks == 0)
        {
            OverBlocks = 1;
        }
    }

    private static List<Color> MachineColors = new List<Color>
    {
        new Color(0f, 0.6f, 0.6f, 1.0f),
        new Color(0.1f, 0.6f, 0.6f, 1.0f),
        new Color(0.2f, 0.6f, 0.6f, 1.0f),
        new Color(0.0f, 0.0f, 0.9f, 1.0f),
        new Color(0.1f, 0.0f, 0.9f, 1.0f),
        new Color(0.2f, 0.0f, 0.9f, 1.0f),
        new Color(0.3f, 0.0f, 0.9f, 1.0f),

        new Color(0, 0.2f, 0.1f, 1.0f),
        new Color(0.1111111111111111111f, 0.2f, 0.1f, 1.0f),
        new Color(0.2222222222222222222f, 0.2f, 0.1f, 1.0f),

        new Color(0.3333333333333333333f, 0.2f, 0.1f, 1.0f),
        new Color(0.4444444444444444444f, 0.2f, 0.1f, 1.0f),
        new Color(0.5555555555555555555f, 0.2f, 0.1f, 1.0f),

        new Color(0.6666666666666666666f, 0.2f, 0.1f, 1.0f),
        new Color(0.7777777777777777777f, 0.2f, 0.1f, 1.0f),
        new Color(0.8888888888888888888f, 0.2f, 0.1f, 1.0f),
        
        new Color(0, 0.3f, 0.1f, 1.0f),
        new Color(0.1111111111111111111f, 0.3f, 0.1f, 1.0f),
        new Color(0.2222222222222222222f, 0.3f, 0.1f, 1.0f),

        new Color(0.3333333333333333333f, 0.3f, 0.1f, 1.0f),
        new Color(0.4444444444444444444f, 0.3f, 0.1f, 1.0f),
        new Color(0.5555555555555555555f, 0.3f, 0.1f, 1.0f),

        new Color(0.6666666666666666666f, 0.3f, 0.1f, 1.0f),
        new Color(0.7777777777777777777f, 0.3f, 0.1f, 1.0f),
        new Color(0.8888888888888888888f, 0.3f, 0.1f, 1.0f),
    };

    private static List<Color> UnderColors = new List<Color>
    {
        new Color(0.2f, 0.2f, 0.2f, 1.0f),
        new Color(0.3f, 0.2f, 0.2f, 1.0f),
        new Color(0.4f, 0.2f, 0.2f, 1.0f),
        new Color(0.0f, 0.3f, 0.3f, 1.0f),
        new Color(0.1f, 0.3f, 0.3f, 1.0f),
        new Color(0.0f, 0.4f, 0.4f, 1.0f),
        new Color(0.1f, 0.4f, 0.4f, 1.0f),
        new Color(0.2f, 0.4f, 0.4f, 1.0f),
        new Color(0.3f, 0.4f, 0.4f, 1.0f),
        new Color(0.0f, 0.7f, 0.7f, 1.0f),
        new Color(1.0f, 0.7f, 0.7f, 1.0f),
        new Color(0.0f, 0.0f, 0.8f, 1.0f),
        new Color(0.0f, 0.1f, 0.8f, 1.0f),
        new Color(0.1f, 0.0f, 0.8f, 1.0f),
        new Color(0.1f, 0.1f, 0.8f, 1.0f),
    };

    private static List<Color> OverColors = new List<Color>
    {
        new Color(0, 0.2f, 0.2f, 1.0f),
        new Color(0.1f, 0.2f, 0.2f, 1.0f),
        //new Color(0, 0, 0, 0),
        new Color(0.0f, 0.5f, 0.2f, 1.0f),
        new Color(0.0f, 0.6f, 0.2f, 1.0f),
    };
}
