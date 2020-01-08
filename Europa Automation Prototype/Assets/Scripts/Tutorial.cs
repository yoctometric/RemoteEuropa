using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Tutorial : MonoBehaviour
{
    int storedCop = 0;
    int prevCop = 0;
    int storedBrick = 0;
    int prevBrick = 0;
    int storedIro = 0;
    int prevIro = 0;
    int[] activatedEvents;
    [Header("display")]
    [SerializeField] Animator panel;
    [SerializeField] TMP_Text pText;
    [Header("events")]
    [SerializeField] GameObject[] event1Objs;
    [SerializeField] GameObject[] event2Objs;
    [SerializeField] GameObject[] event3Objs;
    [SerializeField] GameObject[] event4Objs;


    float timeSinceStart = 0;
    void Start()
    {
        timeSinceStart = Time.time;
        activatedEvents = new int[4];
        
        for(int i = 0; i < activatedEvents.Length; i++)
        {
            activatedEvents[i] = 0;
        }     
    }
    public void StartEvent(string item)
    {
        if(item == "Refined Iron" && activatedEvents[3] == 0)
        {
            Event3();
        }else if(item == "Refined Copper" && activatedEvents[1] == 0)
        {
            Event1();
        }
        else if (item == "Brick" && activatedEvents[2] == 0)
        {
            Event2();
        }
    }

    void Event1()
    {
        activatedEvents[1] = 1;

        panel.SetTrigger("go");
        pText.text = "Good job!";
        foreach(GameObject obj in event1Objs)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in event2Objs)
        {
            obj.SetActive(true);
        }
    }
    void Event2()
    {
        activatedEvents[2] = 1;

        panel.SetTrigger("go");
        pText.text = "Almost done!";
        foreach (GameObject obj in event2Objs)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in event3Objs)
        {
            obj.SetActive(true);
        }
    }
    void Event3()
    {
        activatedEvents[3] = 1;

        panel.SetTrigger("go");
        pText.text = "Tutorial Completed!";
        foreach (GameObject obj in event3Objs)
        {
            if(obj.name == "Fan (1)" || obj.name.Contains("tip3"))
            {
                obj.SetActive(false);
            }
        }
        foreach (GameObject obj in event4Objs)
        {
            obj.SetActive(true);
        }
    }
}
