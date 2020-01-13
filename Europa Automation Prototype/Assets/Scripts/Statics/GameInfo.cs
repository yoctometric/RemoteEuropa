using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameInfo : MonoBehaviour
{
    [SerializeField] GameObject transPanel;
    public ScriptableRecipe storedRecipe;
    public bool currentlyMouseAiming = false;

    public void Start()
    {
        DontDestroyOnLoad(Instantiate(transPanel, Vector3.zero, Quaternion.identity));
    }
    
}
