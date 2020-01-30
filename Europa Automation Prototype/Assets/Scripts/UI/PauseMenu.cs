using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PauseMenu : MonoBehaviour
{
    SaveMaster mast;
    WorldGen wGen;
    [SerializeField] TMP_InputField input;

    private void OnEnable()
    {
        if (!mast)
        {
            mast = GameObject.FindObjectOfType<SaveMaster>();
        }
        if (!wGen)
        {
            wGen = GameObject.FindObjectOfType<WorldGen>();
        }
        print(mast.currentPath);
        if (input && mast)
        {
            input.text = mast.currentPath;
        }
    }
    private void OnDisable()
    {
        FreezeTime(false);
    }
    public void FreezeTime(bool yes)
    {
        if (yes && !mast.loading && wGen.doneGenerating)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
