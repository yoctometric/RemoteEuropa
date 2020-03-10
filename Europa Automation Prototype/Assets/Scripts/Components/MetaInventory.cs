using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaInventory : MonoBehaviour
{

    private void Start()
    {
        fuel = PlayerPrefs.GetInt("MetaFuel");
    }

    public int fuel;

    public void ModifyInventory(int amount)
    {
        fuel += amount;
        PlayerPrefs.SetInt("MetaFuel", fuel);
    }
    public string FuelAbbreviated()
    {
        return StaticFunctions.AbbreviateNumber(fuel);
    }
}
