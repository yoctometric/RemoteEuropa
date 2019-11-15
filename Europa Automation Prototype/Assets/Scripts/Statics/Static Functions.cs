using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class StaticFunctions
{

    public static float RoundTo(float value, float multipleOf)
    {
        return Mathf.Round(value / multipleOf) * multipleOf;
    }

    public static ScriptableRecipe GetRecipeFromIndex(int index)
    {
        ScriptableRecipe[] recipes = Resources.LoadAll<ScriptableRecipe>("Crafting");
        return recipes[index];
    }

    public static Item GetItemFromString(string input)
    {
        Item[] allItems = Resources.LoadAll<Item>("Items");
        Item nullItem = Resources.Load<Item>("Items/NullItem");
        for (int i = 0; i < allItems.Length; i++)
        {
            Debug.Log(allItems[i].name + ":" + input);
            //currently, input is always an empty string...
            if(allItems[i].name == input)
            {
                return allItems[i];
            }
        }
        //currently no functionality to handle a scenario where there is no  item in the crafter. Solution? IDK
        Debug.Log("RETURNING NULL BECAUSE FUCK yoU THATS WHY");
        return nullItem;
    }
    /*
        public static int GetMasterIntFromListOfItems(Item[] its)
        {
            //declare a dictionary
            Dictionary<int, int> typesAndAmounts = new Dictionary<int, int>();
            //now, iterate over every item in the input
            for (int i = 0; i < its.Length; i++)
            {
                //if the dictionary does not contain a key corresponding to the item's type...
                if (!typesAndAmounts.ContainsKey(its[i].typeInt))
                {
                    //add the key and set its value to one!
                    typesAndAmounts.Add(its[i].typeInt, 1);
                }
                else
                {
                    //otherwise, add one to the value of that key
                    typesAndAmounts[its[i].typeInt] = typesAndAmounts[its[i].typeInt] + 1;
                } 
            }
            //now make a string from the dictionary
            string masterString = "";
            //loop over every key
            foreach(KeyValuePair<int, int> key in typesAndAmounts)
            {
                //add the key as a string
                masterString += key.Key.ToString();
                //add the value as a string
                masterString += key.Value.ToString();
                Debug.Log(masterString);
            }
            //Afterwards, you now must convert the total string to an integer and return it.
            int masterInt = int.Parse(masterString);
            Debug.Log(masterInt);
            return masterInt;
        }
        */
}