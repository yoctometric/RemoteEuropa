using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Core : MonoBehaviour
{
    Inventory invent;
    ///Dictionary<string, int> inventory = new Dictionary<string, int>();
    void Start()
    {
        invent = GameObject.FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Item>())
        {

            Item it = other.GetComponent<Item>();
            string itemType = it.typeOfItem;
            /*
            if (inventory.TryGetValue(itemType, out int value))
            {
                inventory[itemType] += 1;
                Destroy(it.gameObject, 0.1f);
            }
            else
            {
                inventory.Add(itemType, 1);
                Destroy(it.gameObject, 0.1f);
            }*/
            //string val = StaticFunctions.AbbreviateNumber(inventory[itemType]);
            if (invent.storedVals.Keys.ToList().Contains(itemType))
            {
                invent.UpdateInventory(itemType, 1);
                Destroy(it.gameObject);
            }
        }
    }
}
