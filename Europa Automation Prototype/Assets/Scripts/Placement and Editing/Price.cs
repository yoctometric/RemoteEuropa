using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Price : MonoBehaviour
{
    //global to prevent refunding an item u cant afford
    bool refundable = true;
    [SerializeField] string[] costs = new string[1];
    private void Start()
    {
        //price format is "1,Pycrete"
        Inventory invent = GameObject.FindObjectOfType<Inventory>();
        //check if price is doable
        List<bool> canAfford = new List<bool>(costs.Length);
        for (int i = 0; i < costs.Length; i++)
        {
            string index = costs[i].Split(',')[1];
            int amnt = int.Parse(costs[i].Split(',')[0]);
            //check if you cant subtract
            if(invent.storedVals[costs[i].Split(',')[1]] + int.Parse(costs[i].Split(',')[0]) < 0)
            {
                //since u cant afford,
                canAfford.Add(false);
            }
            else
            {
                //can afford, so
                canAfford.Add(true);
            }
        }

        for (int i = 0; i < canAfford.Count; i++)
        {
            string index = costs[i].Split(',')[1];
            int amnt = int.Parse(costs[i].Split(',')[0]);
            //if you cant afford it...
            if (!canAfford[i]) { 
                bool placed = invent.UpdateInventory(costs[i].Split(',')[1], int.Parse(costs[i].Split(',')[0]));
                refundable = false;
                Destroy(gameObject);
            }
            else
            {
                //you can afford THIS item, but can you afford the rest?
                if (!canAfford.Contains(false))
                {
                    //then...
                    invent.UpdateInventory(costs[i].Split(',')[1], int.Parse(costs[i].Split(',')[0]));
                }
            }
        }
    }

    //refund
    private void OnDestroy()
    {
        if (refundable)
        {
            for (int i = 0; i < costs.Length; i++)
            {
                Inventory invent = GameObject.FindObjectOfType<Inventory>();
                string index = costs[i].Split(',')[1];
                int amnt = int.Parse(costs[i].Split(',')[0]);
                //add the negative cost
                if (invent)
                {
                    invent.UpdateInventory(index, -1 * amnt);
                }
            }
        }
    }
}
