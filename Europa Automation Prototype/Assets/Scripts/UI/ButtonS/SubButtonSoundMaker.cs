using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SubButtonSoundMaker : MonoBehaviour
{
    SoundMaker maker;
    private void Start()
    {
        maker = GameObject.FindObjectOfType<SoundMaker>();
    }
    public void PlaySound(AudioClip clip)
    {
        if (!maker)
        {
            maker = GameObject.FindObjectOfType<SoundMaker>();
            maker.MakeSound(clip);
        }
        else
        {
            maker.MakeSound(clip);
        }
    }
}
