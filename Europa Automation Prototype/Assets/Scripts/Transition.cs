using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Transition : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    private void OnLevelWasLoaded(int level)
    {
        anim.SetTrigger("In");
    }
    public void LoadingScene()
    {
        print("loading " + Time.time);
        anim.SetBool("Loading", true);
    }
    public void EndLoadingScene()
    {
        print("endloading " + Time.time);

        anim.SetBool("Loading", false);
        anim.SetBool("Generating", false);

    }
}
