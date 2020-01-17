using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideDistanceRemover : MonoBehaviour
{
    List<GameObject> omittedList;
    Animator anim;
    [SerializeField] float dist = 10;
    public int typeToWatch = 0;
    public bool autoDisable = true;
    private void OnEnable()
    {
        anim = gameObject.GetComponent<Animator>();
        omittedList = new List<GameObject>();
        foreach(Miner min in GameObject.FindObjectsOfType<Miner>())
        {
            omittedList.Add(min.gameObject);
        }
        foreach (Crafting min in GameObject.FindObjectsOfType<Crafting>())
        {
            omittedList.Add(min.gameObject);
        }
        foreach (GameObject min in GameObject.FindGameObjectsWithTag("Fan"))
        {
            omittedList.Add(min.gameObject);
        }
    }
    void LateUpdate()
    {
        foreach (Miner min in GameObject.FindObjectsOfType<Miner>())
        {
            if (!omittedList.Contains(min.gameObject) && Vector2.Distance(min.transform.position, transform.position) > dist)
            {
                if (Vector2.Distance(min.transform.position, transform.position) > dist)
                {
                    Remove(min.gameObject);
                }
                else if (typeToWatch == 0 && autoDisable)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        foreach (Crafting min in GameObject.FindObjectsOfType<Crafting>())
        {
            if (!omittedList.Contains(min.gameObject))
            {
                print("newcrafter");
                if (Vector2.Distance(min.transform.position, transform.position) > dist)
                {
                    Remove(min.gameObject);
                }
                else if (typeToWatch == 1 && autoDisable)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        foreach (GameObject min in GameObject.FindGameObjectsWithTag("Fan"))
        {
            if (!omittedList.Contains(min.gameObject))
            {
                if (Vector2.Distance(min.transform.position, transform.position) > dist)
                {
                    Remove(min.gameObject);
                }
                else if (typeToWatch == 2 && autoDisable)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    void Remove(GameObject gameO)
    {
        Destroy(gameO);
        anim.SetTrigger("Play");
    }
}
