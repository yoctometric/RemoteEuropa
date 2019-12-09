using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class KeyBinder : MonoBehaviour
{
    //uhhhhhhhhhhhhhhhhh it don werk durrrr
    List<Key> keys;

    private void Start()
    {
        keys = new List<Key>();
        keys.Add(new Key("1", "1", "select_0"));
        keys.Add(new Key("2", "2", "select_1"));
        keys.Add(new Key("3", "3", "select_2"));
        keys.Add(new Key("4", "4", "select_3"));
        keys.Add(new Key("5", "5", "select_4"));
        keys.Add(new Key("6", "6", "select_5"));
    }
    void Update()
    {
        //wait for a keypress
        if (Input.anyKeyDown)
        {
            
            //now actually test which key
            Event e = Event.current;
            if (e != null)
            {
                Debug.Log(e.keyCode);
            }

        }
    }
}
