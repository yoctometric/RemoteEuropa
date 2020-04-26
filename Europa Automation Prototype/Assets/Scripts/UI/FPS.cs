using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    private void Start()
    {
        if (StaticFunctions.ShowFPS)
        {
            //no bother
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
    void Update()
    {

        text.text = "FPS: " + Mathf.Round(1.0f / Time.deltaTime).ToString();
    }
}
