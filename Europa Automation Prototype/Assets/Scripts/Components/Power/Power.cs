using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public Beam bPrefab;
    [SerializeField] float range = 20;
    [SerializeField] LayerMask mask;
    
    List<Beam> activeBeams;
    [Header("Power levels")]
    public float currentPower = 10;
    public int maxPower = 100;
    RaycastHit2D[] hits;

    [Header("Power handling")]
    [SerializeField] bool changer = false;
    public int changePerTick = 10;
    void Start()
    {
        activeBeams = new List<Beam>();
        //draw/produce
        if (changer)
        {
            StartCoroutine(Tick());
        }
        else
        {
            hits = new RaycastHit2D[6];
            activeBeams = new List<Beam>();

            Power[] pows = GameObject.FindObjectsOfType<Power>();
            foreach (Power pow in pows)
            {
                if (pow != this)
                {
                    MakeBeam(pow.transform);
                }
            }
        }
    }
    IEnumerator Tick()
    {
        yield return new WaitForSeconds(0.25f);
        //4 ticks p second
        if(activeBeams.Count > 0)
        {
            float increment = changePerTick / activeBeams.Count;
            print(increment);
            foreach (Beam b in activeBeams)
            {
                if(b.origin.maxPower < b.origin.currentPower + increment)
                {
                    b.origin.currentPower = b.origin.maxPower;
                }
                else if(b.origin.currentPower - increment < 0)
                {
                    b.origin.currentPower = 0;
                }
                else
                {
                    b.origin.currentPower += increment;
                }
            }
        }
        StartCoroutine(Tick());
    }
    void Update()
    {
        if (!changer)
        {
            foreach (Beam b in activeBeams)
            {
                //raycast from here to it's target

                int objectCount = Physics2D.RaycastNonAlloc(transform.position, (b.target.transform.position - b.origin.transform.position), hits, b.dist, mask, -100, 100);
                //if it hits more than two obj's (itself and tis target) then it's a nono
                //print(objectCount);

                if (objectCount < 3 && objectCount > 1)
                {
                    //its hitting 2 objs
                    BalancePower(b.target, this);
                    b.sp.color = new Color(0.9f, 0.9f, 0, 0.5f);
                }
                else
                {
                    //dont balance
                    b.sp.color = new Color(0.75f, 0.75f, 0, 0.15f);
                }
            }
        }       
    }

    public void MakeBeam(Transform target)
    {
        float dist = Vector3.Distance(target.position, transform.position);
        if (dist > range)
        {
            return;
        }
        Power targetPow = target.GetComponent<Power>();
        GameObject nB = Instantiate(bPrefab.gameObject, transform.position, Quaternion.identity);

        //rotation
        Vector3 aimDir = (target.position - transform.position);
        float angle = (Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg) + 90;

        Vector2 midpoint =  (target.position + transform.position) / 2;

        nB.transform.position = midpoint;
        nB.transform.localScale = new Vector3(25, dist * 100, 1);
        nB.transform.rotation = Quaternion.Euler(0, 0, angle);

        Beam theB = nB.GetComponent<Beam>();
        theB.target = targetPow;
        theB.origin = this;
        activeBeams.Add(theB);

        //if the other is a changer, add it to that ones thingy too
        if(targetPow.changer == true)
        {
            targetPow.activeBeams.Add(theB);
        }
    }

    void BalancePower(Power other, Power self)
    {
        //only run the statements if they are not balanced
        if(other.currentPower != self.currentPower)
        {
            float total = other.currentPower + self.currentPower;
            float avg = total / 2;

            if (other.maxPower < avg)
            {
                //then set it to max and add the rest to self
                float remainder = avg - other.maxPower;
                self.currentPower = avg + remainder;
                other.currentPower = avg - remainder;
            }
            else if (self.maxPower < avg)
            {
                //then set it to max and add the rest to other
                float remainder = avg - self.maxPower;
                other.currentPower = avg + remainder;
                self.currentPower = avg - remainder;
            }
            else
            {
                other.currentPower = avg;
                self.currentPower = avg;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (Beam b in activeBeams)
        {
            Destroy(b.gameObject);
        }
    }
}
