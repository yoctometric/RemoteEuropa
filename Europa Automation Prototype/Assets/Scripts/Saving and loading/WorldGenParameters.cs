using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WorldGenParameters : MonoBehaviour
{
    ///Parameters are all saved to one string which can be copy pasted to share with others
    ///Parameters are passed to world gen on scene transition
    ///Parameters are edited with slider family?
    ///all parameter names is 3 chars

    [SerializeField] TMP_InputField genStringBox;
    string defaultString = "ore:1000,irW:3,coW:3,icW:6,rkW:6,dis:1,";
    [HideInInspector]public string genString = "";

    [SerializeField] Color badColor = Color.red;
    Color defaultColor;

    [SerializeField] WorldGenParameterContainer container;

    private void Start()
    {
        defaultColor = genStringBox.targetGraphic.color;
        genString = defaultString;
        genStringBox.text = genString; // render it
    }

    public void UpdateFromFamily(SliderInputFamily fam)
    {
        UpdateString(fam.name, fam.storedVal);
    }


    int ValueFromString(string type)
    {
        int value = 1;

        //break out if string does not contain fam name
        if (!genString.Contains(type))
        {
            return - 1;
        }
        //print("Updating " + fam.name);
        string set = genString.Substring(genString.IndexOf(type));
        set = set.Substring(0, set.IndexOf(','));
        if (int.TryParse(set.Split(':')[1], out value))
        {
            return value;
        }
        else
        {
            return -1;
        }
    }
    void UpdateAllFamilies()
    {
        //note: can cause integer overflow if user types too much
        foreach(SliderInputFamily fam in gameObject.GetComponentsInChildren<SliderInputFamily>())
        {
            int val = ValueFromString(fam.name);
            if(val == -1)
            {
                SetColor(false);
                fam.UpdateValuesFromOutsideSource(0); //invalid value, reset to zero
                UpdateString(fam.name, 0);//and here too
                return;
            }
            else
            {
                fam.UpdateValuesFromOutsideSource(val); //valid value, hit it up
            }
            //succeded
            SetColor(true);
        }
    }

    void UpdateString(string type, float amount)
    {
        //check if it exists
        if (!genString.Contains(type))
        {
            return; // break out
        }
        //find index of type
        int spot = genString.IndexOf(type);
        string originalValueSet = genString.Substring(spot);//cuts string from the data input to the end
        originalValueSet = originalValueSet.Substring(0, originalValueSet.IndexOf(','));//ends substring at first comma seperator
        string replacement = originalValueSet.Substring(0, originalValueSet.IndexOf(":") + 1);
        replacement += amount.ToString();
        //now replace it
        genString = genString.Replace(originalValueSet, replacement);
        genStringBox.text = genString;
    }

    public void SetFromInputField(TMP_InputField field)
    {
        genString = field.text;
        UpdateAllFamilies();
    }

    public void ResetString(string input)
    {
        if(input == "default")
        {
            genString = defaultString;
        }
        else
        {
            genString = input;
        }
        genStringBox.text = genString;
        UpdateAllFamilies();
    }

    public void Generate()
    {
        //save vals
        WorldGenParameterContainer data =  Instantiate(container, transform.position, Quaternion.identity);
        DontDestroyOnLoad(data.gameObject);
        data.AttemptedOres = ValueFromString("ore");
        data.ironWeight = ValueFromString("irW");
        data.copperWeight = ValueFromString("coW");
        data.rockWeight = ValueFromString("rkW");
        data.iceWeight = ValueFromString("icW");
        data.distanceBonus = ValueFromString("dis");
        //now GO!
        GameObject.FindObjectOfType<MainMenuCont>().LoadAScene(2);
    }

    void SetColor(bool good)
    {
        if (good)
        {
            genStringBox.targetGraphic.color = defaultColor;
        }
        else
        {
            genStringBox.targetGraphic.color = badColor;
        }
    }
}
