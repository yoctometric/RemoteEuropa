using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SettingsPanel : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] Slider scrollSenseSlider;
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] Toggle graphicsToggle;
    [SerializeField] Toggle FPSToggle;
    [SerializeField] Toggle LowParticlesToggle;
    [SerializeField] GameObject fpsDisplay;
    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        scrollSenseSlider.value = PlayerPrefs.GetFloat("ScrollSense");

        if (PlayerPrefs.GetFloat("ScrollSense") == 0)
        {
            PlayerPrefs.SetFloat("ScrollSense", 1);
        }
        if (PlayerPrefs.GetInt("LowGraphics") == 1)
        {
            graphicsToggle.isOn = true;
        }
        if (PlayerPrefs.GetInt("ShowFPS") == 1)
        {
            FPSToggle.isOn = true;
        }
        if (PlayerPrefs.GetInt("MinimalParticles") == 1)
        {
            LowParticlesToggle.isOn = true;
        }

        //gameObject.SetActive(false);


    }
    public void SetSensitivity()
    {
        PlayerPrefs.SetFloat("ScrollSense", scrollSenseSlider.value);
    }
    public void HandleMusicVolume(string type)
    {
        if (type == "Music")
        {
            float volume = musicVolumeSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", volume);
            mainMixer.SetFloat("MusicVolume", volume);
        }
        else if (type == "SFX")
        {
            float volume = SFXVolumeSlider.value;
            PlayerPrefs.SetFloat("SFXVolume", volume);
            mainMixer.SetFloat("SFXVolume", volume);
        }
        else
        {
            print("ERROR! NON VALID MUSIC VOLUME HANDLER CHOSEN");
        }
    }
    public void ToggleParticles()
    {
        if (LowParticlesToggle.isOn)
        {
            print("on");
            StaticFunctions.minimalParticles = true;
            PlayerPrefs.SetInt("MinimalParticles", 1);
        }
        else
        {
            print("off");
            StaticFunctions.minimalParticles = false;
            PlayerPrefs.SetInt("MinimalParticles", 0);
        }
    }
    public void ToggleFPS()
    {
        if (FPSToggle.isOn)
        {
            StaticFunctions.ShowFPS = true;
            PlayerPrefs.SetInt("ShowFPS", 1);
        }
        else
        {
            print("off");
            StaticFunctions.ShowFPS = false;
            PlayerPrefs.SetInt("ShowFPS", 0);
        }

        fpsDisplay?.gameObject.SetActive(StaticFunctions.ShowFPS);
    }
    public void ToggleGraphics()
    {
        if (graphicsToggle.isOn)
        {
            StaticFunctions.lowGraphics = true;
            PlayerPrefs.SetInt("LowGraphics", 1);
        }
        else
        {
            StaticFunctions.lowGraphics = false;
            PlayerPrefs.SetInt("LowGraphics", 0);
        }
    }
    
}
