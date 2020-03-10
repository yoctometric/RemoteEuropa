using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBase : MonoBehaviour
{
    [HideInInspector] public int storedIron = 0;
    [HideInInspector] public int storedCopper = 0;
    [HideInInspector] public int storedFuel = 0;

    [SerializeField] public int maxIron = 500;
    [SerializeField] public int maxCopper = 500;
    [SerializeField] public int maxFuel = 500;

    [HideInInspector] public int totalMax = 0;

    [Header("Various refrences")]
    [SerializeField] Animator hatch;
    [SerializeField] Animator rocket;
    //item admittence
    private void Start()
    {
        totalMax = maxCopper + maxIron + maxFuel;
    }
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
            else
            {
                //reject item
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

        hatch.SetTrigger("Open");
        yield return new WaitForSeconds(1f);
        rocket.SetTrigger("Launch");
        GameObject.FindObjectOfType<MetaInventory>()?.ModifyInventory(storedFuel);

        storedIron = 0; storedFuel = 0; storedCopper = 0;

    }
    void LaunchRocket()
    {
        StartCoroutine(LaunchLogic());
    }
}
