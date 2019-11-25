using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public GameObject RealObject;
    [SerializeField] SpriteRenderer sp;
    [SerializeField] int index;
    ObjectPlacer cursor;
    private void Start()
    {
        cursor = GameObject.FindObjectOfType<ObjectPlacer>();
    }
    private void Update()
    {
        if (cursor.canPlace)
        {
            sp.color = new Color(0, 1, 0, 0.25f);
        }
        else
        {
            sp.color = new Color(1, 0, 0, 0.25f);
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        cursor.canPlace = false;
        sp.color = new Color(1, 0, 0, 0.25f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        cursor.canPlace = true;
        sp.color = new Color(0, 1, 0, 0.25f);
    }
    */
}
