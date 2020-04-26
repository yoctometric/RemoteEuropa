using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MetaInventory : MonoBehaviour
{
    TMP_Text fDisplay;
    TMP_Text eDisplay;
    public int fuel;
    public int eternalizers;

    private void Start()
    {
        fuel = PlayerPrefs.GetInt("MetaFuel");
        eternalizers = PlayerPrefs.GetInt("MetaEternalizers");
        fDisplay = GameObject.FindGameObjectWithTag("FuelText").GetComponent<TMP_Text>();
        eDisplay = GameObject.FindGameObjectWithTag("EternalizerText").GetComponent<TMP_Text>();

        //update
        Setup();
    }

    
    void Setup()
    {
        ModifyInventory(0);
        ModifyEternalizers(0);
    }

    public void ModifyInventory(int amount)
    {
        //set to zero just for updating render
        fuel += amount;
        PlayerPrefs.SetInt("MetaFuel", fuel);
        if (!fDisplay)
        {
            fDisplay = GameObject.FindGameObjectWithTag("FuelText").GetComponent<TMP_Text>();
        }
        fDisplay.text = "Total Launched Fuel: " + FuelAbbreviated();
    }

    public void ModifyEternalizers(int amount)
    {
        eternalizers += amount;
        eternalizers = Mathf.Clamp(eternalizers, 0, 99999999);
        PlayerPrefs.SetInt("MetaEternalizers", eternalizers);

        eDisplay?.transform.parent.gameObject.SetActive(true);
        if (!eDisplay)
        {
            eDisplay = GameObject.FindGameObjectWithTag("EternalizerText").GetComponent<TMP_Text>();
        }
        if(eternalizers > 0)
        {
            eDisplay.text = "MOED: " + StaticFunctions.AbbreviateNumber(eternalizers);
        }
        else
        {
            eDisplay.text = "";
            eDisplay.transform.parent.gameObject.SetActive(false);
        }
    }

    public string FuelAbbreviated()
    {
        return StaticFunctions.AbbreviateNumber(fuel);
    }

    private void OnLevelWasLoaded(int level)
    {
        ModifyInventory(0);
        ModifyEternalizers(0);
    }
}
