using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour
{
    public string typeOfItem = "";
    public int typeInt = -1;
    public bool canSink = true;
    bool passedInvulnerability = false;
    bool startedShrinking = false;
    Rigidbody2D rb;
    [SerializeField] ParticleSystem onEnableSystem;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(StartInvulnerability());
        TrailRenderer t = gameObject.GetComponent<TrailRenderer>();
        t.startColor = gameObject.GetComponent<SpriteRenderer>().color;
        t.endColor = new Color(1, 1, 1, 0.5f);
        t.endWidth = 0f;
        t.startWidth = 0.4f;

    }
    //this allows the item to sink into the ice after it gets too slow, to improve performance and fun
    private void Update()
    {
        if (canSink && !startedShrinking && passedInvulnerability)
        {
            //if it can sink
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                //if its x vel is low
                if (Mathf.Abs(rb.velocity.y) < 0.1f)
                {
                    //if its y vel is low too, start sinking
                    startedShrinking = true;
                    StartCoroutine(Shrink());
                }
            }
        }
    }
    private void OnEnable()
    {
        //when the object passes thru a launcher it will deactivate. Set this up so that it doesnt shrinkk when it stops moving.
        passedInvulnerability = false;
        StartCoroutine(StartInvulnerability());
        Instantiate(onEnableSystem, transform.position, Quaternion.identity);
    }
    IEnumerator StartInvulnerability()
    {
        yield return new WaitForSeconds(0.5f);
        passedInvulnerability = true;
    }
    IEnumerator Shrink()
    {
        //every frame, shrink. Once you hit real low, destroy.
        yield return new WaitForFixedUpdate();
        transform.localScale *= 0.9f;
        if(transform.localScale.x < 0.01f)
        {
            Destroy(gameObject);
            if (transform.parent)
            {
                print(transform.parent);
            }
        }
        else
        {
            StartCoroutine(Shrink());
        }
    }
}
