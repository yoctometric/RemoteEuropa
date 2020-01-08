using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveMaster : MonoBehaviour
{
    //data
    public LauncherController[] cannons;
    public GameObject[] fans;
    public Crafting[] crafters;
    public Item[] items;
    public Miner[] miners;
    public OreController[] ores;
    public Inventory invent;
    public Packager[] packers;
    public UnPackager[] unPackers;
    public CapsuleController[] capsules;
    public Splitter[] splitters;
    public Pump[] pumps;
    //prefabs
    public LauncherController cannonPrefab;
    public Transform fanPrefab;
    public Crafting crafterPrefab;
    public Miner minerPrefab;
    public OreController orePrefab;
    public CapsuleController eggPrefab;
    public UnPackager unPackPrefab;
    public Packager packPrefab;
    public Splitter splitterPrefab;
    public Pump pumpPrefab;
    //functions

    private void Start()
    {
        //stop from going away
        DontDestroyOnLoad(this.gameObject);
    }
    public void SaveGame(string path)
    {
        //save the launchers
        cannons = GameObject.FindObjectsOfType<LauncherController>();
        fans = GameObject.FindGameObjectsWithTag("Fan");
        crafters = GameObject.FindObjectsOfType<Crafting>();
        items = GameObject.FindObjectsOfType<Item>();
        miners = GameObject.FindObjectsOfType<Miner>();
        ores = GameObject.FindObjectsOfType<OreController>();
        invent = GameObject.FindObjectOfType<Inventory>();
        packers = GameObject.FindObjectsOfType<Packager>();
        unPackers = GameObject.FindObjectsOfType<UnPackager>();
        capsules = GameObject.FindObjectsOfType<CapsuleController>();
        splitters = GameObject.FindObjectsOfType<Splitter>();
        pumps = GameObject.FindObjectsOfType<Pump>();

        SaveLoadManager.SaveData(this, path);
    }

    public void LoadGame(string path)
    {

        //now reload scene
        StartCoroutine(Load("BlankScene", path));

    }
    IEnumerator Load(string scene, string path)
    {
        //wait until the scene is loaded to instantiate, otherwise shit gets overwritten
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (!async.isDone)
        {
            yield return new WaitForSeconds(1f);
            yield return new WaitForEndOfFrame();
        }
        //now that the scene is loaded, send it brother
        AllData allData = SaveLoadManager.LoadData(path);
        //Launchers first
        Cannons(allData);
        Fans(allData);
        Crafters(allData);
        Items(allData);
        Miners(allData);
        Ores(allData);
        LoadInventory(allData);
        UnPackers(allData);
        Packers(allData);
        Eggs(allData);
        Splitters(allData);
        Pumps(allData);
    }

    void Cannons(AllData allData)
    {
        if(allData.cannon != null)
        {
            float[] cannonData = allData.cannon.stats;
            int num = allData.cannon.numStats;
            int cannonAmount = cannonData.Length / num;//because every n'th element starts a new box
            for (int i = 0; i < cannonAmount; i++)
            {
                LauncherController cannon = Instantiate(cannonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cannon.GetComponent<Price>().byPass = true;
                cannon.transform.position = new Vector3(cannonData[(i * num)], cannonData[(i * num) + 1], 0);
                cannon.transform.rotation = Quaternion.Euler(0, 0, cannonData[(i * num) + 2]);
                cannon.coolDown = cannonData[(i * num) + 3];
                cannon.launchForce = cannonData[(i * num) + 4];
                cannon.launchRotationVertex.rotation = Quaternion.Euler(0, 0, cannonData[(i * num) + 5]);
            }
        }
    }

    void Fans(AllData allData)
    {
        if (allData.fan != null)
        {
            float[] fansData = allData.fan.stats;
            int num = allData.fan.numStats;
            int fanAmount = fansData.Length / num;//because every n'th element starts a new box
            for (int i = 0; i < fanAmount; i++)
            {
                Transform fan = Instantiate(fanPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                fan.GetComponent<Price>().byPass = true;

                fan.position = new Vector3(fansData[(i * num)], fansData[(i * num) + 1], 0);
                fan.rotation = Quaternion.Euler(0, 0, fansData[(i * num) + 2]);
            }
        }
    }

    void Crafters(AllData allData)
    {
        if(allData.crafter != null)
        {
            float[] craftersData = allData.crafter.stats;
            int num = allData.crafter.numStats;
            int crafterAmount = craftersData.Length / num;//because every n'th element starts a new box
            string[][] items = allData.crafter.allItems;
            for (int i = 0; i < crafterAmount; i++)
            {
                Crafting crafter = Instantiate(crafterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                crafter.GetComponent<Price>().byPass = true;
                crafter.transform.position = new Vector3(craftersData[(i * num)], craftersData[(i * num) + 1], 0);
                crafter.transform.rotation = Quaternion.Euler(0, 0, craftersData[(i * num) + 2]);
                crafter.ChangeRecipe(StaticFunctions.GetRecipeFromIndex(Mathf.RoundToInt(craftersData[(i * num) + 3])));

                //now give it items
                for (int j = 0; j < items[i].Length; j++)
                {
                    Item it = Instantiate(StaticFunctions.GetItemFromString(items[i][j]), transform.position, Quaternion.identity);
                    crafter.AddItemToCrafter(it);
                }
            }
        }
    }

    void Items(AllData allData)
    {
        if(allData.item != null)
        {
            string[] itemsData = allData.item.stats;
            int num = allData.item.numStats;
            int amount = itemsData.Length / num;//because every n'th element starts a new item
            for (int i = 0; i < amount; i++)
            {
                Item it = Instantiate(StaticFunctions.GetItemFromString(itemsData[(i * num) + 5]), Vector3.zero, Quaternion.identity);

                it.transform.position = new Vector3(float.Parse(itemsData[(i * num)]), float.Parse(itemsData[(i * num) + 1]), 0);
                it.transform.rotation = Quaternion.Euler(0, 0, float.Parse(itemsData[(i * num) + 2]));

                Rigidbody2D rb = it.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector3(float.Parse(itemsData[(i * num) + 3]), float.Parse(itemsData[(i * num) + 4]), 0);
            }
        }
    }

    void Miners(AllData allData)
    {
        if(allData.miner != null)
        {
            float[] data = allData.miner.stats;
            int num = allData.miner.numStats;
            int amount = data.Length / num;//because every n'th element starts a new item
            for (int i = 0; i < amount; i++)
            {
                Miner min = Instantiate(minerPrefab, Vector3.zero, Quaternion.identity);
                min.GetComponent<Price>().byPass = true;
                min.transform.position = new Vector3(data[(i * num)], data[(i * num) + 1], 0);
                min.transform.rotation = Quaternion.Euler(0, 0, data[(i * num) + 2]);
                min.launchForce = data[(i * num) + 3];
            }
        }
    }

    void Ores(AllData allData)
    {
        if (allData.ore != null)
        {
            string[] data = allData.ore.stats;
            int num = allData.ore.numStats;
            int amount = data.Length / num;//because every n'th element starts a new item
            for (int i = 0; i < amount; i++)
            {
                OreController o = Instantiate(StaticFunctions.GetOreFromString(data[(i * num) + 4]), Vector3.zero, Quaternion.identity);
                o.transform.position = new Vector3(float.Parse(data[(i * num)]), float.Parse(data[(i * num) + 1]), 0);
                o.transform.rotation = Quaternion.Euler(0, 0, float.Parse(data[(i * num) + 2]));
                o.currentQuantity = int.Parse(data[(i * num) + 3]);
                o.quantity = int.Parse(data[(i * num) + 3]);
                //string[] color = data[(i * num) + 4].Split(',');
                //o.oreColor = new Color(float.Parse(color[0]), float.Parse(color[1]), float.Parse(color[2]));
                //o.product = StaticFunctions.GetItemFromString(data[(i * num) + 5]);

                o.transform.localScale = new Vector2(float.Parse(data[(i * num) + 5]), float.Parse(data[(i * num) + 6]));
            }
        }

    }
    
    void LoadInventory(AllData allData)
    {
        if(allData.invent != null)
        {
            Inventory inv = GameObject.FindObjectOfType<Inventory>();
            string[] data = allData.invent.stats;
            for (int i = 0; i < data.Length - 1; i++)
            {
                if (data[i] != null)
                {
                    string k = data[i].Split(',')[0];//key
                    int v = int.Parse(data[i].Split(',')[1]);//value
                    inv.storedVals[k] = v;
                }
            }
            for (int i = 0; i < int.Parse(data[5]); i++){
                inv.core.Upgrade(true);
            }

            inv.UpdateAllInventories();
        }
        else
        {
            print("no inventory found");
        }
    }

    void UnPackers(AllData allData)
    {
        if (allData.unPack.x != null)
        {
            float[] x = allData.unPack.x;
            float[] y = allData.unPack.y;
            float[] z = allData.unPack.z;

            float[] fF = allData.unPack.fF;
            float[] ft = allData.unPack.ft;
            for (int i = 0; i < allData.unPack.x.Length; i++)
            {
                UnPackager uP = Instantiate(unPackPrefab, Vector3.zero, Quaternion.identity);
                uP.GetComponent<Price>().byPass = true;
                uP.transform.position = new Vector3(x[i], y[i], 0);
                uP.transform.rotation = Quaternion.Euler(0, 0, z[i]);
                uP.fireForce = fF[i];
            }
        }
    }

    void Packers(AllData allData)
    {
        if(allData.pack.x.Length > 0)
        {
            float[] x = allData.pack.x;
            float[] y = allData.pack.y;
            float[] z = allData.pack.z;

            int[] mI = allData.pack.mI;
            float[] ft = allData.pack.ft;
            for (int i = 0; i < allData.pack.x.Length; i++)
            {
                Packager p = Instantiate(packPrefab, Vector3.zero, Quaternion.identity);
                p.GetComponent<Price>().byPass = true;
                p.transform.position = new Vector3(x[i], y[i], 0);
                p.transform.rotation = Quaternion.Euler(0, 0, z[i]);
                p.maxItems = (mI[i]);
                p.launchForce = Mathf.RoundToInt(ft[i]);
            }
        }
    }

    void Splitters(AllData allData)
    {
        if(allData.split.filters.Length > 0)
        {
            string[] filts = allData.split.filters;
            float[] stats = allData.split.stats;
            int num = allData.split.numStats;
            for(int i  = 0; i < allData.split.filters.Length; i++)
            {
                Splitter sp = Instantiate(splitterPrefab, Vector3.zero, Quaternion.identity);
                sp.GetComponent<Price>().byPass = true;
                sp.transform.position = new Vector3(stats[(i * num)], stats[(i * num) + 1], 0);
                sp.transform.rotation = Quaternion.Euler(0, 0, stats[(i * num) + 2]);
                sp.fireForce = Mathf.RoundToInt(stats[(i * num) + 3]);

                sp.typeName = filts[i];
            }

        }
    }
    void Eggs(AllData allData)
    {

        float[][] vels = allData.egg.vels;
        float[][] transes = allData.egg.transes;
        string[][] allItems = allData.egg.allItems;

        for (int i = 0; i < allData.egg.vels.Length; i++)
        {
            CapsuleController e = Instantiate(eggPrefab, Vector3.zero, Quaternion.identity);
            e.transform.position = new Vector3(transes[i][0], transes[i][1], 0);
            e.transform.rotation = Quaternion.Euler(0, 0, transes[i][2]);
            Rigidbody2D rb = e.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(vels[i][0], vels[i][1]);
            for (int j = 0; j < allItems[i].Length; j++)
            {
                Item it = Instantiate(StaticFunctions.GetItemFromString(allItems[i][j]), transform.position, Quaternion.identity);
                e.AddItem(it);
            }
        }
    }

    void Pumps(AllData allData)
    {
        float[] transes = allData.pump.transforms;
        int num = 3;
        int[] invent = allData.pump.storedBarrels;

        for(int i = 0; i < allData.pump.storedBarrels.Length; i++)
        {
            Pump p = Instantiate(pumpPrefab, Vector3.zero, Quaternion.Euler(0, 0, transes[(i * num) + 2]));
            p.transform.position = new Vector2(transes[(i * num)], transes[(i * num) + 1]);

            p.amntContainersStored = invent[i];
        }
    }
}
