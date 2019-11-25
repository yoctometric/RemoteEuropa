using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoSetup : MonoBehaviour
{
    [SerializeField] GameInfo prefab;
    void Start()
    {
        if (!GameObject.FindObjectOfType<GameInfo>())
        {
            Instantiate(prefab, new Vector3(69, 69, 69), Quaternion.identity);
        }
    }
}
