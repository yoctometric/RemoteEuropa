using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLoader : MonoBehaviour
{

    //this is a script to workaround being unable to add events to the button on runtime
    //it is attatched to a parent button
    //and called by the SaveLoader script
    string path = "NO PATH";

    public void SetPath(string thePath)
    {
        path = thePath;
    }
    public void LoadLevel()
    {
        GameObject.FindObjectOfType<SaveLoader>().LoadSaveAtPath(path);
    }
}
