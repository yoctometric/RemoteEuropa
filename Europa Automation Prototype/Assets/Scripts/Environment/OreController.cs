using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreController : MonoBehaviour
{
    //the product
    public Rigidbody2D product;
    //how many it starts with
    [SerializeField] int quantity;
    //obvious
    public int currentQuantity;
    SpriteRenderer sp;
    //alpha value of the sp
    float aVal = 1;
    void Start()
    {
        //setup
        sp = gameObject.GetComponent<SpriteRenderer>();
        currentQuantity = quantity;
    }

    void Update()
    {
        //once the ore runs out, fade
        if(currentQuantity <= 0 && sp.color.a > 0f)
        {
            aVal -= 0.005f;
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, aVal);
        }
        //remove this GO once it runs out and fades
        if(sp.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
