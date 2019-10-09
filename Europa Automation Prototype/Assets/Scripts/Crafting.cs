using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] Transform launchPoint;
    public ScripltableRecipe recipe;
    [SerializeField] List<string> currentItems;
    bool crafting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool reject = true;
       
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();

            //add the item to the storage only if it is in the recipe
            if (recipe.input.Contains(item.typeOfItem))
            {

                //if the item is not already in the recipe
                if (currentItems.Contains(item.typeOfItem))
                {
                }
                else
                {

                    currentItems.Add(item.typeOfItem);
                    Destroy(other.gameObject);
                    reject = false;
                }
            
            }
            if (reject)
            {
                other.GetComponent<Rigidbody2D>().AddForce(transform.up * -200);
            }
            CheckCraft();

        }

    }

    void CheckCraft()
    {
        int currentSatisfiedAmount = recipe.input.Count;
        //for every item in the recipe, if the current list has it, increment our count of contained items
        for (int i = 0; i < recipe.input.Count; i++)
        {
            if (currentItems.Contains(recipe.input[i]))
            {
                currentSatisfiedAmount--;
            }
        }
        //if we have all the items, craft
        if(currentSatisfiedAmount == 0 && !crafting)
        {
            print("CRAFTING!");
            StartCoroutine(Craft());
            //for every result, instantiate and launch it

        }

    }

    IEnumerator Craft()
    {
        //clear the items count so that it doesnt get too much
        currentItems.Clear();
        crafting = true;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < recipe.output.Count; i++)
        {
            Rigidbody2D result = Instantiate(recipe.output[i], launchPoint.position, launchPoint.rotation).GetComponent<Rigidbody2D>();
            result.AddForce(launchPoint.up * 500);
        }
        //set launch forece because I don't think the player should be able to modify it
        crafting = false;
    }

    /*
    [SerializeField] Transform launchPoint;
    public List<string> recipeItems;
    [SerializeField] List<string> currentItems;
    int currentItemCount = 0;
    public Item itemEndPoint;
    bool crafting = false;

    void Start()
    {
        
    }
    void Update()
    {
        //check if u have the components
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //add the item on a trigger hit
        if(collision.tag == "Item")
        {
            AddItemToStorage(collision.gameObject.GetComponent<Item>());
        }
    }
    public void AddItemToStorage(Item item)
    {
        //only add items if under max capacity. If over, spit it back out
        string itemName = item.GetComponent<Item>().typeOfItem;
        if (recipeItems.Contains(itemName))
        {
            if(currentItems.Count <= recipeItems.Count)
            {
                if (!currentItems.Contains(itemName))
                {
                    print("addeditem");
                    currentItems.Add(itemName);
                    currentItemCount++;
                    Destroy(item.gameObject);
                }
            }
        }
        else
        {
            item.GetComponent<Rigidbody2D>().AddForce(transform.right * -100);
        }
        //we know that the number of items will only change if an item is added via this function, so it makes sense to do a crafting ingredients check here
        //does not support multiple of the same items in one recipe. Oh well no biggie
        //iterate over every recipe item and see if we have a matching contained item
        if (!crafting)
        {
            int currentAmountOfValidItems = 0;

            for (int i = 0; i < recipeItems.Count; i++)
            {
                if (currentItems.Contains(recipeItems[i]))
                {
                    currentAmountOfValidItems++;
                    print("has" + recipeItems[i]);
                }
            }
            int requiredAmountOfValidItems = recipeItems.Count;
            if (currentAmountOfValidItems == requiredAmountOfValidItems)
            {
                print("Craft THAT B");
                StartCoroutine(Craft());
            }
        }

    }
    IEnumerator Craft()
    {
        //clear the items count so that it doesnt get too much
        currentItems.Clear();
        crafting = true;
        yield return new WaitForSeconds(0.5f);
        Rigidbody2D result = Instantiate(itemEndPoint, launchPoint.position, launchPoint.rotation).GetComponent<Rigidbody2D>();
        //set launch forece because I don't think the player should be able to modify it
        result.AddForce(launchPoint.up * 500);
        crafting = false;
    }
    */
}
