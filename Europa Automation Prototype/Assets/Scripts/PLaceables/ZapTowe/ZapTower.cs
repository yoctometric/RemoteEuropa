using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapTower : MonoBehaviour
{
    GameObject laser;
    [HideInInspector]public List<IncinerationBeam> childLasers;
    [SerializeField] float maxDist = 20;
    private void Start()
    {
        //find all other towers
        ZapTower[] tows = GameObject.FindObjectsOfType<ZapTower>();
        foreach(ZapTower tow in tows)
        {
            if (Vector3.Distance(tow.transform.position, transform.position) < maxDist)
            {
                DrawBeam(tow, this);
            }
        }
    }

    void DrawBeam(ZapTower otherTower, ZapTower thisTower)
    {
        IncinerationBeam beam = Instantiate(laser, transform.position, Quaternion.identity).GetComponent<IncinerationBeam>();
        thisTower.childLasers.Add(beam);
        otherTower.childLasers.Add(beam);
        //now move beam to proper angle
        Vector2 avgPos = (otherTower.transform.position + this.transform.position) / 2;
        beam.transform.position = avgPos;
        //since the scale of the beam is 1, multiplying it by dist should be fine
        float dist = Vector2.Distance(otherTower.transform.position, this.transform.position);
        beam.transform.localScale = new Vector2(beam.transform.localScale.x, dist);
    }

    private void OnDestroy()
    {
        
    }
    ///Lasers have a sp, box collider, and on collision check if they hit an item. If so, they start blasting. Otherwise, they chill
    ///Also towers have a refrence to all beams in contact with them. On destroy, they remove them
    ///Also beams are instantly removed if there is an ore or other object in their way.
}
