using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager
{
    public static void SaveData(SaveMaster master, string path)
    {
        DirectoryInfo di = Directory.CreateDirectory(Application.dataPath + "/saves/");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.dataPath + "/saves/" + path + ".europa", FileMode.Create);
        Debug.Log(di.FullName + "_____" + stream.Name);
        AllData data = new AllData(new RelayCannonsData(master), new FansData(master), new CrafterData(master),
            new ItemObjectsData(master), new MinersData(master), new OreData(master), new InventoryData(master), 
            new UnPackagerData(master), new PackagerData(master), new EggData(master), new SplitterData(master),
            new PumpsData(master), new ZapTowerData(master), new RocketData(master));
        bf.Serialize(stream, data);
        //Debug.Log("The directory was created successfully at " + di.Name + path);
        stream.Close();
    }

    public static AllData LoadData(string path)
    {
        Debug.Log(path);
        Debug.Log(Application.dataPath + path + ".europa");
        if (File.Exists(Application.dataPath + path + ".europa"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + path + ".europa", FileMode.Open);


            AllData data = bf.Deserialize(stream) as AllData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("NO FILE AT PATH BROTHER!");
            return new AllData(null, null, null, null, null, null, null, null, null, null, null, null, null, null);
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
    public UnPackagerData unPack;
    public PackagerData pack;
    public EggData egg;
    public SplitterData split;
    public PumpsData pump;
    public ZapTowerData zap;
    public RocketData roc;
    public AllData(RelayCannonsData cannons, FansData fans, CrafterData crafters, ItemObjectsData items, MinersData miners, OreData ores, InventoryData invents, UnPackagerData unPacks,
        PackagerData packs, EggData eggs, SplitterData splits, PumpsData pumps, ZapTowerData zaps, RocketData rock)
    {
        cannon = cannons;
        fan = fans;
        crafter = crafters;
        item = items;
        miner = miners;
        ore = ores;
        invent = invents;
        unPack = unPacks;
        pack = packs;
        egg = eggs;
        split = splits;
        pump = pumps;
        zap = zaps;
        roc = rock;
    }

}

[Serializable]
public class PumpsData
{
    public float[] transforms;
    public int[] storedBarrels;
    int numStats = 0;
    public PumpsData(SaveMaster mast)
    {
        int len = mast.pumps.Length;
        numStats = 3;
        transforms = new float[len * 3];
        storedBarrels = new int[len];

        for(int i = 0; i < len; i++)
        {
            Pump p = mast.pumps[i];
            //x,y,rot lol
            transforms[(i * numStats)] = p.transform.position.x;
            transforms[(i * numStats) + 1] = p.transform.position.y;
            transforms[(i * numStats) + 2] = p.transform.rotation.eulerAngles.z;

            storedBarrels[i] = p.amntContainersStored;
        }
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
    public bool[] eternalMounted;

    public OreData(SaveMaster mast)
    {
        numStats = 7; //posx, y, rot, count of remaining items, color r, g, b
        //actually, its just the product lol
        //actually, thats wrong too lol
        eternalMounted = new bool[mast.ores.Length];
        stats = new string[mast.ores.Length * numStats];
        for(int i = 0; i < mast.ores.Length; i++)
        {
            OreController ore = mast.ores[i];

            eternalMounted[i] = ore.eternal; // now mount eternalizer based on this

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
        numStats = 6; //because what if I add more items?
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
        stats[5] = mast.invent.core.level.ToString();
    }
}

[Serializable]
public class SplitterData
{
    public string[] filters;
    public float[] stats;
    public int numStats = 0;
    public SplitterData(SaveMaster mast)
    {
        numStats = 4; ///posx, posy, rot, force
        stats = new float[mast.splitters.Length * numStats];
        filters = new string[mast.splitters.Length];
        for(int i = 0; i < mast.splitters.Length; i++)
        {
            Splitter split = mast.splitters[i];
            stats[i * numStats] = split.transform.position.x;
            stats[(i * numStats) + 1] = split.transform.position.y;
            stats[(i * numStats) + 2] = split.transform.rotation.eulerAngles.z;
            stats[(i * numStats) + 3] = split.fireForce;
            filters[i] = split.typeName;
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
[Serializable]
public class ZapTowerData
{
    public float[] x;
    public float[] y;
    public float[] z;
    //zaptowers should automatically connect to all local towers
    public ZapTowerData(SaveMaster mast)
    {
        int num = mast.zapTowers.Length;
        x = new float[num];
        y = new float[num];
        z = new float[num];

        for(int i = 0; i < num; i++)
        {
            ZapTower zap = mast.zapTowers[i];
            x[i] = zap.transform.position.x;
            y[i] = zap.transform.position.y;
            z[i] = zap.transform.position.z;
        }
    }
}
[Serializable]
public class PackagerData
{
    public float[] x;
    public float[] y;
    public float[] z;

    public int[] mI;
    public float[] ft;

    public PackagerData(SaveMaster mast)
    {
        int num = mast.packers.Length;
        x = new float[num];
        y = new float[num];
        z = new float[num];

        mI = new int[num];
        ft = new float[num];
        for (int i = 0; i < mast.packers.Length; i++)
        {
            Packager p = mast.packers[i];
            x[i] = p.transform.position.x;
            y[i] = p.transform.position.y;
            z[i] = p.transform.rotation.eulerAngles.z;

            mI[i] = p.maxItems;
            ft[i] = p.launchForce;
        }
    }
}
[Serializable]
public class UnPackagerData
{
    public float[] x;
    public float[] y;
    public float[] z;

    public float[] fF;
    public float[] ft;
    public string[][] allItems;

    public UnPackagerData(SaveMaster mast)
    {
        int num = mast.unPackers.Length;
        x = new float[num];
        y = new float[num];
        z = new float[num];

        fF = new float[num];
        ft = new float[num];
        allItems = new string[num][];
        for(int i = 0; i < num; i++)
        {
            UnPackager uP = mast.unPackers[i];
            Debug.Log(uP.name);
            x[i] = uP.transform.position.x;
            y[i] = uP.transform.position.y;
            z[i] = uP.transform.rotation.eulerAngles.z;

            fF[i] = uP.fireForce;
            ft[i] = uP.fireDelay;
        }
    }
}
[Serializable]
public class EggData
{
    public float[][] vels;
    public float[][] transes;
    public string[][] allItems;

    public EggData(SaveMaster mast)
    {
        int num = mast.capsules.Length;
        vels = new float[num][];
        transes = new float[num][];
        allItems = new string[num][];
        for (int i = 0; i < mast.capsules.Length; i++)
        {
            CapsuleController egg = mast.capsules[i];
            Rigidbody2D rb = egg.GetComponent<Rigidbody2D>();
            float[] v = new float[2];
            v[0] = rb.velocity.x;
            v[1] = rb.velocity.y;
            //Debug.Log(v[0] + ' ' + v[1]);
            vels[i] = v;

            float[] t = new float[3];
            t[0] = egg.transform.position.x;
            t[1] = egg.transform.position.y;
            t[2] = egg.transform.rotation.eulerAngles.z;
            transes[i] = t;

            allItems[i] = new ItemListSave(egg.stringIts).items.ToArray();  
        }
    }
}
[Serializable]
public class RocketData
{
    public float[][] transes;
    public int[][] invents;

    public RocketData(SaveMaster mast)
    {
        int num = mast.rockets.Length;
        transes = new float[num][];
        invents = new int[num][];

        for(int i = 0; i < num; i++)
        {
            RocketBase rocket = mast.rockets[i];
            float[] t = new float[3];
            t[0] = rocket.transform.position.x;
            t[1] = rocket.transform.position.y;
            t[2] = rocket.transform.rotation.eulerAngles.z;
            transes[i] = t;

            int[] its = new int[3];
            its[0] = rocket.storedIron;
            its[1] = rocket.storedCopper;
            its[2] = rocket.storedFuel;

            invents[i] = its;
        }
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

