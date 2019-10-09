using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void FreezeTime(bool yes)
    {
        if (yes)
        {
            Time.timeScale = 0;

        }
        else
        {
            Time.timeScale = 1;

        }
    }
}
