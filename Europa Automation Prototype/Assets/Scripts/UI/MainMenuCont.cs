﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuCont : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] Slider scrollSenseSlider;
    [SerializeField] AudioMixer mainMixer;
    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        scrollSenseSlider.value = PlayerPrefs.GetFloat("ScrollSense");

        if(PlayerPrefs.GetFloat("ScrollSense") == 0)
        {
            PlayerPrefs.SetFloat("ScrollSense", 1);
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HandleMusicVolume(string type)
    {
        if(type == "Music")
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

    public void setSensitivity()
    {
        PlayerPrefs.SetFloat("ScrollSense", scrollSenseSlider.value);
    }
    public void LoadAScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
