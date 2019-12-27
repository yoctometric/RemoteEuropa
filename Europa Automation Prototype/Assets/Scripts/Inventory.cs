using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField] List<TMP_Text> countDisplays;
    public Dictionary<string, int> storedVals = new Dictionary<string, int>();
    [HideInInspector]public Core core;
    [SerializeField] Button upButton;
    bool buttonOn = false;
    private void Start()
    {
        core = GameObject.FindObjectOfType<Core>();
        //storedVals.Add("Refined Copper", 0);
        //storedVals.Add("Refined Iron", 0);
        //storedVals.Add("Pycrete", 50);

        UpdateAllInventories();
        //UpdateDisplayTexts("Refined Copper", storedVals["Refined Copper"].ToString());
        //UpdateDisplayTexts("Refined Iron", storedVals["Refined Iron"].ToString());
        //UpdateDisplayTexts("Pycrete", storedVals["Pycrete"].ToString());

    }
    public bool UpdateInventory(string index, int amount)
    {
        int cAmnt = storedVals[index];

        //check if it would be negative
        if ((cAmnt + amount) < 0)
        {
            //flash the displays and break out
            if (index == "Refined Copper")
            {
                countDisplays[0].transform.parent.GetComponent<Animator>().SetTrigger("Flash");
            }
            else if (index == "Refined Iron")
            {
                countDisplays[1].transform.parent.GetComponent<Animator>().SetTrigger("Flash");

            }
            else if (index == "Pycrete")
            {
                countDisplays[2].transform.parent.GetComponent<Animator>().SetTrigger("Flash");
            }
            else if (index == "Brick")
            {
                countDisplays[3].transform.parent.GetComponent<Animator>().SetTrigger("Flash");
            }
            return false;
            
        }
        if(core.level < 2)
        {
            if (storedVals["Brick"] >= core.lvlCosts[core.level].w & storedVals["Refined Copper"] >= core.lvlCosts[core.level].x && storedVals["Refined Iron"] >= core.lvlCosts[core.level].y && storedVals["Pycrete"] >= core.lvlCosts[core.level].z)
            {
                if (!buttonOn)
                {
                    ToggleUpButton(true);
                }
            }
            else
            {
                ToggleUpButton(false);
            }
        }
        else
        {
            ToggleUpButton(false);
        }

        string disp = StaticFunctions.AbbreviateNumber(cAmnt + amount);
        UpdateDisplayTexts(index, disp);
        storedVals[index] = cAmnt + amount;
        return true;
    }
    public void ToggleUpButton(bool yesnt)
    {
        if (yesnt)
        {
            buttonOn = true;
            upButton.gameObject.SetActive(true);
            upButton.GetComponent<Animator>().SetBool("flash", true);
        }
        else
        {
            buttonOn = false;
            upButton.GetComponent<Animator>().SetBool("flash", false);
            upButton.gameObject.SetActive(false);
        }
    }
    public bool UpdateDisplayTexts(string type, string value)
    {
        //string noAbb = value;
        //noAbb = noAbb.Replace("k", "000");
        //noAbb = noAbb.Replace("m", "000000");
        //updates the inventories based on the type given. better than running it in update
        if (type == "Refined Copper")
        {
            countDisplays[0].text = value;
            //storedVals["Refined Copper"] = int.Parse(noAbb);
        }
        else if (type == "Refined Iron")
        {
            countDisplays[1].text = value;
            //storedVals["Refined Iron"] = int.Parse(noAbb);
        }
        else if (type == "Pycrete")
        {
            countDisplays[2].text = value;
            //storedVals["Pycrete"] = int.Parse(noAbb);
        }
        else if (type == "Brick")
        {
            countDisplays[3].text = value;
            //storedVals["Brick"] = int.Parse(noAbb);            
        }
        else
        {
            //solid error
            print("no inventory at index " + type.ToString());
            return false;
        }
        return true; 
    }
    public void UpdateAllInventories()
    {
        if(!storedVals.ContainsKey("Refined Copper"))
        {
            storedVals.Add("Refined Copper", 50);
        }
        if (!storedVals.ContainsKey("Refined Iron"))
        {
            storedVals.Add("Refined Iron", 50);
        }
        if (!storedVals.ContainsKey("Pycrete"))
        {
            storedVals.Add("Pycrete", 0);
        }
        if (!storedVals.ContainsKey("Brick"))
        {
            storedVals.Add("Brick", 100);
        }
        foreach (string key in storedVals.Keys.ToList())
        {
            bool yes = UpdateDisplayTexts(key, StaticFunctions.AbbreviateNumber(storedVals[key]));
        }
    }

    
}
