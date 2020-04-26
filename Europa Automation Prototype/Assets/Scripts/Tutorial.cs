using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Tutorial : MonoBehaviour
{
    int currentEvent = 0;
    [SerializeField] Animator panel;
    [SerializeField] TMP_Text pText;
    [SerializeField] TextTyper objectiveText;

    [SerializeField] GameObject[] objectiveObjects;
    [SerializeField] GameObject limiter;

    [SerializeField] GameObject checkDestroyThisCrafter;
    [SerializeField] IndicationArrow indicationArrow;

    RewardsManager rewardsManager;

    int currentObject = 0;
    //local variable for objective testing
    Transform storedTrans = null;
    float prevStoredRot;
    float waitTime = 1f;
    float prevTime = 0;
    //invenory handlin
    bool recievedCopper = false;
    bool recievedIron = false;
    bool objectPlaced = false;
    int objectAliveTimer = 0;


    private void Start()
    {
        rewardsManager = GameObject.FindObjectOfType<RewardsManager>();
        panel.SetTrigger("go");
        pText.text = "Welcome.";
        objectiveText.Play("YOUR OBJECTIVE: use W, A, S, or D to move your view.");
    }

    private void Update()
    {
        if (currentEvent == 0)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                currentEvent++;
                objectiveText.Play("Next, Hold shift and use the scroll wheel to increase the size of your view");
                prevTime = Time.time;
            }
        }else if (currentEvent == 1 && prevTime + waitTime < Time.time)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(Input.mouseScrollDelta.y) > 0)
            {
                currentEvent++;
                Camera.main.transform.position = new Vector3(0, 0, -10);
                objectiveText.Play("Now Select a miner, and place it next to the chunk of copper ore at the marked position.");
                limiter.SetActive(true);
                limiter.GetComponent<OutsideDistanceRemover>().typeToWatch = 0;
                ActivateNextObject();
                indicationArrow.gameObject.SetActive(true);
                prevTime = Time.time;
            }
        }
        else if (currentEvent == 2 && prevTime + waitTime < Time.time)
        {
            if (GameObject.FindObjectsOfType<Miner>().Length > 0 && !objectPlaced)
            {
                objectAliveTimer++;//as long as this round isnt over, check if a miner has been alive for a while
                if(objectAliveTimer > 5)
                {
                    //five frames of the miner beign around
                    objectPlaced = true;
                    objectAliveTimer = 0;//now reset
                }
            }
            else
            {
                objectAliveTimer = 0;//reset timer after no miners so that it dont get in the way
            }
            if (objectPlaced)
            {
                currentEvent++;
                limiter.SetActive(false);
                limiter.transform.position = new Vector2(-7.54f, 2.6f);
                limiter.SetActive(true);
                limiter.GetComponent<OutsideDistanceRemover>().typeToWatch = 1;
                objectiveText.Play("next, Place a crafting machine at the marked position");
                indicationArrow.MoveTo(new Vector2(-413, -210));
                prevTime = Time.time;
                objectPlaced = false;//reset obejct placed for next test
            }
        }
        else if (currentEvent == 3 && prevTime + waitTime < Time.time)
        {
            if (GameObject.FindObjectsOfType<Crafting>().Length > 0 && !objectPlaced)
            {
                objectAliveTimer++;
                if(objectAliveTimer > 5)
                {
                    objectAliveTimer = 0;
                    objectPlaced = true;
                }
            }
            else
            {
                objectAliveTimer = 0;
            }
            if (objectPlaced)
            {
                storedTrans = GameObject.FindObjectOfType<Crafting>().transform;
                currentEvent++;
                objectiveText.Play("next, Hover your cursor over the crafting machine and use 'f' to rotate it");
                prevStoredRot = storedTrans.rotation.eulerAngles.z;
                prevTime = Time.time;
                indicationArrow.Out();
                objectPlaced = false;
            }
        }
        else if(currentEvent == 4 && prevTime + waitTime < Time.time)
        {
            if (!storedTrans)
            {
                storedTrans = GameObject.FindObjectOfType<Crafting>().transform;
            }
            else
            {
                if (storedTrans.GetComponentInChildren<EditRotation>().mouseAim)
                {
                    currentEvent++;
                    objectiveText.Play("Using the same methods, aim the crafter at the core, and the miner at the crafter.");
                    ActivateNextObject();
                    prevTime = Time.time;
                }
                /*
                float newRot = storedTrans.rotation.eulerAngles.z;
                if (!(newRot < prevStoredRot + 25 && newRot > prevStoredRot - 25))
                {
                    //nice it's moved
                    currentEvent++;
                    objectiveText.Play("Using the same methods, aim the crafter at the core, and the miner at the crafter.");
                    ActivateNextObject();
                    prevTime = Time.time;
                }
                //prevStoredRot = rot;
                */
            }
        }else if (currentEvent == 5 && prevTime + waitTime < Time.time)
        {
            if(recievedCopper)
            {
                currentEvent++;
                objectiveText.Play("Uh oh, A broken production line has appeared to the north. Use a fan to help it reach the core.");
                prevTime = Time.time;
                ActivateNextObject();
                ActivateNextObject();
                ActivateNextObject();
                ActivateNextObject();
                limiter.SetActive(false);
                limiter.transform.position = new Vector2(-2.66f, 24.56f);
                limiter.SetActive(true);
                indicationArrow.MoveTo(new Vector2(-561, -210));
                limiter.GetComponent<OutsideDistanceRemover>().typeToWatch = 2;
                limiter.GetComponent<OutsideDistanceRemover>().autoDisable = false;
            }
        }
        else if (currentEvent == 6 && prevTime + waitTime < Time.time)
        {
            if (recievedIron)
            {
                currentEvent++;
                prevTime = Time.time;
                limiter.SetActive(false);
                limiter.transform.position = new Vector2(-3.16f, 3.39f);
                limiter.SetActive(true);
                limiter.GetComponent<OutsideDistanceRemover>().typeToWatch = 2;
                limiter.GetComponent<OutsideDistanceRemover>().autoDisable = false;
                indicationArrow.Out();
                ActivateNextObject();
                objectiveText.Play("A misplaced crafter has appeared! Right click on the machine to reclaim its resources");
            }
        }else if (currentEvent == 7 && prevTime + waitTime < Time.time)
        {
            if (!checkDestroyThisCrafter)
            {
                currentEvent++;
                prevTime = Time.time;
                limiter.SetActive(false);
                rewardsManager.MakeGoal(true); //override makegoal
                rewardsManager.GetComponent<CanvasGroup>().alpha = 1; // and make that bb visible
                rewardsManager.ManualLoop(); // oh and get the checker up n runnin
                objectiveText.Play("A production goal has been set. Produce enough Refined Copper per second to meet it, and you will be rewarded");
                ActivateNextObject(); // make all the ores go on
                ActivateNextObject(); //ore 2
                ActivateNextObject(); // ore 3

            }
        }
        else if (currentEvent == 8 && prevTime + waitTime < Time.time)
        {
            if (rewardsManager.aGoalHasBeenMet)
            {
                currentEvent++;
                prevTime = Time.time;
                objectiveText.Play("Well done! Return to the main menu by clicking on the top left of the screen. Enjoy the game!");
            }
        }

    }
    void ActivateNextObject()
    {
        objectiveObjects[currentObject].SetActive(true);
        currentObject++;
    }

    public void GotItem(string item)
    {
        if(item == "Refined Copper")
        {
            recievedCopper = true;
        }else if(item == "Refined Iron"){
            recievedIron = true;
        }
    }
}
