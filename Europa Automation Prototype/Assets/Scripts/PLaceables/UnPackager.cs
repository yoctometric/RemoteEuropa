using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnPackager : MonoBehaviour
{
    public float fireDelay = 1;
    public float fireForce = 500;
    [SerializeField] Transform holdingPoint;
    [SerializeField] Transform launchPoint;
    [SerializeField] ParticleSystem eggParticle;

    Animator anim;
    CapsuleController egg = null;

    public List<GameObject> storedItems;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(LaunchItem());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //when hit by a capsule, say this 
        if (other.GetComponent<CapsuleController>())
        {
            egg = other.GetComponent<CapsuleController>();

            //THIS IS WHERE THE ITEMS ARE DISSAPEARING WHYYYYYYYYYY
            //FUTURE SELF FIGURE IT OUT NERDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
            //activaate anims

            anim.SetTrigger("Grab");
            //snap egg
            egg.transform.position = holdingPoint.transform.position;

            egg.transform.rotation = holdingPoint.transform.rotation;
            //freeze egg

            egg.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            egg.Unload(gameObject.GetComponent<UnPackager>());
            StartCoroutine(ReturnEgg());
        }
    }
    IEnumerator ReturnEgg()
    {
        yield return new WaitForSeconds(1);
        if (!egg)
        {
            yield break;
        }
        if(egg.items.Count > 0)
        {
            StartCoroutine(ReturnEgg());
        }
        else
        {
            egg.GetComponent<Animator>().SetTrigger("Close");
            anim.SetTrigger("Throw");
            yield return new WaitForSeconds(0.5f);
            //wait a sec, throw, then close egg
            //unfreeze egg
            Rigidbody2D e = egg.GetComponent<Rigidbody2D>();
            e.constraints = RigidbodyConstraints2D.None;
            e.AddForce(holdingPoint.right * -fireForce * 2);
            yield return new WaitForSeconds(0.05f);
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForEndOfFrame();
                if (egg)
                {
                    egg.transform.localScale *= 0.9f;
                    if (egg.transform.localScale.x < 0.1f)
                    {
                        break;
                    }
                }

            }
            if (egg)
            {
                Transform part = Instantiate(eggParticle, egg.transform.position, Quaternion.identity).transform;
                Destroy(egg.gameObject);
            }


            //times 5 becuz of hge mass of egg
            egg = null;
        } 
    }

    IEnumerator LaunchItem()
    {
        yield return new WaitForSeconds(fireDelay);
        if (egg)
        {
            if (egg.items.Count > 0)
            {
                //activate, move, launch, and remove item
                Item it = egg.items[0].GetComponent<Item>();
                it.gameObject.SetActive(true);
                it.transform.position = launchPoint.transform.position;
                it.GetComponent<Rigidbody2D>().AddForce(launchPoint.up * fireForce);
                egg.items.RemoveAt(0);
            }
        }
        
        StartCoroutine(LaunchItem());
    }
}
