using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCanvasGroupActivator : MonoBehaviour
{
    
    public void ToggleGroup(CanvasGroup group)
    {
        if(group.alpha > 0.5f)
        {
            group.alpha = 0f;
            transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
            group.alpha = 1f;
        }
    }
}
