using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
public class ScrollMenuController : MonoBehaviour
{
    [SerializeField] Scrollbar scroller;
    public float saveHeight;
    public float saveWidth;
    public Button listItemTemplate;
    public Transform scrollingMaster;
    //saves ust be formatted as "Name - Path"
    public List<string> sav;
    public float scrollMultiplier = 5;
    void Start()
    {
        string[] filePaths =Directory.GetFiles(Application.persistentDataPath, "*.europa");
        for(int i = 0; i < filePaths.Length; i++)
        {
            filePaths[i] = filePaths[i].Split('\\')[1];
        }
        GenList(filePaths.ToList());
    }

    void GenList(List<string> saves)
    {
        sav = saves;
        float currentPosY = (saveHeight * 3); //-(saveHeight * (saves.Count + 4));
        for (int i = 0; i < saves.Count; i++)
        {
            //instantiates a button childed to the scrollbar
            RectTransform b = Instantiate(listItemTemplate, transform.position, Quaternion.identity, scrollingMaster.transform).GetComponent<RectTransform>();
            ButtonSubLoader bs = b.GetComponentInChildren<ButtonSubLoader>();
            bs.path = saves[i].Split('.')[0];
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
        //scroll wheel input
        float mod = Mathf.RoundToInt(Input.mouseScrollDelta.y);
        scroller.value += mod;
        //moving
        scrollingMaster.position = new Vector3(0, (scroller.value * saveHeight * sav.Count), 0);
    }
}
