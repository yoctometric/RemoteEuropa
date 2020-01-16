using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncinerationBeam : MonoBehaviour
{
    [SerializeField] GameObject destEffect;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Item>())
        {
            Instantiate(destEffect, other.transform.position, Quaternion.identity).transform.localScale *= 0.2f;
            Destroy(other.gameObject);
        }
    }
}
