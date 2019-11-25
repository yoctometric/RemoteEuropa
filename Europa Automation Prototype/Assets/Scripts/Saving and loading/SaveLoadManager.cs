﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager
{
    public static void SaveData(SaveMaster master, string path)
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + path + ".europa", FileMode.Create);

        AllData data = new AllData(new RelayCannonsData(master), new FansData(master), new CrafterData(master), new ItemObjectsData(master), new MinersData(master), new OreData(master), new InventoryData(master));
        bf.Serialize(stream, data);

        stream.Close();
    }

    public static AllData LoadData(string path)
    {
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/" + path + ".europa"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + path + ".europa", FileMode.Open);


            AllData data = bf.Deserialize(stream) as AllData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("NO FILE AT PATH BROTHER!");
            return new AllData(null, null, null, null, null, null, null);
        }
    }
}

[Serializable]
public class AllData
{
    //place all classes that are being saved into here, and then save alldata to the file!
    public RelayCannonsData cannon;
    public FansData fan;
    public CrafterData crafter;
    public ItemObjectsData item;
    public MinersData miner;
    public OreData ore;
    public InventoryData invent;
    public AllData(RelayCannonsData cannons, FansData fans, CrafterData crafters, ItemObjectsData items, MinersData miners, OreData ores, InventoryData invents)
    {
        cannon = cannons;
        fan = fans;
        crafter = crafters;
        item = items;
        miner = miners;
        ore = ores;
        invent = invents;
    }

}

[Serializable]
public class RelayCannonsData
{
    public float[] stats;
    public int numStats = 0;

    public RelayCannonsData(SaveMaster mast)
    {
        numStats = 6;
        stats = new float[mast.cannons.Length * numStats];
        for (int i = 0; i < mast.cannons.Length; i++)
        {
            LauncherController cannon = mast.cannons[i];
            //slot 1 is x, 2 is y, 3 is z rotation
            stats[(i * numStats)] = cannon.transform.position.x;
            stats[(i * numStats) + 1] = cannon.transform.position.y;
            stats[(i * numStats) + 2] = cannon.transform.eulerAngles.z;
            //next slot is cooldown, then comes launchforce. Currently hoping to avoid saving their stored items, b/c that sounds really hard
            stats[(i * numStats) + 3] = cannon.coolDown;
            stats[(i * numStats) + 4] = cannon.launchForce;
            stats[(i * numStats) + 5] = cannon.launchRotationVertex.rotation.eulerAngles.z;
        }
    }
}

[Serializable]
public class FansData
{
    public float[] stats;
    public int numStats = 0;

    public FansData(SaveMaster mast)
    {
        numStats = 3;
        stats = new float[mast.fans.Length * numStats];
        for (int i = 0; i < mast.fans.Length; i++)
        {
            Transform fan = mast.fans[i].transform;
            stats[(i * numStats)] = fan.transform.position.x;
            stats[(i * numStats) + 1] = fan.transform.position.y;
            stats[(i * numStats) + 2] = fan.transform.rotation.eulerAngles.z;
        }
    }
}
[Serializable]
public class OreData
{
    public string[] stats;
    public int numStats = 0;

    public OreData(SaveMaster mast)
    {
        numStats = 7; //posx, y, rot, count of remaining items, color r, g, b
        //actually, its just the product lol
        //actually, thats wrong too lol
        stats = new string[mast.ores.Length * numStats];
        for(int i = 0; i < mast.ores.Length; i++)
        {
            OreController ore = mast.ores[i];
            
            stats[(i * numStats)] = ore.transform.position.x.ToString();
            stats[(i * numStats) + 1] = ore.transform.position.y.ToString();
            stats[(i * numStats) + 2] = ore.transform.rotation.eulerAngles.z.ToString();
            //numitems
            stats[(i * numStats) + 3] = ore.currentQuantity.ToString();
            //item type
            stats[(i * numStats) + 4] = ore.product.GetComponent<Item>().typeOfItem;//for getting the prefab later
            //scale mod
            
            stats[(i * numStats) + 5] = ore.transform.localScale.x.ToString();
            stats[(i * numStats) + 6] = ore.transform.localScale.y.ToString();
        }
    }
}
[Serializable]
public class InventoryData
{
    public int numStats = 0;
    //CANNNOT SERIALIZE DICTIONARY! MAKE CUSTOM CLASS OR STRING LIST SILLY
    public string[] stats;

    public InventoryData(SaveMaster mast)
    {
        numStats = 3; //because what if I add more items?
        stats = new string[numStats];
        int i = 0;
        foreach(KeyValuePair<string, int> keyVal in mast.invent.storedVals)
        {
            stats[i] = keyVal.Key + "," + keyVal.Value.ToString(); //"Key,Value"
            if (i > numStats)
            {
                Debug.LogError("More keys in storedvals dictionary than are allowed to be saved! Increase numstats");
            }
            i++;
        }
    }
}



[Serializable] 
public class CrafterData
{
    public float[] stats;
    public int numStats = 0;
    public string[][] allItems;
    public CrafterData(SaveMaster mast)
    {
        numStats = 4;
        stats = new float[mast.crafters.Length * numStats];
        allItems = new string[mast.crafters.Length][];
        for (int i = 0; i < mast.crafters.Length; i++)
        {
            Crafting crafter = mast.crafters[i];
            stats[(i * numStats)] = crafter.transform.position.x;
            stats[(i * numStats) + 1] = crafter.transform.position.y;
            stats[(i * numStats) + 2] = crafter.transform.rotation.eulerAngles.z;
            stats[(i * numStats) + 3] = crafter.recipe.id;
            //adds item list to the index in the parent list.
            allItems[i] = new ItemListSave(crafter.currentItems).items.ToArray();
        }
    }
}
[Serializable] 
public class ItemObjectsData
{
    public string[] stats;
    public int numStats = 0;

    public ItemObjectsData(SaveMaster mast)
    {
        numStats = 6;
        stats = new string[mast.items.Length * numStats];
        //x, y, rot, xV, yV, type.
        for (int i = 0; i < mast.items.Length; i++)
        {
            //Debug.Log(mast.items[i].name);
            Item item = mast.items[i];
            //Debug.Log(item.name);

            stats[(i * numStats)] = item.transform.position.x.ToString();
            stats[(i * numStats) + 1] = item.transform.position.y.ToString();
            stats[(i * numStats) + 2] = item.transform.rotation.eulerAngles.z.ToString();
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            stats[(i * numStats) + 3] = rb.velocity.x.ToString();
            stats[(i * numStats) + 4] = rb.velocity.y.ToString();
            stats[(i * numStats) + 5] = item.typeOfItem;
        }
    }
}
[Serializable]
public class MinersData
{
    public float[] stats;
    public int numStats = 0;
    
    public MinersData(SaveMaster mast)
    {
        numStats = 4;
        stats = new float[mast.miners.Length * numStats];
        for (int i = 0; i < mast.miners.Length; i++)
        {
            Miner miner = mast.miners[i];
            stats[(i * numStats)] = miner.transform.position.x;
            stats[(i * numStats) + 1] = miner.transform.position.y;
            stats[(i * numStats) + 2] = miner.transform.rotation.eulerAngles.z;
            stats[(i * numStats) + 3] = miner.launchForce;
        }
    }
}
[Serializable]
public class ItemListSave
{
    public List<string> items;
    
    public ItemListSave(List<string> its)
    {

        items = its;
        items.ToArray();
    }
}
/*    public float[] transforms;
    public BoxDatas(SaveMaster mast)
    {
        transforms = new float[mast.boxes.Length * 9];
        for (int i = 0; i < mast.boxes.Length; i++)
        {
            //every six numbers, move to the next object
            transforms[(i * 9)] = mast.boxes[i].transform.position.x;
            transforms[(i * 9) + 1] = mast.boxes[i].transform.position.y;
            transforms[(i * 9) + 2] = mast.boxes[i].transform.position.z;

            transforms[(i * 9) + 3] = mast.boxes[i].transform.rotation.x;
            transforms[(i * 9) + 4] = mast.boxes[i].transform.rotation.y;
            transforms[(i * 9) + 5] = mast.boxes[i].transform.rotation.z;

            transforms[(i * 9) + 6] = mast.boxes[i].GetComponent<Rigidbody>().velocity.x;
            transforms[(i * 9) + 7] = mast.boxes[i].GetComponent<Rigidbody>().velocity.y;
            transforms[(i * 9) + 8] = mast.boxes[i].GetComponent<Rigidbody>().velocity.z;
        }
    }
*/

