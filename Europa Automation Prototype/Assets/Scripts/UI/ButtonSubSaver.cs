using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonSubSaver : MonoBehaviour
{

    public void ButtonPressed(string path)
    {
        SaveMaster mast = GameObject.FindObjectOfType<SaveMaster>();
        mast.SaveGame(path);
    }

    public void LoadScene(int s)
    {
        SceneManager.LoadScene(s);
    }
}
