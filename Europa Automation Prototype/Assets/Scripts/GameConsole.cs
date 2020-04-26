using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameConsole : MonoBehaviour
{
    [SerializeField] TMP_Text console;
    private CanvasGroup can;

    private List<string> lines;
    [SerializeField] int displayedLinesPerPage;
    
    //legacy fade
    bool visual = false;
    //new fade
    bool shouldDisplay = false;

    [SerializeField] float timeToDisplay;
    float lasttimeToDisplay = -1;

    int currentPage = 0;
    int totalPages = 0;

    private void Start()
    {
        lines = new List<string>();
        can = gameObject.GetComponent<CanvasGroup>();
        Render();
        //StartCoroutine(Fade(true));

        //testing:

        AddLine("Session started. Welcome, commander.");
    }

    public void Show()
    {
        shouldDisplay = true;
        Render();
    }
    public void EndShow()
    {
        shouldDisplay = false;
    }

    public void AddLine(string line)
    {
        lines.Add(line);
        totalPages += 1;
        ScrollText(1); // keep with the times
        Render();
    }

    private void Update()
    {
        //debugg
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddLine("This is entry number " + totalPages + " being displayed in the console");
        }*/

        //fading logic
        if(shouldDisplay && can.alpha < 1)
        {
            can.alpha += Time.deltaTime;
        }
        else if (!shouldDisplay && can.alpha > 0)
        {
            can.alpha -= Time.deltaTime;
        }

        if (lasttimeToDisplay + timeToDisplay < Time.time && shouldDisplay)
        {
            shouldDisplay = false;
        }
    }
    public void Render()
    {
        shouldDisplay = true;
        lasttimeToDisplay = Time.time;
        /*
        if (!visual)
        {
            StartCoroutine(Fade(true));
        }*/

        string master = "";
        for (int i = currentPage; i < currentPage + displayedLinesPerPage; i++)
        {

            if(i < lines.Count && i >= 0)
            {
                master += lines?[i] + Environment.NewLine;
            }
            else
            {
                continue;
            }
        }
        console.text = master;
    }


    //legacy fade
    /*
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

            while (shouldDisplay)
            {
                print("waiting");
                yield return new WaitUntil(() => lasttimeToDisplay + timeToDisplay <= Time.time);
            }
        }
        if (!fadingIn)
        {
            visual = false;
            step = -0.05f;
            while (can.alpha > 0 && !shouldDisplay)
            {
                print("going out");
                can.alpha += step;
                yield return new WaitForEndOfFrame();
            }
        }
    }*/

    public void ScrollText(int dir)
    {
        currentPage += dir;
        currentPage = Mathf.Clamp(currentPage, 0, totalPages - displayedLinesPerPage);
        Render();
    }
}
