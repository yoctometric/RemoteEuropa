using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicationArrow : MonoBehaviour
{
    Animator anim;
    RectTransform rT;

    private void Start()
    {
        rT = gameObject.GetComponent<RectTransform>();
        anim = gameObject.GetComponent<Animator>();
    }


    public void In()
    {
        anim.SetTrigger("Enter");
    }

    public void Out()
    {
        anim.SetTrigger("Exit");
    }

    public void MoveTo(Vector2 pos)
    {
        StartCoroutine(MoveLogic(pos));
    }

    IEnumerator MoveLogic(Vector2 pos)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Out"))
        {
            Out();//only out if not already out

        }
        yield return new WaitForSeconds(0.2f);
        rT.anchoredPosition = pos;
        In();
    }
}
