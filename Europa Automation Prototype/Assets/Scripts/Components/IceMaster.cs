using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMaster : MonoBehaviour
{
    [SerializeField] GameObject bigG;
    [SerializeField] GameObject smolG;
    void Start()
    {
        if (StaticFunctions.lowGraphics)
        {
            //set nuuu
            bigG.SetActive(false);
            smolG.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            //set truuu
        }
    }


}
