using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    [SerializeField] AudioSource sourcePrefab;

    public void MakeSound(AudioClip clip)
    {
        AudioSource s = Instantiate(sourcePrefab, Vector3.zero, Quaternion.identity);
        Destroy(s.gameObject, clip.length + 0.5f);
        s.clip = clip;
        s.Play();
    }
}