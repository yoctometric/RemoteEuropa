using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<TMP_Text> countDisplays;

    public string AbbreviateNumber(int number)
    {
        string abb = number.ToString();
        //abbreviates integers based on k, mil
        if(number >= 1000000)
        {
            abb = abb.Substring(0, abb.Length-6) + "m";
        }else if (number >= 1000)
        {
            abb = abb.Substring(0, abb.Length-3) + "k";
        }
        else
        {
        }
        print(abb);
        return abb;

    }

    public void UpdateDisplayTexts(string type, string value)
    {
        //updates the inventories based on the type given. better than running it in update
        print(value);
        if(type == "Refined Copper")
        {
            countDisplays[0].text = value;
        }
        else if (type == "Refined Iron")
        {
            countDisplays[1].text = value;
        }
        else if (type == "Pycrete")
        {
            countDisplays[2].text = value;
        }
        else
        {
            //solid error
            print("no inventory at index " + type.ToString());
        }
    }
}
