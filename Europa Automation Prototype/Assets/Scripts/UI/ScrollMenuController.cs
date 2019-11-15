using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollMenuController : MonoBehaviour
{
    [SerializeField] Scrollbar scroller;
    public float saveHeight;
    public float saveWidth;
    public Button listItemTemplate;
    public Transform scrollingMaster;
    //saves ust be formatted as "Name - Path"
    public List<string> saves;
    public float scrollMultiplier = 5;
    void Start()
    {
        float currentPosY = (saveHeight * 3); //-(saveHeight * (saves.Count + 4));
        for (int i = 0; i < saves.Count; i++)
        {
            //instantiates a button childed to the scrollbar
            RectTransform b = Instantiate(listItemTemplate, transform.position, Quaternion.identity, scrollingMaster.transform).GetComponent<RectTransform>();
            b.position = new Vector3(Screen.width / 2, currentPosY, 0);
            currentPosY -= saveHeight + 10;
            //get display name
            string[] splitName = saves[i].Split('-');
            string displayName = splitName[0];
            string loadPath = "No path";
            if (splitName.Length > 1)
            {
                loadPath = splitName[1];
            }
            //set up display text
            b.GetComponentInChildren<TMP_Text>().text = displayName;
            //set up button
            if (loadPath != null)
            {
                //load from path
            }
        }
        scroller.numberOfSteps = saves.Count;
        

    }

    void Update()
    {
        scrollingMaster.position = new Vector3(0, (scroller.value * saveHeight * saves.Count), 0);
    }
}
