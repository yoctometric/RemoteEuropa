using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseOnMouseExit : MonoBehaviour
{
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }

}
