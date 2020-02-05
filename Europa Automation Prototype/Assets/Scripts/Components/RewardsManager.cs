using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RewardsManager : MonoBehaviour
{
    [SerializeField] DropPod dropPodAnim;
    [SerializeField] float goalStrengthMod = 1.25f;
    [SerializeField] Vector2Int rewardAmountRange;

    [HideInInspector] public int ironSinceLastCheck = 0;
    [HideInInspector] public int copperSinceLastCheck = 0;
    [HideInInspector] public int brickSinceLastCheck = 0;
    [HideInInspector] public int pycreteSinceLastCheck = 0;

    float ironRate = 0;
    float copperRate = 0;
    float brickRate = 0;
    float pycreteRate = 0;

    float prevIronRate = 0;
    float prevCopperRate = 0;
    float prevBrickRate = 0;
    float prevPycreteRate = 0;

    int currentGoalListenerIndex = -1;
    float currentGoalListenerRate = 0;

    [SerializeField] TMP_Text[] texts;
    [SerializeField] Image[] indics;
    [SerializeField] Sprite[] indicSprites;

    [SerializeField] TMP_Text timingText;
    [SerializeField] Animator goalAlert; //Add a little alert to the open button
    [SerializeField] TMP_Text goalText;

    Color green = new Color(0, 1, 0);
    Color red = new Color(0.9f, 0, 0);
    Color yellow = new Color(0.9f, 0.9f, 0);

    CanvasGroup cGroup;
 
    void Start()
    {
        UpdateTexts();
        cGroup = this.GetComponent<CanvasGroup>();
        StartCoroutine(AnalyzeLoop()); //always run it. It will update twice per minute
        StartCoroutine(ProductionGoalTimer());
    }

    IEnumerator AnalyzeLoop()
    {
        ironSinceLastCheck = 0;
        copperSinceLastCheck = 0;
        brickSinceLastCheck = 0;
        pycreteSinceLastCheck = 0;

        for(int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(1);
            timingText.text = ("(Last 30 seconds) Update in: " + (30 - i).ToString() + " sec");
        }

        ironRate = (float)System.Math.Round(ironSinceLastCheck / 30f, 2); //iron per second
        copperRate = (float)System.Math.Round(copperSinceLastCheck / 30f, 2); //cops per second
        brickRate = (float)System.Math.Round(brickSinceLastCheck / 30f, 2); //bricks per second
        pycreteRate = (float)System.Math.Round(pycreteSinceLastCheck / 30f, 2); //pycrete per second

        if(cGroup.alpha != 0)// why waste performance if its not gonna be seen
        {
            UpdateTexts();
        }

        prevIronRate = ironRate;
        prevCopperRate = copperRate;
        prevBrickRate = brickRate;
        prevPycreteRate = pycreteRate;

        //run your checks on the goal
        if (CheckGoal(currentGoalListenerIndex))
        {
            //succeeded goal
            goalText.text = "Goal Met";
            goalText.color = green;
            StartCoroutine(ProductionGoalTimer());
        }


        //print("Iron: " + ironRate.ToString() + " Copper: " + copperRate.ToString() + " Brick: " + brickRate.ToString() + " Pycrete: " + pycreteRate.ToString());
        StartCoroutine(AnalyzeLoop()); //always run it. It will update twice per minute
    }
    IEnumerator ProductionGoalTimer()
    {
        yield return new WaitForSeconds(Random.Range(60, 120)); // wait btw 1 and 2 ins for another goal
        MakeGoal();
        //StartCoroutine(ProductionGoalTimer());
    }

    bool CheckGoal(int choice)
    {
        if(currentGoalListenerIndex >= 0)
        {
            float successRate = -1;
            string type = "null";

            if (choice == 0)
            {
                successRate = ironRate;
                type = "Refined Iron";
            }
            else if (choice == 1)
            {
                successRate = copperRate;
                type = "Refined Copper";
            }
            else if (choice == 2)
            {
                successRate = brickRate;
                type = "Brick";
            }
            else if (choice == 3)
            {
                successRate = pycreteRate;
                type = "Pycrete";
            }
            print(successRate.ToString() + " vs " + currentGoalListenerRate.ToString());
            if (successRate >= currentGoalListenerRate)
            {
                //succeeded goal
                DropPod pod = Instantiate(dropPodAnim, Vector3.zero, Quaternion.identity);
                pod.Drop(type, Mathf.RoundToInt(Random.Range(rewardAmountRange.x, rewardAmountRange.y)));
                //reset listeners
                currentGoalListenerIndex = -1;
                return true;
            }
            else
            {
                //failed goal
                print("negative index");
                return false;
            }
        }
        else
        {
            return false;
        }

    }
    void MakeGoal()
    {
        goalText.color = Color.white;

        if(cGroup.alpha < 1)
        {
            goalAlert.gameObject.SetActive(true);
        }
        int choice = Random.Range(0, 4);
        if(choice == 0)
        {
            currentGoalListenerIndex = 0;
            currentGoalListenerRate = (ironRate * goalStrengthMod) + 0.25f;
            goalText.text = "Goal: " + currentGoalListenerRate.ToString() + " iron/s";
        }else if(choice == 1)
        {
            currentGoalListenerIndex = 1;
            currentGoalListenerRate = (copperRate * goalStrengthMod) + 0.25f;
            goalText.text = "Goal: " + currentGoalListenerRate.ToString() + " copper/s";
        }
        else if (choice == 2)
        {
            currentGoalListenerIndex = 2;
            currentGoalListenerRate = (brickRate * goalStrengthMod) + 0.25f;
            goalText.text = "Goal: " + currentGoalListenerRate.ToString() + " brick/s";
        }
        else if (choice == 3)
        {
            currentGoalListenerIndex = 3;
            currentGoalListenerRate = (pycreteRate * goalStrengthMod) + 0.25f;
            goalText.text = "Goal: " + currentGoalListenerRate.ToString() + " pycrete/s";
        }
    }

    void UpdateTexts()
    {
        texts[0].text = "Iron:   " + ironRate.ToString() + "/s";
        if(ironRate > prevIronRate)
        {
            UpdateIndicator(indics[0], 2);
        }else if(ironRate < prevIronRate)
        {
            UpdateIndicator(indics[0], 0);
        }
        else
        {
            UpdateIndicator(indics[0], 1);
        }
        texts[1].text = "Copper:   " + copperRate.ToString() + "/s";
        if (copperRate > prevCopperRate)
        {
            UpdateIndicator(indics[1], 2);
        }
        else if (copperRate < prevCopperRate)
        {
            UpdateIndicator(indics[1], 0);
        }
        else
        {
            UpdateIndicator(indics[1], 1);
        }
        texts[2].text = "Brick:   " + brickRate.ToString() + "/s";
        if (brickRate > prevBrickRate)
        {
            UpdateIndicator(indics[2], 2);
        }
        else if (brickRate < prevBrickRate)
        {
            UpdateIndicator(indics[2], 0);
        }
        else
        {
            UpdateIndicator(indics[2], 1);
        }
        texts[3].text = "Pycrete:   " + pycreteRate.ToString() + "/s";
        if (pycreteRate > prevPycreteRate)
        {
            UpdateIndicator(indics[3], 2);
        }
        else if (pycreteRate < prevPycreteRate)
        {
            UpdateIndicator(indics[3], 0);
        }
        else
        {
            UpdateIndicator(indics[3], 1);
        }
    }

    void UpdateIndicator(Image img, int state)
    {
        img.sprite = indicSprites[state];

        if (state == 0)
        {
            img.transform.localRotation = Quaternion.Euler(0, 0, 0);
            img.color = red;
        }
        else if (state == 1)
        {
            img.transform.localRotation = Quaternion.Euler(0, 0, 0);
            img.color = yellow;
        }
        else
        {
            img.transform.localRotation = Quaternion.Euler(0, 0, 180);
            img.color = green;
        }
    }
}
