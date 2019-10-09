using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    Rigidbody2D product;
    [SerializeField] float coolDown;
    public float launchForce;
    [SerializeField] Transform launchPoint;
    [SerializeField] LayerMask layerMask;
    OreController orePatch;
    SpriteRenderer sp;
    void Start()
    {
        StartCoroutine(LaunchItem());
        sp = gameObject.GetComponent<SpriteRenderer>();
        //make sure you dont add shit behind this, because it will error out and fail if the miner isnt on a patch
        if(Physics2D.OverlapCircle(transform.position, 1, layerMask))
        {
            orePatch = Physics2D.OverlapCircle(transform.position, 1, layerMask).gameObject.GetComponent<OreController>();
            product = orePatch.product;

        }
    }

    IEnumerator LaunchItem()
    {
        ///I think that any launcher which does not start with an item is failing rn because it errors and then stops
        ///I need it to only launch if there is an item in it. This is vital. Otherwise it stops the loop
        yield return new WaitForSeconds(coolDown);
        if (!orePatch)
        {
            product = null;
            sp.color = new Color(0.2f, 0.2f, 0.2f, 1);
        }
        if (product)
        {
            Rigidbody2D p = Instantiate(product, launchPoint.position, launchPoint.rotation);
            p.gameObject.SetActive(true);
            p.AddForce(launchPoint.up * launchForce);
            orePatch.currentQuantity -= 1;
            StartCoroutine(LaunchItem());
        }
    }
}
