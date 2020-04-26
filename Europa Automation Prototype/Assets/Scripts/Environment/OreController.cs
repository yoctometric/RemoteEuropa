using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreController : MonoBehaviour
{
    public float hardness = 1;//needs no saving, just for miners to know how hard it is to mine. (multiplies miner speed)
    //the product
    public Item product;
    //how many it starts with
    public int quantity;
    //obvious
    public int currentQuantity;
    public Color oreColor;
    SpriteRenderer sp;
    //alpha value of the sp
    float aVal = 1;

    //is the ore eternal?
    public bool eternal = false;
    [SerializeField] ParticleSystem OreDestroyEffect;

    void Start()
    {
        //setup
        sp = gameObject.GetComponent<SpriteRenderer>();
        sp.color = oreColor;
        currentQuantity = quantity;
    }

    void Update()
    {
        //eternally maintain ore
        if(eternal && quantity < 100)
        {
            quantity = 100;
        }

        //once the ore runs out, fade
        if(currentQuantity <= 0 && sp.color.a > 0f)
        {
            aVal -= 0.005f;
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, aVal);
            //remove this GO once it runs out and fades
            if (sp.color.a <= 0.01f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Detonate()
    {
        ParticleSystem p = Instantiate(OreDestroyEffect, transform.position, Quaternion.identity);
        p.startColor = oreColor;
        Destroy(this.gameObject);
    }
}
