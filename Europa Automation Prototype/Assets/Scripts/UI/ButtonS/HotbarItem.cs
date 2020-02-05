using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HotbarItem : MonoBehaviour
{
    [SerializeField] int index;
    ObjectPlacer cursor;
    void Start()
    {
        cursor = GameObject.FindObjectOfType<ObjectPlacer>();
    }
    public void Click()
    {
        cursor.SetIndex(index);
    }
}
