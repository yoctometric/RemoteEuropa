using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "ScriptableObjects/recipe", order = 1)]
public class ScripltableRecipe : ScriptableObject
{
    public List<string> input;
    public List<GameObject> output;
    
}
