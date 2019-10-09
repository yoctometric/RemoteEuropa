using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Game
{
    public static Game current;
    public List<GameObject> objects;
    public List<int> resources;
}
