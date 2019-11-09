using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packager : MonoBehaviour
{
    [SerializeField] CapsuleController eggPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float launchForce = 1000;
    CapsuleController egg;
    public int maxItems = 50;

    //inventory
    List<Item> items;
    private void Start()
    {
        //now check if u have an egg
        if (!egg)
        {
            egg = Instantiate(eggPrefab, firePoint.position, firePoint.rotation);
            egg.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            egg.transform.parent = transform;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Item>())
        {
            //so its an item...

            //okay now load egg with item
            if (egg)
            {
                if (egg.items.Count < maxItems)
                {
                    //print(other.name);
                    egg.AddItem(other.GetComponent<Item>());
                }
                else
                {
                    StartCoroutine(LaunchEgg());
                }
            }
            else
            {
                //if no egg, reject
                other.GetComponent<Rigidbody2D>().AddForce(firePoint.up * -200);
            }
        }
    }

    IEnumerator LaunchEgg()
    {
        egg.transform.parent = null;
        Rigidbody2D r = egg.GetComponent<Rigidbody2D>();
        r.constraints = RigidbodyConstraints2D.None;
        egg = null;
        yield return new WaitForSeconds(0.25f);
        r.AddForce(firePoint.up * launchForce);
        //r.AddTorque(250);
        //now make an egg
        yield return new WaitForSeconds(0.5f);
        egg = Instantiate(eggPrefab, firePoint.position, firePoint.rotation);
        egg.transform.parent = transform;
        egg.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

}
