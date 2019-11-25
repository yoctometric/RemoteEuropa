using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSubLoader : MonoBehaviour
{
    public string path = "test";

    public void ButtonPressed()
    {
        SaveMaster mast = GameObject.FindObjectOfType<SaveMaster>();

        mast.LoadGame(path);
    }
}
