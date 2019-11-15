using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMaster : MonoBehaviour
{
    //data
    public LauncherController[] cannons;
    public GameObject[] fans;
    public Crafting[] crafters;

    //prefabs
    public LauncherController cannonPrefab;
    public Transform fanPrefab;
    public Crafting crafterPrefab;
    //functions

    public void SaveGame()
    {
        //save the launchers
        cannons = GameObject.FindObjectsOfType<LauncherController>();
        fans = GameObject.FindGameObjectsWithTag("Fan");
        crafters = GameInfo.FindObjectsOfType<Crafting>();
        //debug
        print(crafters[0].currentItems[0]);
        SaveLoadManager.SaveData(this, "test");

    }

    public void LoadGame()
    {
        AllData allData = SaveLoadManager.LoadData("test");
        //Launchers first
        Cannons(allData);
        Fans(allData);
        Crafters(allData);
    }

    void Cannons(AllData allData)
    {
        float[] cannonData = allData.cannon.stats;
        int num = allData.cannon.numStats;
        int cannonAmount = cannonData.Length / num;//because every n'th element starts a new box
        for (int i = 0; i < cannonAmount; i++)
        {
            LauncherController cannon = Instantiate(cannonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            cannon.transform.position = new Vector3(cannonData[(i*num)], cannonData[(i * num) + 1], 0);
            cannon.transform.rotation = Quaternion.Euler(0, 0, cannonData[(i * num) + 2]);
            cannon.coolDown = cannonData[(i * num) + 3];
            cannon.launchForce = cannonData[(i * num) + 4];
            cannon.launchRotationVertex.rotation = Quaternion.Euler(0, 0, cannonData[(i * num) + 5]);
        }
    }

    void Fans(AllData allData)
    {
        float[] fansData = allData.fan.stats;
        int num = allData.fan.numStats;
        int fanAmount = fansData.Length / num;//because every n'th element starts a new box
        for (int i = 0; i < fanAmount; i++)
        {
            Transform fan = Instantiate(fanPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            fan.position = new Vector3(fansData[(i * num)], fansData[(i * num) + 1], 0);
            fan.rotation = Quaternion.Euler(0, 0, fansData[(i * num) + 2]);
        }
    }

    void Crafters(AllData allData)
    {
        float[] craftersData = allData.crafter.stats;
        int num = allData.crafter.numStats;
        int crafterAmount = craftersData.Length / num;//because every n'th element starts a new box
        string[][] items = allData.crafter.allItems;
        for (int i = 0; i < crafterAmount; i++)
        {
            Crafting crafter = Instantiate(crafterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            crafter.transform.position = new Vector3(craftersData[(i * num)], craftersData[(i * num) + 1], 0);
            crafter.transform.rotation = Quaternion.Euler(0, 0, craftersData[(i * num) + 2]);
            crafter.ChangeRecipe(StaticFunctions.GetRecipeFromIndex(Mathf.RoundToInt(craftersData[(i * num) + 3])));

            //now give it items
            for(int j = 0; j < items[i].Length; j++)
            {
                print(items[i][j]);
                Item it = Instantiate(StaticFunctions.GetItemFromString(items[i][j]), transform.position, Quaternion.identity);
                print(it.name);
                crafter.AddItemToCrafter(it);
            }
        }
    }
}
