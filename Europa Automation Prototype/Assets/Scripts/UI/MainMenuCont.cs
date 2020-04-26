using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuCont : MonoBehaviour
{
    /*
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] Slider scrollSenseSlider;
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] Toggle graphicsToggle;
    [SerializeField] Toggle FPSToggle;
    [SerializeField] Toggle LowParticlesToggle;*/
    private void Start()
    {
        /*
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        scrollSenseSlider.value = PlayerPrefs.GetFloat("ScrollSense");

        if(PlayerPrefs.GetFloat("ScrollSense") == 0)
        {
            PlayerPrefs.SetFloat("ScrollSense", 1);
        }
        if(PlayerPrefs.GetInt("LowGraphics") == 1)
        {
            graphicsToggle.isOn = true;
        }
        if(PlayerPrefs.GetInt("ShowFPS") == 1)
        {
            FPSToggle.isOn = true;
        }
        if (PlayerPrefs.GetInt("MinimalParticles") == 1)
        {
            LowParticlesToggle.isOn = true;
        }
        */
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.RightControl))
        {
            print("Playerprefs cleared");
            PlayerPrefs.DeleteAll();
        }
        
    }
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
    public void EnterTutorial(bool bypass)
    {
        if (bypass)
        {
            SceneManager.LoadScene(4);

        }else
        if (PlayerPrefs.GetInt("HasPlayedTutorial") == 0)
        {
            PlayerPrefs.SetInt("HasPlayedTutorial", 1);
            SceneManager.LoadScene(4);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    /*
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
    }*/


    public void LoadAScene(int scene)
    {
        StartCoroutine(Load(scene));
    }
    IEnumerator Load(int scene)
    {
        GameObject.FindObjectOfType<Transition>().GetComponent<Animator>().SetTrigger("Out");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }
    /*
    public void setSensitivity()
    {
        PlayerPrefs.SetFloat("ScrollSense", scrollSenseSlider.value);
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
    }*/
}
