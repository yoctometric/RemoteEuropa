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
    [SerializeField] GameObject mouseAimButton;
    [SerializeField] GameObject filterPanel;
    [SerializeField] GameObject rocketPanel;
    [SerializeField] SliderFamily rCopSlider;
    [SerializeField] SliderFamily rIronSlider;
    [SerializeField] SliderFamily rFuelSlider;
    //[SerializeField] TMP_Text[] rTexts;


    [SerializeField] TMP_Text MidInfoT;
    int typeSelected = 0;

    GameObject obj;
    Miner miner;
    LauncherController launcher;
    OreController ore;
    EditRotation eRot;
    Splitter split;
    Crafting craft;
    Pump pump;
    GameInfo gameInfo;
    Core core;
    RocketBase rocket;
    [SerializeField] List<GameObject> elements;
    private void Start()
    {
        //get gameinfo for hupcont prevention
    }
    public void Close()
    {
        ObjectPlacer op = GameObject.FindObjectOfType<ObjectPlacer>();
        op.CloseUI();
        //close all the elements on the hupcont to reset it.
        foreach (GameObject go in elements)
        {
            if (go)
            {
                go.SetActive(false);
            }
        }
    }
    public void ActivateMouseAim()
    {       
        if (eRot)
        {
            print("now do the mouse aiming");
            eRot.StartMouseAim();
            //handle the mouseaiming thru the erot
            
            Close();
        }
    }
    public void OpenEditor(GameObject editable)
    {
        //break out?
        gameInfo = GameObject.FindObjectOfType<GameInfo>();
        if (gameInfo.currentlyMouseAiming)
        {
            Close();
            return;
        }
        //activate cursor
        Cursor.visible = true;

        //get editable
        obj = editable.transform.parent.gameObject;
        if (editable.GetComponent<EditRotation>())
        {
            //activate mouseaim if...
            mouseAimButton.SetActive(true);
            eRot = editable.GetComponent<EditRotation>();
            elements[4].SetActive(true);
        }
        else
        {
            mouseAimButton.SetActive(false);
            eRot = null;
            //so that you never end up doing it for something else
        }
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
            s2T.text = "Fire Interval: " + s2.value.ToString();
        }
        if (obj.GetComponent<OreController>())
        {
            typeSelected = 3;
            ore = obj.GetComponent<OreController>();
            MidInfoT.gameObject.SetActive(true);
            MidInfoT.text = "Quantity: " + ore.currentQuantity.ToString() + System.Environment.NewLine + "Hardness: " + ore.hardness;
            MidInfoT.color = ore.GetComponent<SpriteRenderer>().color;
        }
        if (obj.GetComponent<Crafting>())
        {
            //set up crafter ui
            typeSelected = 4;
            crafterPanel.SetActive(true);
            craft = obj.GetComponent<Crafting>();

            UIButtonArray car = crafterPanel.GetComponent<UIButtonArray>();
            foreach (Button b in car.buttons)
            {
                ///THIS
                ///MEANS
                ///THAT
                ///ALL
                ///BUTTONS
                ///MUST BE NAMED
                ///P R O P E R L Y !!!
                //print(b.name + " : " + craft.recipe.name);
                if (b.name == craft.recipe.name)
                {
                    car.BClick(b);
                }
            }
        }
        if (obj.GetComponent<UnPackager>())
        {
            typeSelected = 5;
        }
        if (obj.GetComponent<Packager>())
        {
            Packager pack = obj.GetComponent<Packager>();
            typeSelected = 6;
            s1.gameObject.SetActive(true);
            s1.maxValue = 100;
            s1.minValue = 10;
            s1.value = pack.maxItems;
            s1T.text = "Maximum Items: " + Mathf.RoundToInt(s1.value).ToString();
        }else if (obj.GetComponent<Splitter>())
        {
            split = obj.GetComponent<Splitter>();
            typeSelected = 7;
            MidInfoT.gameObject.SetActive(true);
            MidInfoT.text = "Set a filter ->";
            MidInfoT.color = new Color(0, 255, 255, 255);
            filterPanel.gameObject.SetActive(true);
            //setup filter panel
            UIButtonArray ar = filterPanel.GetComponent<UIButtonArray>();
            foreach (Button b in ar.buttons)
            {
                ///THIS
                ///MEANS
                ///THAT
                ///ALL
                ///BUTTONS
                ///MUST BE NAMED
                ///P R O P E R L Y !!!
                if(b.name == split.typeName)
                {
                    ar.BClick(b);
                }
                else
                {
                    if(b.name == "Empty filter" && split.typeName == "")
                    {
                        ar.BClick(b);
                    }
                }
            }
        }else if (obj.GetComponent<Pump>())
        {
            pump = obj.GetComponent<Pump>();
            MidInfoT.gameObject.SetActive(true);
            typeSelected = 8;
        }else if (obj.GetComponent<Core>())
        {
            typeSelected = 9;
            core = obj.GetComponent<Core>();
            int lv = core.level;
            MidInfoT.gameObject.SetActive(true);
            MidInfoT.text = "Level " + lv.ToString();
        }else if (obj.GetComponent<RocketBase>())
        {
            typeSelected = 10;
            rocket = obj.GetComponent<RocketBase>();
            MidInfoT.gameObject.SetActive(true);
            rocketPanel.SetActive(true);
            rIronSlider.slider.maxValue = rocket.maxIron;
            rCopSlider.slider.maxValue = rocket.maxCopper;
            rFuelSlider.slider.maxValue = rocket.maxFuel;
            MidInfoT.text = "Needs rocket fuel";
            //set max
            rIronSlider.SetSliderParameters(rocket.maxIron);
            rCopSlider.SetSliderParameters(rocket.maxCopper);
            rFuelSlider.SetSliderParameters(rocket.maxFuel);
        }
    }
    public void SetFilter(string input)
    {
        if (split)
        {
            split.typeName = input;
        }
        else
        {
            print("wetf why no split");
        }
    }
    private void Update()
    {
        if(typeSelected == 3)
        {
            MidInfoT.text = "Quantity: " + ore.currentQuantity.ToString() + System.Environment.NewLine + "Hardness: " + ore.hardness;
        }else if(typeSelected == 8)
        {
            if(pump.amntContainersStored > 0)
            {
                MidInfoT.text = "Stored Barrels: " + pump.amntContainersStored.ToString();
            }
            else
            {
                MidInfoT.text = "Needs empty barrel";
            }
        }else if (typeSelected == 10)
        {
            rCopSlider.ChangeValue(rocket.storedCopper);
            rIronSlider.ChangeValue(rocket.storedIron);
            rFuelSlider.ChangeValue(rocket.storedFuel);
            //rTexts[0].text = rocket.storedCopper.ToString();
            //rTexts[1].text = rocket.storedIron.ToString();
            //rTexts[2].text = rocket.storedFuel.ToString();

            if (rocket.storedFuel < 1)
            {
                MidInfoT.text = "Needs rocket fuel";
            }
            else
            {
                float percent = StaticFunctions.RoundTo(((((float)rocket.storedFuel + (float)rocket.storedIron + (float)rocket.storedCopper) / (float)rocket.totalMax) * 100f), 2);
                MidInfoT.text = "Rocket is "+ percent.ToString() +"% complete";
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
            Cursor.visible = false;
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
        if (typeSelected == 6)
        {
            obj.GetComponent<Packager>().maxItems = Mathf.RoundToInt(s1.value);
            s1T.text = "Maximum Items: " + s1.value.ToString();
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
            s2T.text  = "Fire Interval: " + sliderVal.ToString();
        }
        if(typeSelected == 4)
        {
            s2T.text = "Set a recipe";
        }

    }

    public void SetCrafterRecipe(ScriptableRecipe rec)
    {
        if(typeSelected == 4)   
        {
            //instead of directly setting the recipe, call a function on the crafter so that I can activate other shit when it changes recipe.
            if (obj)
            {
                obj.GetComponent<Crafting>().ChangeRecipe(rec);
            }
        }
        else
        {
            //if you don't have a crafter selected, say "No crater selected. This message should never be seen"
            print("No crafter selected. This message should never be seen");
        }
    }
}
