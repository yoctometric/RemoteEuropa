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
    int currentObject = 0;
    //local variable for objective testing
    Transform storedTrans = null;
    Quaternion prevStoredRot;
    float waitTime = 1f;
    float prevTime = 0;
    //invenory handlin
    bool recievedCopper = false;
    bool recievedIron = false;
    private void Start()
    {
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
                prevTime = Time.time;
            }
        }
        else if (currentEvent == 2 && prevTime + waitTime < Time.time)
        {
            print(GameObject.FindObjectsOfType<Miner>().Length);
            if (GameObject.FindObjectsOfType<Miner>().Length > 0)
            {
                currentEvent++;
                limiter.SetActive(false);
                limiter.transform.position = new Vector2(-7.54f, 2.6f);
                limiter.SetActive(true);
                limiter.GetComponent<OutsideDistanceRemover>().typeToWatch = 1;
                objectiveText.Play("next, Place a crafting machine at the marked position");
                prevTime = Time.time;
            }
        }
        else if (currentEvent == 3 && prevTime + waitTime < Time.time)
        {
            if (GameObject.FindObjectsOfType<Crafting>().Length > 0)
            {
                storedTrans = GameObject.FindObjectOfType<Crafting>().transform;
                currentEvent++;
                objectiveText.Play("next, Hover your cursor over the crafting machine and use 'q','e', 'f' or scroll wheel to rotate it");
                prevStoredRot = storedTrans.rotation;
                prevTime = Time.time;
            }
        }
        else if(currentEvent == 4 && prevTime + waitTime < Time.time)
        {
            Quaternion rot = storedTrans.rotation;
            if (rot != prevStoredRot)
            {
                //nice it's moved
                currentEvent++;
                objectiveText.Play("Using the same methods, aim the crafter at the core, and the miner at the crafter.");
                ActivateNextObject();
                prevTime = Time.time;
            }
            prevStoredRot = rot;
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
                limiter.GetComponent<OutsideDistanceRemover>().typeToWatch = 2;
                limiter.GetComponent<OutsideDistanceRemover>().autoDisable = false;
            }
        }
        else if (currentEvent == 6 && prevTime + waitTime < Time.time)
        {
            if (recievedIron)
            {
                limiter.SetActive(false);
                currentEvent++;
                objectiveText.Play("Well done, engineer! Return to the main menu by clicking on the top left of the screen. Enjoy the game!");
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
