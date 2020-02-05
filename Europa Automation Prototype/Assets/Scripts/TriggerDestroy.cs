using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDestroy : MonoBehaviour
{
    /// <summary>
    /// Ths is explicitly, and only relevant to the item consumption effect
    /// </summary>
    Transform target;
    bool isApplicationQuitting = false;//so that you can cancel isntantiate
    bool activated = false;
    public bool sank = false;
    [SerializeField] GameObject GoodConsumeEffect;
    private void Start()
    {
        //gameObject.GetComponent<CircleCollider2D>().radius = 2*(1 / transform.localScale.x);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 && !activated && !sank)//this is the ground layer
        {
            activated = true;
            Instantiate(GoodConsumeEffect, transform.position, Quaternion.identity);
            target = other.transform;
        }
    }
    private void Update()
    {
        if (target)
        {
            transform.parent.position = Vector2.Lerp(transform.position, target.position, 0.01f);
        }
    }
    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
    public void Dest()
    {
        if (transform.parent && !isApplicationQuitting)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
