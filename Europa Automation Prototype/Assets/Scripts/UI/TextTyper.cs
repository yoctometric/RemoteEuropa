using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//require nescessary items
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TMP_Text))]

public class TextTyper : MonoBehaviour
{
    //This script will allow for ui text to be typed over time, with a sound
    public bool playOnStart = true;
    public bool blinkingCursor = true;
    public float playDelay = 1;
    public float charDelay = 0.1f;
    public float blinkDelay = 0.5f;
    bool done = false;
    bool started = false;
    public GameObject objToActivate;

    string cursor = " ";
    //audio controls
    public float pitchShift = 0;

    string initialString = "";
    string currentString = "";
    int iteration = 0;

    TMP_Text Ttext;
    AudioSource aud;
    void Start()
    {
        //reset started
        started = false;
        //get the string from the tmp asset
        aud = gameObject.GetComponent<AudioSource>();
        Ttext = gameObject.GetComponent<TMP_Text>();

        initialString =Ttext.text;
        //clear the tmp asset
        Ttext.text = "";
        //if play on start, play it on start dummy
        if (playOnStart)
        {
            StartCoroutine(StartTyping(playDelay));
        }

    }

    private void Update()
    {
        if (!done)
        {
            Ttext.text = currentString + cursor;
        }
        if (Input.anyKeyDown)
        {
            charDelay = 0.005f;
            StartCoroutine(StartTyping(0));

        }

    }
    //blinking IEnum
    IEnumerator Blinking()
    {
        yield return new WaitForSecondsRealtime(blinkDelay);

        cursor = " ";
        yield return new WaitForSecondsRealtime(blinkDelay);
        if (!done)
        {
            cursor = "|";
        }
        if (!done)
        {
            StartCoroutine(Blinking());
        }
        else
        {
            cursor = " ";
            Ttext.text = currentString + cursor;
        }

    }
    //Starter IEnum
    IEnumerator StartTyping(float delay)
    {
        //wait start delay
        yield return new WaitForSecondsRealtime(delay);
        if (started)
        {
            yield break;
        }

        started = true;
        //set obj active if exists
        if (objToActivate)
        {
            objToActivate.SetActive(true);
        }
        if (blinkingCursor)
        {
            StartCoroutine(Blinking());
        }
        //iterate
        iteration++;
        //set currentstring
        currentString = initialString.Substring(0, 1);
        //play sounds
        aud.Play();
        //start main
        StartCoroutine(IterateLetterType());
    }

    IEnumerator IterateLetterType()
    {
        float pitchCurrentShift = 0;
        //set current pitch
        if(pitchShift != 0)
        {
            pitchCurrentShift = Random.Range(-pitchShift, pitchShift);
        }
        aud.pitch += pitchCurrentShift;

        //wait
        yield return new WaitForSecondsRealtime(charDelay);
        //play audio and reset pitch
        aud.Play();
        aud.pitch -= pitchCurrentShift;
        //iterate
        iteration++;
        //set string
        currentString = initialString.Substring(0, iteration);
        if (currentString.Length < initialString.Length)
        {
            StartCoroutine(IterateLetterType());
        }
        else
        {
            //currentString = currentString + initialString[initialString.Length - 1];
            yield return new WaitForSecondsRealtime(0.1f);
            done = true;
        }
    }
}
