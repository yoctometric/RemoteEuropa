using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
public class Warning : MonoBehaviour
{
    [SerializeField] int maxDist = 250;
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject panel;
    int attempts = 0;
    bool warningRightNow = false;
    bool waitForOver = false;
    bool ignoreDistunctions = false;
    Vector2 mid = new Vector2(0, 0);
    Animator anim;

    [SerializeField] AudioClip[] progressionSounds;
    SoundMaker soundMaker;
    private void Start()
    {
        anim = panel.GetComponent<Animator>();
        attempts = 0;
        soundMaker = GameObject.FindObjectOfType<SoundMaker>();
    }
    void Update()
    {
        //calculate dist
        float dist = Vector2.Distance(mid, transform.position);
        if (!ignoreDistunctions)
        {
            if (!waitForOver)
            {
                if (dist > maxDist * 3)
                {
                    if (attempts < 2)
                    {
                        InitiateWarning("goodbye", true, false);
                    }
                    else if (attempts < 3)
                    {
                        InitiateWarning("You are persistent... head north from 0,0", true, false);
                    }
                    else if (dist > maxDist * 3.5f)
                    {
                        CancelWarning();
                    }
                }
                else if (dist > maxDist * 2)
                {
                    InitiateWarning("WARNING:\n DESTRUCTiON IMMINENT!", false, false);
                }
                else if (dist > maxDist)
                {
                    ///warn
                    InitiateWarning("RETURN TO PLAY AREA!", false, false);
                }
            }
            if (warningRightNow && dist < maxDist)
            {
                CancelWarning();
            }
        }
    }
    IEnumerator PlaySounds()
    {
        if (!soundMaker)
        {
            soundMaker = GameObject.FindObjectOfType<SoundMaker>();
        }
        soundMaker.MakeSound(progressionSounds[0]);
        yield return new WaitForSeconds(progressionSounds[0].length);
        soundMaker.MakeSound(progressionSounds[1]);
    }

    public void InitiateWarning(string warning, bool reset, bool overrideDistFunctions)
    {
        StartCoroutine(PlaySounds());
        warningRightNow = true;
        ignoreDistunctions = overrideDistFunctions;
        //panel.SetActive(true);
        anim.SetBool("warning", true);
        text.text = warning;
        if (reset)
        {
            gameObject.GetComponent<ObjectPlacer>().ResetPosition();
            if (!waitForOver)
            {
                if (!overrideDistFunctions)
                {
                    attempts++;
                    print(attempts);
                }
                waitForOver = true;
            }
        }
    }
    public void CancelWarning()
    {
        print("resetttingcam" + attempts.ToString());
        warningRightNow = false;
        //panel.SetActive(false);
        anim.SetBool("warning", false);
        text.text = "";
        waitForOver = false;
        ignoreDistunctions = false;
    }
}
