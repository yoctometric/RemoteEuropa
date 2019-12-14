using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class Inventory : MonoBehaviour
{
    [SerializeField] List<TMP_Text> countDisplays;
    public Dictionary<string, int> storedVals = new Dictionary<string, int>();
    private void Start()
    {
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
        string disp = StaticFunctions.AbbreviateNumber(cAmnt + amount);
        UpdateDisplayTexts(index, disp);
        storedVals[index] = cAmnt + amount;
        print(storedVals["Refined Copper"]);
        return true;
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
            storedVals.Add("Refined Copper", 999);
        }
        if (!storedVals.ContainsKey("Refined Iron"))
        {
            storedVals.Add("Refined Iron", 200);
        }
        if (!storedVals.ContainsKey("Pycrete"))
        {
            storedVals.Add("Pycrete", 200);
        }
        if (!storedVals.ContainsKey("Brick"))
        {
            storedVals.Add("Brick", 200);
        }
        foreach (string key in storedVals.Keys.ToList())
        {
            bool yes = UpdateDisplayTexts(key, StaticFunctions.AbbreviateNumber(storedVals[key]));
        }
    }
}
