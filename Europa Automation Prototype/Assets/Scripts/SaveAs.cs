using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveAs : MonoBehaviour
{
    [SerializeField] TMP_InputField input;
    [SerializeField] ButtonSubSaver saver;

    [SerializeField] string valids;
    public void Save()
    {
        string p = IsValid(input.text);
        if(p == "invalid")
        {
            return;
        }
        if(p.Length > 0)
        {
            //save
            saver.ButtonPressed(p);
        }
        else
        {
            StartCoroutine(Flash());
        }
    }
    string IsValid(string input)
    {
        string output = input;
        input = input.ToLower();
        for(int i = 0; i < input.Length; i++)
        {
            if (!valids.Contains(input[i].ToString()))
            {
                StartCoroutine(Flash());
                return "invalid";
            }
        }

        return output;
    }
    IEnumerator Flash()
    {
        input.image.color = new Color(1, 0, 0, 0.75f);
        yield return new WaitForSecondsRealtime(0.25f);
        
        input.image.color = new Color(1, 1, 1, 0.38f);
    }
}
