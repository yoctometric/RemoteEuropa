using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    Rigidbody2D product;
    [SerializeField] float coolDown = 4;
    //int cTicks = 0;
    bool canGo = true;
    float hardnessMultiplier = 1;
    public float launchForce;
    [SerializeField] Transform launchPoint;
    [SerializeField] LayerMask layerMask;
    OreController orePatch;
    SpriteRenderer sp;
    Timer tim;
    void Start()
    {
        //get the timer
        tim = GameObject.FindObjectOfType<Timer>();
        tim.miners.Add(gameObject.GetComponent<Miner>());
        StartCoroutine(LaunchItem());
        sp = gameObject.GetComponent<SpriteRenderer>();
        //make sure you dont add shit behind this, because it will error out and fail if the miner isnt on a patch
        if(Physics2D.OverlapCircle(transform.position, 1, layerMask))
        {
            orePatch = Physics2D.OverlapCircle(transform.position, 1, layerMask).gameObject.GetComponent<OreController>();
            product = orePatch.product.GetComponent<Rigidbody2D>();
            hardnessMultiplier = orePatch.hardness;
        }
    }
    /*
    private void FixedUpdate()
    {
        //timer. This method is extremely laggy. Big problem
        float mod = 1;
        if(hardnessMultiplier != null)
        {
            mod = hardnessMultiplier;
        }
        float div = Mathf.RoundToInt(Time.time * 10 * mod);
        float divA = div % coolDown;
        print(div.ToString() + ' ' + divA.ToString());
        if (divA == 0)
        {
            if (canGo)
            {
                canGo = false;
                StartCoroutine(LaunchItem());
            }
        }
        else
        {
            canGo = true;
        }
    }
    */
    public void Tick(int tick)
    {
        //setup modifier
        float mod = 1;
        if (hardnessMultiplier != null)
        {
            mod = hardnessMultiplier;
        }
        //now, if tick is divisible by cooldown
        if(tick % Mathf.RoundToInt(mod * coolDown) == 0)
        {
            StartCoroutine(LaunchItem());
        }
        /*
        print("ticked" + Mathf.RoundToInt(mod * coolDown).ToString());

        //check ticks
        if (cTicks == Mathf.RoundToInt(mod * coolDown))
        {
            cTicks = 0;
            StartCoroutine(LaunchItem());
        }
        else
        {
            //cncrement
            cTicks += 1;
        }
        */
    }
    IEnumerator LaunchItem()
    {
        ///I think that any launcher which does not start with an item is failing rn because it errors and then stops
        ///I need it to only launch if there is an item in it. This is vital. Otherwise it stops the loop
        //yield return new WaitForSeconds(coolDown * hardnessMultiplier);
        yield return new WaitForEndOfFrame();
        if (!orePatch)
        {
            //product = null;
            sp.color = new Color(0.2f, 0.2f, 0.2f, 1);
            orePatch = Physics2D.OverlapCircle(transform.position, 1, layerMask).gameObject.GetComponent<OreController>();
            if (orePatch)
            {
                hardnessMultiplier = orePatch.hardness;

                product = orePatch.product.GetComponent<Rigidbody2D>();
            }
            //StartCoroutine(LaunchItem());
        }else
        {
            Rigidbody2D p = Instantiate(product, launchPoint.position, launchPoint.rotation);
            p.gameObject.SetActive(true);
            p.AddForce(launchPoint.up * launchForce);
            orePatch.currentQuantity -= 1;
            sp.color = Color.white;
            //StartCoroutine(LaunchItem());
        }
    }

    //remove miner from list
    private void OnDestroy()
    {
        tim.miners.Remove(gameObject.GetComponent<Miner>());
    }
}
