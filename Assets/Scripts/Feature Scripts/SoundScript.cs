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

    private Color[] colorsAround;
    private float timeSinceColorRefresh;

    void Start()
    {
        MiningAudios = MiningSource.GetComponents<AudioSource>();
        BackgroundAudios = BackgroundSource.GetComponents<AudioSource>();
        
        LowPass = Camera.main.GetComponent<AudioLowPassFilter>();

        ClassManager.SoundScript = this;
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

    void Update()
    {
        timeSinceColorRefresh += Time.deltaTime;

        LowPass.cutoffFrequency = 1000 + 10000 * (ClassManager.MainScript.transform.position.y + 512) / 1024f;

        if ((BackgroundAudios?.Length ?? 0) == 0) return;
        foreach(var bs in BackgroundAudios)
        {
            if (!bs.isPlaying)
            {
                var clips = GetClipsFromAround();
                if (clips.Length == 0) continue;
                bs.PlayOneShot(clips[Random.Range(0, clips.Length)]);
                break;
            }
        }
    }

    private AudioClip[] GetClipsFromAround()
    {
        if (timeSinceColorRefresh > 1.0f)
        {
            UpdateColorsAround();
        }
        int machineBlocks = 1;
        int overBlocks = 0;
        int underBlocks = 0;

        var randomV = Random.Range(0, machineBlocks + overBlocks + underBlocks);

        if (randomV < machineBlocks) return MachineClips;
        if (randomV < overBlocks + machineBlocks) return OverworldClips;
        return UndergroundClips;
    }

    private void UpdateColorsAround()
    {
        timeSinceColorRefresh = 0;

        var playerPos = new Vector2(ClassManager.MainScript.mainTexture.width / 2 - ClassManager.MainScript.transform.position.x, ClassManager.MainScript.mainTexture.height / 2 - ClassManager.MainScript.transform.position.y);

        colorsAround = ClassManager.MapReadService.GetFromTexture(playerPos - new Vector2(10, 10), new Vector2Int(20, 20));
    }
}
