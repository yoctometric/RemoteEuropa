using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    AudioSource aud;

    [SerializeField] AudioClip[] songs;
    int prevIndex = -1;
    void Awake()
    {
        aud = gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(MusicLoop());
    }

    IEnumerator MusicLoop()
    {
        yield return new WaitForEndOfFrame();
        //every loop, play a different song

        int choice = Random.Range(0, songs.Length);
        if (songs.Length > 1) //There can only be a different song if there is more than one
        {
            while (choice == prevIndex)
            {
                choice = Random.Range(0, songs.Length); // makes sure it is never the previous song
            }
        }

        //now play logic

        AudioClip clip = songs[choice];
        aud.clip = clip;
        float waitTime = clip.length;
        aud.Play();

        //now wait for song to end and restart
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(MusicLoop());
    }
}
