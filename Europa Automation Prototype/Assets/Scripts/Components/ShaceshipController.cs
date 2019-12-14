using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaceshipController : MonoBehaviour
{
    Animator anim;
    [SerializeField]Transform satellite;
    [SerializeField] float d = 20;
    [SerializeField] float startDlay;
    void Awake()
    {        
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(startDlay);
        anim.SetTrigger("Go");
    }
    void Update()
    {
        float dist = Vector2.Distance(transform.position, satellite.position);
        if(Mathf.Abs(dist) < d)
        {
            anim.SetTrigger("Fill");
        }
    }
}
