using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Warning : MonoBehaviour
{
    [SerializeField] int maxDist = 250;
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject panel;

    bool warningRightNow = false;
    Vector2 mid = new Vector2(0, 0);
    void Update()
    {
        //calculate dist
        float dist = Vector2.Distance(mid, transform.position);
        Debug.Log(dist);
        if(dist > maxDist)
        {
            ///warn
            InitiateWarning("RETURN TO PLAY AREA!");
        }else if (warningRightNow)
        {
            CancelWarning();
        }
    }

    void InitiateWarning(string warning)
    {
        warningRightNow = true;
        panel.SetActive(true);
        text.text = warning;
    }
    void CancelWarning()
    {
        warningRightNow = false;
        panel.SetActive(false);
        text.text = "";
    }
}
