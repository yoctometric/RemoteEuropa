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

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + path + ".europa", FileMode.Create);

        AllData data = new AllData(new RelayCannonsData(master), new FansData(master), new CrafterData(master));
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
            return new AllData(null, null, null);
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
    public AllData(RelayCannonsData cannons, FansData fans, CrafterData crafters)
    {
        cannon = cannons;
        fan = fans;
        crafter = crafters;
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
public class ItemListSave
{
    public List<string> items;
    
    public ItemListSave(List<string> its)
    {
        items = its;
        items.ToArray();
        Debug.Log(items[0]);
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

