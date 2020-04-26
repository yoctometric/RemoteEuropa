using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] GameObject sinkEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //cause items to sink on contact. You know, because it is a hole
        if(other.GetComponent<Item>())
        {
            other.GetComponent<Item>().AnimateDestruct();
            Instantiate(sinkEffect, other.transform.position, Quaternion.identity);
        }
    }
}
