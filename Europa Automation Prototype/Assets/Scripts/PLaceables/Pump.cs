using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pump : MonoBehaviour
{
    public string inputType = "Barrel";
    [SerializeField] Item product;
    [SerializeField] Transform firePoint;
    [SerializeField] int fireForce = 500;
    [SerializeField] float coolDown = 1;
    [HideInInspector] public int amntContainersStored = 0;
    [SerializeField] int maxContained = 10;
    [SerializeField] GameObject emptyIndicator;
    [SerializeField] Animator anim;
    Timer tim;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Item>())
        {
            //check if barrel
            Item o = other.GetComponent<Item>();
            if(o.typeOfItem == inputType)
            {
                if(amntContainersStored < maxContained)
                {
                    amntContainersStored += 1;
                    Destroy(o.gameObject);
                }

            }
            //update indic
            if(amntContainersStored != 0)
            {
                emptyIndicator.SetActive(false);
            }
        }
    }
    public void Tick(int iter)
    {
        
        //update indicator before launch so that it only shows empty after it tries to launch on empty.
        if (iter % Mathf.RoundToInt(coolDown) == 0)
        {
            if (amntContainersStored < 1)
            {
                emptyIndicator.SetActive(true);
            }
            if (amntContainersStored > 0 && this != null)
            {
                Item p = Instantiate(product.gameObject, firePoint.position, firePoint.rotation).GetComponent<Item>();
                p.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce);
                amntContainersStored -= 1;
                //color green
                anim.SetTrigger("go");
            }
        }
    }
    private void Start()
    {
        tim = GameObject.FindObjectOfType<Timer>();
        tim.pumps.Add(this);
        if (amntContainersStored != 0)
        {
            emptyIndicator.SetActive(false);
        }
        else
        {
            emptyIndicator.SetActive(true);
        }
    }
    private void OnDestroy()
    {
        tim.pumps.Remove(this);
    }
}
