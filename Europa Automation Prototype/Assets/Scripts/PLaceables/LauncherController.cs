using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour
{
    public List<Rigidbody2D> itemsInLauncher;
    [SerializeField] Transform launchPoint;
    public Transform launchRotationVertex; //this is pretty much only for saving and loading right here
    [SerializeField] Transform capacityIndicator;
    SpriteRenderer capacitySP;
    public float coolDown = 1;
    public float launchForce = 500;
    public int maxCapacity = 4;
    private void Start()
    {
        StartCoroutine(LaunchItem());
        capacitySP = capacityIndicator.GetComponentInChildren<SpriteRenderer>();
    }

    public void AddItemToStorage(Rigidbody2D item)
    {
        //only add items if under max capacity. If over, spit it back out
        if(itemsInLauncher.Count < maxCapacity)
        {
            itemsInLauncher.Add(item.gameObject.GetComponent<Rigidbody2D>());
            item.gameObject.SetActive(false);
            UpdateIndicator();
        }
        else
        {
            item.AddForce(transform.right * -100);
        }

    }
    private void UpdateIndicator()
    {
        //update the storage indicator
        //this should be based on percentage, so that no matter the max capacity it will still work properly
        if(itemsInLauncher.Count == 0)
        {
            //this if statement prevents divide by zero errors
            capacityIndicator.localScale = new Vector3(0, 1, 1);
        }
        else if(itemsInLauncher.Count == maxCapacity)
        {
            capacityIndicator.localScale = new Vector3(1, 1, 1);
            capacitySP.color = new Color(1, 0, 0, 1);
        }
        else 
        {
            //why doesnt this work?
            float percentFilled = 1.25f - (1 / (float)itemsInLauncher.Count);
            capacityIndicator.localScale = new Vector3(percentFilled, 1f, 1f);
            capacitySP.color = new Color(percentFilled, 1, 0, 1);
        }
    }
    IEnumerator LaunchItem()
    {
        ///I think that any launcher which does not start with an item is failing rn because it errors and then stops
        ///I need it to only launch if there is an item in it. This is vital. Otherwise it stops the loop
        yield return new WaitForSeconds(coolDown);
        
        if(itemsInLauncher.Count > 0)
        {
            Rigidbody2D item = itemsInLauncher[0];
            item.transform.position = launchPoint.position;
            item.transform.rotation = launchPoint.rotation;
            item.gameObject.SetActive(true);
            item.AddForce(launchPoint.up * launchForce);
            itemsInLauncher.RemoveAt(0);
            UpdateIndicator();
        }

        StartCoroutine(LaunchItem());

    }
}
