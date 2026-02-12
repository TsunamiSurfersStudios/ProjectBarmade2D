
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public AudioMixer audioMixer; // References objects in the setting menu Edit -> ProjectSetting -> Quality
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Dropdown textureDropdown;
    public Dropdown aaDropdown;
    public Slider volumeSlider;

    float currentVolume;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
