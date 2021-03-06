﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPod : MonoBehaviour
{
    private string typ;
    private int amnt;

    [SerializeField] AudioClip risingClip;
    [SerializeField] AudioClip landingClip;

    Animator anim;
    Inventory invent;
    AudioSource aud;
    private void Awake()
    {
        aud = gameObject.GetComponent<AudioSource>();
        anim = gameObject.GetComponent<Animator>();
        invent = GameObject.FindObjectOfType<Inventory>();
    }

    public void Drop(string type, int amount)
    {
        aud.clip = landingClip;
        aud.Play();
        if (!anim)
        {
            anim = gameObject.GetComponent<Animator>();
        }
        typ = type;
        amnt = amount;
        //Now anim
        print("droppping");
        anim.SetTrigger("Drop");
    }

    public void AnimatorDropEnd()
    {
        if (!invent)//make sure there is an inventory
        {
            invent = GameObject.FindObjectOfType<Inventory>();
        }

        invent.UpdateInventory(typ, amnt);
        anim.SetTrigger("Up");
    }

    public void StopDropAudio()
    {
        aud.Stop();
    }

    public void StartRiseAudio()
    {
        aud.clip = risingClip;
        aud.Play();
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }
}
