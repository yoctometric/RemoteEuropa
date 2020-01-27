using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class ButtonSubSaver : MonoBehaviour
{
    string baseText = "";
    TMP_Text t;
    bool yousure = false;
    private void Start()
    {
        t = gameObject.GetComponent<TMP_Text>();
        baseText = t.text;
    }
    public void ButtonPressed(string path)
    {
        SaveMaster mast = GameObject.FindObjectOfType<SaveMaster>();
        mast.SaveGame(path);
    }

    IEnumerator LoadScene(int s)
    {
        t.text = "Loading...";
        GameObject.FindObjectOfType<Transition>().GetComponent<Animator>().SetTrigger("Out");
        GameObject.FindObjectOfType<PauseMenu>().FreezeTime(false);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(s);
    }

    public void AreYouSureLoadScene(int s)
    {
        t.text = "You sure?";
        if (yousure)
        {
            t.text = baseText;
            yousure = false;
            StartCoroutine(LoadScene(s));
        }
        else
        {
            yousure = true;
        }
    }
    private void OnDisable()
    {
        t.text = baseText;
        yousure = false;
    }
}
