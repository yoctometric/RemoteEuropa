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
    float savedAt = -999;

    private void Start()
    {
        t = gameObject.GetComponent<TMP_Text>();
        baseText = t.text;
    }
    public void ButtonPressed(string path)
    {
        savedAt = Time.time;
        savedAt = Time.time;
        savedAt = Time.time;
        savedAt = Time.time;
        print(savedAt);
        path = path.ToLower();
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
        float since = Time.time - savedAt;
        print(since + ", " + savedAt);
        if (since < 1)
        {
            yousure = true;
        }
        else
        {
            t.text = "Exit without \nsaving?";
        }
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
