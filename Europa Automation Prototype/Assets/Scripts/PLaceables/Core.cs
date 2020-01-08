using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Core : MonoBehaviour
{
    Inventory invent;
    ///Dictionary<string, int> inventory = new Dictionary<string, int>();
    public int level = 0;
    [SerializeField] GameObject[] levels;
    [Header("Copper, iron, pycrete, then brick")]
    public Vector4[] lvlCosts;
    [SerializeField] GameObject[] lvl2Objects;
    [SerializeField] GameObject[] lvl3Objects;
    [SerializeField] GameObject impactParticle;
    [SerializeField] GameObject impact2Particle;
    [SerializeField] GameObject explosionParticle;
    [Header("Animation Bitties")]
    [SerializeField] float scaleSpeed = 1;
    float min = 0.75f;
    [SerializeField] float animScale = 15f;
    bool scaling = false;

    Tutorial tut;
    Animator anim;
    void Start()
    {
        tut = GameObject.FindObjectOfType<Tutorial>();
        anim = gameObject.GetComponent<Animator>();        
        invent = GameObject.FindObjectOfType<Inventory>();
        //set upgrade target
        if (level < 2)
        {
            Project proj = GameObject.FindObjectOfType<Project>();
            proj.SetPanel(Mathf.RoundToInt(lvlCosts[level].x), Mathf.RoundToInt(lvlCosts[level].x), Mathf.RoundToInt(lvlCosts[level].x), Mathf.RoundToInt(lvlCosts[level].x), "Upgrade Core");
        }

    }
    private void FixedUpdate()
    {
        //handle scaling animation
        if (scaling)
        {
            transform.localScale *= 0.9f * scaleSpeed;
            print(transform.localScale.x);
            if(transform.localScale.x < 0.75f)
            {
                scaling = false;
                transform.localScale = new Vector3(0.75f, 0.75f, 1);
            }
        }
    }
    public void Upgrade(bool bypassCost)
    {
        bool goisgo = false;
        if (!bypassCost)
        {
            if(invent.UpdateInventory("Refined Copper", -Mathf.RoundToInt(lvlCosts[level].x)))
            {
                if (invent.UpdateInventory("Refined Iron", -Mathf.RoundToInt(lvlCosts[level].y)))
                {
                    if(invent.UpdateInventory("Pycrete", -Mathf.RoundToInt(lvlCosts[level].z)))
                    {
                        if (invent.UpdateInventory("Brick", -Mathf.RoundToInt(lvlCosts[level].w)))
                        {
                            goisgo = true;
                        }
                        else
                        {
                            print(":refund cops iron pyc:");
                            //invent.UpdateInventory("Brick", Mathf.RoundToInt(lvlCosts[level].w));
                            invent.UpdateInventory("Pycrete", Mathf.RoundToInt(lvlCosts[level].z));
                            invent.UpdateInventory("Refined Iron", Mathf.RoundToInt(lvlCosts[level].y));
                            invent.UpdateInventory("Refined Copper", Mathf.RoundToInt(lvlCosts[level].x));
                        }

                    }
                    else
                    {
                        print(":refund cops iron :");
                        //invent.UpdateInventory("Pycrete", Mathf.RoundToInt(lvlCosts[level].z));
                        invent.UpdateInventory("Refined Iron", Mathf.RoundToInt(lvlCosts[level].y));
                        invent.UpdateInventory("Refined Copper", Mathf.RoundToInt(lvlCosts[level].x));
                    }
                }
                else
                {
                    print("refund cops, ");
                    //invent.UpdateInventory("Refined Iron", Mathf.RoundToInt(lvlCosts[level].y));
                    invent.UpdateInventory("Refined Copper", Mathf.RoundToInt(lvlCosts[level].x));
                }

            }
            else
            {
                print("cancel");
                //invent.UpdateInventory("Refined Copper", Mathf.RoundToInt(lvlCosts[level].x));
            }
        }
        else
        {
            goisgo = true;
        }
        if (goisgo)
        {
            //invent.ToggleUpButton(false);
            level++;
            if(level == 2)
            {
                transform.localScale *= animScale/2;
            }
            else
            {
                transform.localScale *= animScale;
            }
            
            levels[level].SetActive(true);
            levels[level - 1].SetActive(false);
            ///Animation area
            if (!bypassCost)
            {
                Instantiate(explosionParticle, transform.position, Quaternion.identity);
                //make a warning
                GameObject.FindObjectOfType<Warning>().InitiateWarning("CORE DROP INCOMING", false, true);
                anim.SetTrigger("Drop");
                ///End animation area
            }

            //set upgrade target
            if (level < 2)
            {
                Project proj = GameObject.FindObjectOfType<Project>();

                proj.SetPanel(Mathf.RoundToInt(lvlCosts[level].x), Mathf.RoundToInt(lvlCosts[level].x), Mathf.RoundToInt(lvlCosts[level].x), Mathf.RoundToInt(lvlCosts[level].x), "Upgrade Core");
            }
            //set new objs active
            if (level == 1)
            {
                foreach (GameObject obj in lvl2Objects)
                {
                    obj.SetActive(true);
                }
            } else if (level == 2)
            {
                foreach (GameObject obj in lvl3Objects)
                {
                    obj.SetActive(true);
                }
            }
        }

    }
    public void StartLand()
    {
        scaling = true;
    }
    public void Land()
    {
        Transform pt = Instantiate(impactParticle, transform.position, Quaternion.identity).transform;
        Transform pt2 = Instantiate(impact2Particle, transform.position, Quaternion.identity).transform;
        pt.localScale *= level;
        pt2.localScale *= level *.75f;
        GameObject.FindObjectOfType<Warning>().CancelWarning();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Item>())
        {

            Item it = other.GetComponent<Item>();
            string itemType = it.typeOfItem;
            /*
            if (inventory.TryGetValue(itemType, out int value))
            {
                inventory[itemType] += 1;
                Destroy(it.gameObject, 0.1f);
            }
            else
            {
                inventory.Add(itemType, 1);
                Destroy(it.gameObject, 0.1f);
            }*/
            //string val = StaticFunctions.AbbreviateNumber(inventory[itemType]);
            if (invent.storedVals.Keys.ToList().Contains(itemType))
            {
                invent.UpdateInventory(itemType, 1);
                if (tut)
                {
                    tut.StartEvent(itemType);
                }
                Destroy(it.gameObject);
            }
        }
    }
}
