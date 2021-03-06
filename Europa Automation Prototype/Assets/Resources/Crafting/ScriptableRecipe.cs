﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "ScriptableObjects/recipe", order = 1)]
[System.Serializable]
public class ScriptableRecipe : ScriptableObject
{
    [SerializeField] public List<string> input;
    [SerializeField] public List<GameObject> output;
    [SerializeField] public Sprite img;
    [SerializeField] public Color imgColor;
    [SerializeField] public int id = 0;
}
