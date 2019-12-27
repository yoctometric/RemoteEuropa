using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    bool fireLeft = false;
    [SerializeField] Transform p1;
    [SerializeField] Transform p2;
    public int fireForce = 500;
    public string typeName = "";
    bool priorityLeft = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Item>())
        {
            if (typeName == "")
            {
                fireLeft = !fireLeft;

                if (fireLeft)
                {
                    StartCoroutine(Fire(p1, other.gameObject));
                }
                else
                {
                    StartCoroutine(Fire(p2, other.gameObject));
                }
            }
            else
            {
                Item it = other.GetComponent<Item>();
                print(it.typeOfItem + "/" + typeName);

                if (priorityLeft && it.typeOfItem == typeName)
                {
                    StartCoroutine(Fire(p1, other.gameObject));

                }
                else if(!priorityLeft && it.typeOfItem == typeName)
                {
                    StartCoroutine(Fire(p2, other.gameObject));
                }
                else if(priorityLeft)
                {
                    //fire it the other way now
                    StartCoroutine(Fire(p2, other.gameObject));
                }
                else
                {
                    StartCoroutine(Fire(p1, other.gameObject));
                }
            }

        }
    }

    void SetFilter(string name)
    {
        typeName = name;
    }
    IEnumerator Fire(Transform position, GameObject obj)
    {
        obj.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        obj.transform.position = position.position;
        obj.transform.rotation = position.rotation;
        obj.SetActive(true);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.AddForce(position.right * fireForce);

    }
}
