using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//this is for the Tolist()function
using System.Linq;

public class HUPCONT : MonoBehaviour
{
    [SerializeField] TMP_Text name;
    [SerializeField] Slider s1;
    [SerializeField] Slider s2;
    [SerializeField] TMP_Text s1T;
    [SerializeField] TMP_Text s2T;
    [SerializeField] GameObject crafterPanel;

    [SerializeField] TMP_Text MidInfoT;
    int typeSelected = 0;

    GameObject obj;
    Miner miner;
    LauncherController launcher;
    OreController ore;

    public void OpenEditor(GameObject editable)
    {
        //get editable
        obj = editable.transform.parent.gameObject;
        //make the title of the ui the obj name
        string objName = obj.name;
        //if it has a (, remove everything after that (
        if (objName.Contains("("))
        {
            int loc = objName.IndexOf("(");
            //substring does the trick
            objName = objName.Substring(0, loc);
            name.text = objName;
        }
        else
        {
            name.text = objName;
        }

        if (obj.GetComponent<Miner>())
        {
            //set type
            typeSelected = 1;
            //set up layout for miner obj
            miner = obj.GetComponent<Miner>();
            s1.gameObject.SetActive(true);
            s1.maxValue = 1000;
            s1.value = miner.launchForce;
            s1T.text = "Launch Force: " + s1.value.ToString();
        }
        if (obj.GetComponent<LauncherController>())
        {
            //set type
            typeSelected = 2;
            //set up layout for launcher obj
            launcher = obj.GetComponent<LauncherController>();
            s1.gameObject.SetActive(true);
            s1.maxValue = 1000;
            s1.value = launcher.launchForce;
            s1T.text = "Launch Force: " + s1.value.ToString();
            //set up second slider for interval
            s2.gameObject.SetActive(true);
            s2.maxValue = 2;
            s2.minValue = 0.25f;
            s2.value = launcher.coolDown;
            s2T.text = "Interval: " + s2.value.ToString();
        }
        if (obj.GetComponent<OreController>())
        {
            typeSelected = 3;
            ore = obj.GetComponent<OreController>();
            MidInfoT.gameObject.SetActive(true);
            MidInfoT.text = "Quantity: " + ore.currentQuantity.ToString();
            MidInfoT.color = ore.GetComponent<SpriteRenderer>().color;
        }
        if (obj.GetComponent<Crafting>())
        {
            //set up crafter ui
            typeSelected = 4;
            crafterPanel.SetActive(true);

        }
    }
    private void Update()
    {
        if(typeSelected == 3)
        {
            MidInfoT.text = "Quantity: " + ore.currentQuantity.ToString();
        }
    }
    //allows slider one to set values
    public void SetValueSlider1()
    {
        if(typeSelected == 1)
        {
            obj.GetComponent<Miner>().launchForce = s1.value;
            s1T.text = "Launch Force: " + s1.value.ToString();
        }
        if(typeSelected == 2)
        {
            obj.GetComponent<LauncherController>().launchForce = s1.value;
            s1T.text = "Launch Force: " + s1.value.ToString();
        }
    }
    //allows slider 2 to set values
    public void SetValueSlider2()
    {
        float sliderVal = StaticFunctions.RoundTo(s2.value, 0.25f);


        if (typeSelected == 1)
        {

        }
        if (typeSelected == 2)
        {
            obj.GetComponent<LauncherController>().coolDown = sliderVal;
            s2T.text  = "Interval: " + sliderVal.ToString();
        }
    }

    public void SetCrafterRecipe(ScriptableRecipe rec)
    {
        if(typeSelected == 4)
        {



            //instead of directly setting the recipe, call a function on the crafter so that I can activate other shit when it changes recipe.
            obj.GetComponent<Crafting>().ChangeRecipe(rec);
        }
        else
        {
            //if you don't have a crafter selected, say "No crater selected. This message should never be seen"
            print("No crafter selected. This message should never be seen");
        }


    }
}
