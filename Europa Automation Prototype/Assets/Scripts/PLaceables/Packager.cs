using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packager : MonoBehaviour
{
    [SerializeField] CapsuleController eggPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask eggMask;
    public int launchForce = 3000;
    CapsuleController egg;
    public int maxItems = 50;
    bool launching = false;

    //inventory
    List<Item> items;
    private void Start()
    {
        StartCoroutine(OnBegin());
    }
    IEnumerator OnBegin()
    {
        yield return new WaitForSeconds(0.5f);
        //now check if u have an egg
        if (!egg)
        {
            //check if there is an egg nearby
            bool obj = Physics2D.OverlapCircle(firePoint.position, 4, eggMask);

            if (obj)
            {
                egg = Physics2D.OverlapCircle(firePoint.position, 1, eggMask).GetComponent<CapsuleController>();
                egg.transform.position = firePoint.position;
                egg.transform.rotation = firePoint.rotation;
            }
            else
            {
                egg = Instantiate(eggPrefab, firePoint.position, firePoint.rotation);
            }
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
                //if not launching, that means no egg is being created to replace. So, another must be made
                if (!launching)
                {
                    egg = Instantiate(eggPrefab, firePoint.position, firePoint.rotation);
                    egg.transform.parent = transform;
                    egg.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    egg.AddItem(other.GetComponent<Item>());
                }
            }
        }
    }

    IEnumerator LaunchEgg()
    {
        launching = true;
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
        launching = false;
    }

}
