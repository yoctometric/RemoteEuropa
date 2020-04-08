using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MetaInventory : MonoBehaviour
{
    TMP_Text display;
    public int fuel;

    private void Start()
    {
        fuel = PlayerPrefs.GetInt("MetaFuel");
        display = GameObject.FindGameObjectWithTag("FuelText").GetComponent<TMP_Text>();
        //update
        ModifyInventory(0);
    }

    public void ModifyInventory(int amount)
    {
        //set to zero just for updating render
        fuel += amount;
        PlayerPrefs.SetInt("MetaFuel", fuel);
        if (!display)
        {
            display = GameObject.FindGameObjectWithTag("FuelText").GetComponent<TMP_Text>();
        }
        display.text = "Total Launched Fuel: " + FuelAbbreviated();
    }

    public string FuelAbbreviated()
    {
        return StaticFunctions.AbbreviateNumber(fuel);
    }

    private void OnLevelWasLoaded(int level)
    {
        ModifyInventory(0);
    }
}
