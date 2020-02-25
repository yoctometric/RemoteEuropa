using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ButtonSubLoader : MonoBehaviour
{
    public string path = "test";

    public void ButtonPressed()
    {
        SaveMaster mast = GameObject.FindObjectOfType<SaveMaster>();

        mast.LoadGame(path);
    }
    public void DeleteGame()
    {
        Debug.Log(Application.dataPath + "/" + path + ".europa");
        File.Delete(Application.dataPath + "/" + path + ".europa");
        Destroy(transform.parent.gameObject);
    }
}
