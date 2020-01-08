using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Crafting : MonoBehaviour
{
    [SerializeField] Transform launchPoint;
    public ScriptableRecipe recipe;
    public List<string> currentItems;
    List<int> currentAmounts;
    List<string> currentRecipe;
    bool crafting = false;
    [SerializeField] SpriteRenderer recipeDisplay;
    [SerializeField] GameInfo info;
    [SerializeField] SpriteRenderer display;
    [Header("bypass info auto recipe")]
    [SerializeField] ScriptableRecipe bypassRecipe;
    private void Awake()
    {
        info = GameObject.FindObjectOfType<GameInfo>();

        if (bypassRecipe)
        {
            ChangeRecipe(bypassRecipe);
        }
        else
        {
            ChangeRecipe(info.storedRecipe);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == "Item")
        {
            AddItemToCrafter(other.GetComponent<Item>());
        }

    }
    public void AddItemToCrafter(Item item)
    {
        bool reject = true;
        //add the item to the storage only if it is in the recipe
        if (currentRecipe.Contains(item.typeOfItem))
        {

            //if the recipe still has capacity for that type of item
            int indexOfAmount = currentRecipe.IndexOf(item.typeOfItem);

            if (currentAmounts[indexOfAmount] > 0)
            {
                UpdateDisplayColor(1);
                currentAmounts[indexOfAmount] -= 1;
                
                //purely for the purpose of saving/loading, update the amount of items in the crafter
                currentItems.Add(item.typeOfItem);
                Destroy(item.gameObject);
                reject = false;
            }
        }
        if (reject)
        {
            item.GetComponent<Rigidbody2D>().AddForce(transform.up * -400);
        }
        CheckCraft();
    }
    void UpdateDisplayColor(int val)
    {
        if (val == 0)
        {
            display.color = new Color(1, 0, 0, 1);
        }
        else if (val == 1)
        {
            display.color = new Color(1, 1, 0, 1);
        }
        else if (val == 2)
        {
            display.color = new Color(0, 1, 0, 1);
        }
    }
    void CheckCraft()
    {
        
        int satisfactionRemaining = currentAmounts.Sum();
        if(satisfactionRemaining == 0)
        {
            UpdateDisplayColor(2);
            StartCoroutine(Craft());
            currentAmounts = new List<int>();
            for (int i = 0; i < recipe.input.Count; i++)
            {

                string temp = recipe.input[i].Split(',')[0];
                currentAmounts.Add(int.Parse(temp) + 1);

                //finish resetting
            }
        }

        /*
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
            StartCoroutine(Craft());
            //for every result, instantiate and launch it

        }
        */

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
            //result.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360)); //It has been decided that this randomness was introducing too many problems with fan chains
            yield return new WaitForSeconds(0.1f);
        }
        UpdateDisplayColor(0);

        //set launch forece because I don't think the player should be able to modify it
        crafting = false;
    }
    public void ChangeRecipe(ScriptableRecipe r)
    {
        recipe = r;
        recipeDisplay.sprite = r.img;
        recipeDisplay.color = r.imgColor;
        //also set a stored default recipe so that it is easier to make a lot of the same machine
        if (r)
        {
            info.storedRecipe = r;
        }
        //set recipe string list and int list
        currentRecipe = new List<string> ();
        currentAmounts = new List<int> ();
        currentRecipe.Capacity = recipe.input.Count;
        currentAmounts.Capacity = recipe.input.Count;

        for (int i = 0; i < recipe.input.Count; i++)
        {
            string temp = recipe.input[i].Split(',')[0];
            currentAmounts.Add(int.Parse(temp) + 1);
            currentRecipe.Add(recipe.input[i].Split(',')[1]);
        }
        //the way this works means that you must order your recipes as "1,sand 2,rock etc"
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
