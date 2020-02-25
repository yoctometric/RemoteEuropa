using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class OpenOnMouseOver : MonoBehaviour
{
    [SerializeField] GameObject activeTarget;
    GameConsole cons;
    private void Start()
    {
        if (activeTarget.GetComponent<GameConsole>())
        {
            cons = activeTarget.GetComponent<GameConsole>();
        }
    }
    public void Over()
    {
        print("over");
        activeTarget.SetActive(true);
        cons?.Render();
    }
    public void Click()
    {
        print("click");
        activeTarget.SetActive(true);
        cons?.Render();
    }
}
