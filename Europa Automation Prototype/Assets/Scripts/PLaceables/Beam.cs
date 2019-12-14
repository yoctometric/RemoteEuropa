using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public Power target;
    public Power origin;
    public SpriteRenderer sp;
    public float dist = 0;
    private void Start()
    {
        dist = Vector2.Distance(target.transform.position, origin.transform.position);
        sp = gameObject.GetComponent<SpriteRenderer>();
    }
}
