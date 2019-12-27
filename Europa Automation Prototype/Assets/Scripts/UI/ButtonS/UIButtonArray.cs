using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIButtonArray : MonoBehaviour
{
    /// <summary>
    /// This adds all required components to every child button. Just add it to the master and you're all set
    /// </summary>
    [SerializeField] GameObject selectIndicator;
    public Button[] buttons;
    private void Awake()
    {
        GameObject obj = Instantiate(selectIndicator, transform.position, Quaternion.identity);
        obj.SetActive(false);
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(1, 1, 1);
        selectIndicator = obj;
        buttons = gameObject.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
        {
            ArrayChild kiddo = b.gameObject.AddComponent<ArrayChild>();
            b.onClick.AddListener(kiddo.Activate);
            kiddo.master = this;
        }
    }

    public void BClick(Button b)
    {
        selectIndicator.SetActive(true);
        selectIndicator.transform.position = b.transform.position;
    }
}
