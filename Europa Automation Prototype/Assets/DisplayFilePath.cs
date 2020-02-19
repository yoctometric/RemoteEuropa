using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayFilePath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TMP_InputField>().text = Application.persistentDataPath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
