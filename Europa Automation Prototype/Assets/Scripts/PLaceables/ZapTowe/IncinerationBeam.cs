using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncinerationBeam : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Item>())
        {
            Destroy(other.gameObject);
        }
    }
}
