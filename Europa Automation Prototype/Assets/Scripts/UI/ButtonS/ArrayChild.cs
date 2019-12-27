using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArrayChild : MonoBehaviour
{
    [HideInInspector] public UIButtonArray master;
    public void Activate()
    {
        master.BClick(gameObject.GetComponent<Button>());
    }
}
