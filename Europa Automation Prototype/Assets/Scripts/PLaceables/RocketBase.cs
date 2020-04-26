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

    AudioSource aud;
    MetaInventory metaInventory;
    private void Start()
    {
        metaInventory = GameObject.FindObjectOfType<MetaInventory>();
        totalMax = maxCopper + maxIron + maxFuel;
        aud = gameObject.GetComponent<AudioSource>();
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
        GameObject.FindObjectOfType<GameConsole>()?.AddLine("Rocket launched. Command has allocated you a MOED, use it as you see best");
        int temp = storedFuel;
        storedIron = 0; storedFuel = 0; storedCopper = 0;

        hatch.SetTrigger("Open");
        aud.Play();
        yield return new WaitForSeconds(1f);
        rocket.SetTrigger("Launch");
        metaInventory.ModifyInventory(temp);
        metaInventory.ModifyEternalizers(1);//give the player a new etenalizer

    }
    void LaunchRocket()
    {
        StartCoroutine(LaunchLogic());
    }
}
