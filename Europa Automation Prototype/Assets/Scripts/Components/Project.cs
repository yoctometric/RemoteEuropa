using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Project : MonoBehaviour
{
    [SerializeField] TMP_Text[] texts;
    [SerializeField] GameObject pan;

    [SerializeField] int[] costs;

    [SerializeField] GameObject button;

    Color r = new Color(1f, 0.5f, 0.5f);
    Color g = new Color(0, 0.9f, 0);

    private string theObjective;
    public bool panOn = false;
    private void Start()
    {
        UnsetPanel();
        costs = new int[4];
    }
    public void SetPanel(int cop, int iro, int pyc, int bri, string objective)
    {
        panOn = true;
        pan.SetActive(true);
        texts[0].text = "Project: " + objective;
        texts[1].text = StaticFunctions.AbbreviateNumber(cop);
        texts[2].text = StaticFunctions.AbbreviateNumber(iro);
        texts[3].text = StaticFunctions.AbbreviateNumber(pyc);
        texts[4].text = StaticFunctions.AbbreviateNumber(bri);

        costs[0] = cop;
        costs[1] = iro;
        costs[2] = pyc;
        costs[3] = bri;

        theObjective = objective;
        //hit it with a quick color update
        GameObject.FindObjectOfType<Inventory>().UpdateInventory("Refined Copper", 0);
    }
    public void UpdatePanel(int cop, int iro, int pyc, int bri)
    {
        int counter = 0;
        if(costs[0] > cop)
        {
            texts[1].color = r;
        }
        else
        {
            texts[1].color = g;
            counter++;
        }

        if (costs[1] > iro)
        {
            texts[2].color = r;
        }
        else
        {
            texts[2].color = g;
            counter++;
        }

        if (costs[2] > pyc)
        {
            texts[3].color = r;
        }
        else
        {
            texts[3].color = g;
            counter++;
        }

        if (costs[3] > bri)
        {
            texts[4].color = r;
        }
        else
        {
            texts[4].color = g;
            counter++;
        }
        if(counter == 4)
        {
            //button, ACTIVATE
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }
    }
    public void UnsetPanel()
    {
        panOn = false;
        pan.SetActive(false);
        button.SetActive(false);
    }
    public void ContextuallyOperate()
    {
        //op
        UnsetPanel();

        if (theObjective.ToLower().Contains("core"))
        {
            GameObject.FindObjectOfType<Core>().Upgrade(false);
        }
    }
}
