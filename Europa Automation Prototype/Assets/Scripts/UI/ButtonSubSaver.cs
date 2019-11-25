using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSubSaver : MonoBehaviour
{

    public void ButtonPressed(string path)
    {
        SaveMaster mast = GameObject.FindObjectOfType<SaveMaster>();
        mast.SaveGame(path);
    }
}
