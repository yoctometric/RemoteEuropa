using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBase : MonoBehaviour
{
    [HideInInspector] public int storedIron = 0;
    [HideInInspector] public int storedCopper = 0;
    [HideInInspector] public int storedFuel = 0;

    [SerializeField] int maxIron = 500;
    [SerializeField] int maxCopper = 500;
    [SerializeField] int maxFuel = 500;

    [Header("Various refrences")]
    [SerializeField] Animator hatch;
    [SerializeField] Animator rocket;
    //item admittence
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Item>())
        {
            //now what type is it
            Item it = other.GetComponent<Item>();
            if(it.typeOfItem == "Refined Copper" && storedCopper < maxCopper)
            {
                storedCopper++;
                Destroy(it.gameObject);
            }
            else if (it.typeOfItem == "Refined Iron" && storedIron < maxIron)
            {
                storedIron++;
                Destroy(it.gameObject);
            }
            else if (it.typeOfItem == "Rocket Fuel" && storedFuel < maxFuel)
            {
                storedFuel++;
                Destroy(it.gameObject);
            }
        }
        //Now, check if cann launch
        if(storedIron == maxIron && storedCopper == maxCopper && storedFuel == maxFuel)
        {
            LaunchRocket();
        }
    }
    IEnumerator LaunchLogic()
    {
        storedIron = 0; storedFuel = 0; storedCopper = 0;

        hatch.SetTrigger("Open");
        yield return new WaitForSeconds(1f);
        rocket.SetTrigger("Launch");

    }
    void LaunchRocket()
    {
        StartCoroutine(LaunchLogic());
    }
}
