using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    bool go = false;
    private void Start()
    {
        if (StaticFunctions.ShowFPS)
        {
            go = true;
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (go)
        {
            text.text = "FPS: " + Mathf.Round(1.0f / Time.deltaTime).ToString();
        }
    }
}
