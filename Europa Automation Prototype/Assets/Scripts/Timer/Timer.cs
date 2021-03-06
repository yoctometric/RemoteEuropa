﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    ///This is going to work. When an obj is instantiated, it adds itself to the global timer. 
    ///When the timer runs, it ticks each obj. each obj has its own method for handling a tick
    ///besides, this only has to be done for miners, right? All else depends on them.
    //
    public List<Miner> miners;
    public List<Pump> pumps;

    WaitForSeconds seconds = new WaitForSeconds(0.25f);
    private void Start()
    {
        StartCoroutine(Tick());
    }
    private void OnLevelWasLoaded(int level)
    {
        miners.Clear();
        pumps.Clear();
    }
    IEnumerator Tick()
    {
        //now cycle 20 times
        for(int i = 0; i < 20; i++)
        {
            yield return seconds; //less resource intensive than a new wait every cycle
            foreach (Miner min in miners)
            {
                min.Tick(i);
            }
            foreach (Pump p in pumps)
            {
                p.Tick(i);
            }
        }
        StartCoroutine(Tick());
    }


    /*
    public float coolDown = 1;
    Color a = new Color(255, 255, 0, 255);
    Color b = new Color(0, 255, 0, 255);
    [SerializeField] Image sp;
    private void Start()
    {
        
    }
    private void Update()
    {
        float div = Mathf.RoundToInt(Time.time * 10);
        float divA = div % coolDown;
        print(div.ToString() + ' ' +  divA.ToString());
        if(divA == 0)
        {
            sp.color = a;
        }
        else
        {
            sp.color = b;
        }
    }
    */
}
