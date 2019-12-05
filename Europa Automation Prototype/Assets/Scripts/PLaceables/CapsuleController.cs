using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;

    //public int maxItems = 50;//moved to packager
    public List<Item> items;
    public List<string> stringIts;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public bool AddItem(Item it)
    {
        //This is a bool because I want the loader to say:
        //"If(AddItem(other)){ do shit } else { start moving items to buffer }". Should be clean that way
        //print(it.name);

        if (items.Count < 1000 )//just replace with maxitems if things get hairy
        {
            //add item, deactivate and freeze it
            GameObject iter = Instantiate(it.gameObject, transform.position, transform.rotation);
            Item itemmmmm = iter.GetComponent<Item>();
            itemmmmm.gameObject.SetActive(false);
            items.Add(itemmmmm);
            stringIts.Add(itemmmmm.typeOfItem);
            Destroy(it.gameObject);
            //it.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return true;

        }
        else
        {
            //failure
            //tell the packager to launch up and stop accepting
            return false;
        }
    }

    private void Update()
    {
    }

    public void Unload(UnPackager p)
    {
        anim.SetTrigger("Open");

        if (items.Count > 0)
        {
            //sets up stored items, empties egg
            for (int i = 0; i < items.Count; i++)
            {
                p.storedItems.Add(items[i].gameObject);
            }
            //items.Clear();
        }
        else
        {
            print("emptyCapsule");
        }
 

    }
}
