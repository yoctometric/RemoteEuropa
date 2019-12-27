using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pump : MonoBehaviour
{
    public string inputType = "Barrel";
    [SerializeField] Item product;
    [SerializeField] Transform firePoint;
    [SerializeField] int fireForce = 500;
    [SerializeField] float fireRate = 1;
    [SerializeField] int amntContainersStored = 0;
    [SerializeField] int maxContained = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Item>())
        {
            //check if barrel
            Item o = other.GetComponent<Item>();
            if(o.typeOfItem == inputType)
            {
                print("ADDING");
                if(amntContainersStored < maxContained)
                {
                    amntContainersStored += 1;
                    Destroy(o.gameObject);
                }
                else
                {
                    print("REJECT");
                }
            }
            else
            {
                print("REJECT");
            }
        }
    }
    public void Tick()
    {
        print(amntContainersStored);
        if (amntContainersStored > 0)
        {
            Item p = Instantiate(product.gameObject, firePoint.position, firePoint.rotation).GetComponent<Item>();
            p.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce);
            amntContainersStored -= 1;
            //color green
        }
        else
        {
            //color red
        }

    }
    private void Start()
    {
        Timer tim = GameObject.FindObjectOfType<Timer>();
        tim.pumps.Add(this);
    }
}
