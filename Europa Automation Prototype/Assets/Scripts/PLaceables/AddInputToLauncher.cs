using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInputToLauncher : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            transform.parent.GetComponent<LauncherController>().AddItemToStorage(collision.gameObject.GetComponent<Rigidbody2D>());
        }
    }
}
