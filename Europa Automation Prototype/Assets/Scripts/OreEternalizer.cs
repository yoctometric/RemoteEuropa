using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreEternalizer : MonoBehaviour
{
    //Mobile Ore Eternalizer Device should be abbreviated to MOED
    Animator anim;
    OreController mountedOre;
    GameConsole cons;

    public bool goThru = true;

    void Start()
    {
        cons = GameObject.FindObjectOfType<GameConsole>();

        anim = gameObject.GetComponent<Animator>();
        if (!goThru)
        {
            anim.SetTrigger("Bypass");
            Landed();
        }
    }
    public void Landed()
    {
        cons?.AddLine("MOED has been mounted");
        bool foundOre = false;
        Collider2D[] possibilities = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach(Collider2D p in possibilities)
        {
            if (p.GetComponent<OreController>())
            {
                foundOre = true;
                mountedOre = p.GetComponent<OreController>();
            }
        }

        if (foundOre)
        {
            print("ore found");
            mountedOre.eternal = true;
        }
        else
        {
            print("exit");
            Leave();
        }
    }

    /*
    public void Detonate()
    {
        //where does this activate from? Does the eternalizer have its own hup? no... must be on ore hup
    }
    //exiting is unbalanced. Instead, allow the eternalizer to detonate destroying the ore. Only use for failstate
    */
    public void Leave()
    {
        anim.SetBool("Exit", true);
    }
}
