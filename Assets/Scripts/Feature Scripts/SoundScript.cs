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

    void Start()
    {
        MiningAudios = MiningSource.GetComponents<AudioSource>();
        BackgroundAudios = BackgroundSource.GetComponents<AudioSource>();
        
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
        
    }
}
