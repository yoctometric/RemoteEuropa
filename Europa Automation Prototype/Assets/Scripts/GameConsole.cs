using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameConsole : MonoBehaviour
{
    private TMP_Text console;
    private CanvasGroup can;

    private List<string> lines;
    [SerializeField] int maxLines = 10;
    bool visual = false;

    float timeToDisplay = 5;
    float lasttimeToDisplay = -1;

    private void Start()
    {
        lines = new List<string>();
        console = gameObject.GetComponent<TMP_Text>();
        can = gameObject.GetComponent<CanvasGroup>();
        Render();
        //testing:
        AddLine("Session started. Welcome, commander.");
    }

    public void AddLine(string line)
    {
        lines.Add(line);
        Render();
    }

    public void Render()
    {
        lasttimeToDisplay = Time.time;
        if (!visual)
        {
            StartCoroutine(Fade(true));
        }

        if (lines.Count > maxLines)
        {
            lines.RemoveAt(0);
        }
        string master = "";
        for (int i = 0; i < lines.Count; i++)
        {
            master += lines[i] + Environment.NewLine;
        }
        console.text = master;
    }

    IEnumerator Fade(bool fadingIn)
    {
        float step = -1;
        if (fadingIn)
        {
            visual = true;
            step = 0.05f;
            while (can.alpha < 1)
            {
                can.alpha += step;
                yield return new WaitForEndOfFrame();
            }
            //now wait for fadeOut
            fadingIn = false;

            yield return new WaitUntil(() => lasttimeToDisplay + timeToDisplay <= Time.time);
        }
        if (!fadingIn)
        {
            visual = false;
            step = -0.05f;
            while (can.alpha > 0)
            {
                can.alpha += step;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
