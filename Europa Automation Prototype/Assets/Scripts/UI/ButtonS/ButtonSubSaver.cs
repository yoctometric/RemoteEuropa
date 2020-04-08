using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class ButtonSubSaver : MonoBehaviour
{
    string baseText = "";
    TMP_Text t;
    int youSureSteps = 0;
    static float savedAt = -999;

    private void Start()
    {
        t = gameObject.GetComponent<TMP_Text>();
        baseText = t.text;
    }
    public void ButtonPressed(string path)
    {
        savedAt = Time.time;
        //print(savedAt);
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
        //print(since + ", " + savedAt);
        if (since < 10)
        {
            youSureSteps = 5;
        }
        else if (youSureSteps != 1)
        {
            youSureSteps = 1;
            t.text = "Exit without \nsaving?";
            return;
        }

        if (youSureSteps == 1 || youSureSteps == 5)
        {
            t.text = baseText;
            youSureSteps = 0;
            StartCoroutine(LoadScene(s));
        }
        else
        {
            youSureSteps = 1;
        }
        //print("Steps: " + youSureSteps);
    }
    private void OnDisable()
    {
        t.text = baseText;
        youSureSteps = 0;
    }
}
