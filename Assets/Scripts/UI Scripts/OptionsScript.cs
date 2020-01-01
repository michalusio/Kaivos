using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    public Dropdown ResolutionDropdown;
    public Toggle FullScreenToggle;
    public Slider VolumeSlider;

    public AudioClip VolumeSlideClip;

    private Resolution[] resolutions;

    void Awake()
    {
        resolutions = Screen.resolutions.Where(r => r.width > 1000).ToArray();

        ResolutionDropdown.ClearOptions();

        var options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            var resolution = resolutions[i];
            options.Add(resolution.ToString());

            if (Screen.currentResolution.Equals(resolution))
            {
                currentResolutionIndex = i;
            }
        }
        ResolutionDropdown.onValueChanged.RemoveAllListeners();
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.onValueChanged.AddListener(SetResolution);
        ResolutionDropdown.RefreshShownValue();
        
        FullScreenToggle.onValueChanged.RemoveAllListeners();
        FullScreenToggle.isOn = Screen.fullScreen;
        FullScreenToggle.onValueChanged.AddListener(SetFullScreen);
        
        VolumeSlider.onValueChanged.RemoveAllListeners();
        VolumeSlider.value = ClassManager.Volume * VolumeSlider.maxValue;
        VolumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    public void SetVolume(float volume)
    {
        ClassManager.Volume = volume / VolumeSlider.maxValue;
        AudioListener.volume = ClassManager.Volume;
        Camera.main.GetComponent<AudioSource>().PlayOneShot(VolumeSlideClip);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}