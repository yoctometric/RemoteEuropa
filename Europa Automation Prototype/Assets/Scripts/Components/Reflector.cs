using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script gets all the SP's in an parent's heirerarchy
/// It then duplicates them transparently
/// and then rotates and offset's them based on the obj
/// les go
/// </summary>
public class Reflector : MonoBehaviour
{
    List<SpriteRenderer> sps;
    Vector3 offset;
    Transform p;
    Transform container;
    bool created = false;
    void Start()
    {

        if (created)
        {//just that easy
            return;
        }
        sps = new List<SpriteRenderer>();
        if (StaticFunctions.lowGraphics)
        {
            return;
            //break out if there is low graphics
        }
        //container = GameObject.Find("Reflection Container").GetComponent<Transform>();
        offset = new Vector2(0.25f, -0.25f);
        p = gameObject.transform.parent;
        foreach (SpriteRenderer sp in p.GetComponentsInChildren<SpriteRenderer>())
        {
            //check if it is an editor
            if (sp.transform.name.Contains("Edit") || sp.transform.name.Contains("noreflect"))
            {
                continue;
            }
            SpriteRenderer nsp = new GameObject().AddComponent<SpriteRenderer>();

            nsp.sprite = sp.sprite;
            nsp.color = sp.color;
            nsp.color = new Color(nsp.color.r, nsp.color.g, nsp.color.b, 0.25f);
            sps.Add(nsp);//why

            nsp.transform.parent = sp.transform;
            nsp.name = "reflect";
            nsp.sortingOrder = sp.sortingOrder - 15;
            nsp.transform.localScale = new Vector3(1, 1, 1);
        }
        
        created = true;

    }

    void Update()
    {
        foreach(SpriteRenderer sp in sps)
        {
            sp.transform.position = sp.transform.parent.position + offset;
            sp.transform.rotation = sp.transform.parent.rotation;
        }
    }
}
