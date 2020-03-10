using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class OreWeightPair
{
    public int weight;
    public OreController ore;
}
public class WorldGen : MonoBehaviour
{
    public bool doneGenerating = false;
    public OreWeightPair[] ores;
    [SerializeField] int numberOf;
    [SerializeField] int r;
    [HideInInspector] public List<GameObject> GeneratedOres;
    SaveMaster mast;
    WorldGenParameterContainer cont;

    public void CancelGen()
    {
        StopCoroutine(ScatterOres(0, 0, 0));
        print("cancelling");
        doneGenerating = true;
        foreach(GameObject g in GeneratedOres)
        {
            Destroy(g);
        }
    }
    private void Start()
    {
        mast = GameObject.FindObjectOfType<SaveMaster>();
        GeneratedOres = new List<GameObject>();
        //look for data set
        if (GameObject.FindObjectOfType<WorldGenParameterContainer>())
        {
            cont = GameObject.FindObjectOfType<WorldGenParameterContainer>();

            //update pairs. Copper, rock, ice, iron
            ores[0].weight = cont.copperWeight;
            ores[1].weight = cont.rockWeight;
            ores[2].weight = cont.iceWeight;
            ores[3].weight = cont.ironWeight;

            StartCoroutine(ScatterOres(cont.AttemptedOres, r, cont.distanceBonus));
        }
        else
        {
            print("NO PARAMETERS FOUND");
        }
    }
    
    IEnumerator ScatterOres(int amount, int radius, float distBonus)
    {
        //yield return new WaitForSeconds(1f); //testing says this doesnt matter
        //check if there are already ores
        if (GameObject.FindObjectOfType<OreController>())
        {
            GameObject.FindObjectOfType<Transition>().GetComponent<Animator>().SetBool("Generating", false);
            yield break;
        }
        if (GameObject.FindObjectOfType<Transition>())
        {
            GameObject.FindObjectOfType<Transition>().GetComponent<Animator>().SetBool("Generating", true);
        }
        List<int> weights = new List<int>();
        for(int i = 0; i < ores.Length; i++)
        {
            weights.Add(ores[i].weight);
        }
        
        weights.Sort();//so that it actually works
        
        int maxVal = 0;
        //sum the values. This will let u set how likely the ore will be by percentile
        for (int i = 0; i < weights.Count; i++) { maxVal += weights[i];}

        //make a list of ints based on each weight
        List<int> weightedIntList = new List<int>();
        for(int i = 0; i < weights.Count; i++)
        {
            for(int j = 0; j < weights[i]; j++)
            {
                weightedIntList.Add(i); //adds the index. So, it will add a bunch of 0's, a couplea 1's, etc etc
                print(i);
            }
        }
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForEndOfFrame(); // this is actually nescessary to prevent overlapping, believe it or not
            //check to cancel
            if (mast.loading)
            {
                CancelGen();
                yield break;
            }
            int choice = weightedIntList[Random.Range(0, weightedIntList.Count)];
            //print(choice + " . . . " + weightedIntList.Count);
            PlaceOre(ores[choice].ore, distBonus);
        }
        //this is where it ends
        doneGenerating = true;
        GameObject.FindObjectOfType<Transition>().GetComponent<Animator>().SetBool("Generating", false);
    }

    void PlaceOre(OreController o, float distBonus)
    {
        Vector3 pos = Random.insideUnitCircle * r;
        float dist = Vector3.Distance(Vector3.zero, pos);
        float rot = Random.Range(0, 360);
        bool over = Physics2D.OverlapCircle(pos, dist * 0.05f);
        if (!over)
        {
            OreController t = Instantiate(o.gameObject, Vector3.zero, Quaternion.Euler(0, 0, rot)).GetComponent<OreController>();
            t.transform.position = pos;
            t.transform.localScale *= Mathf.Clamp((dist * 0.02f), 0.5f, 100);

            t.quantity = Mathf.RoundToInt(t.quantity * dist * dist *0.0005f * (distBonus) / 2);
            t.currentQuantity = Mathf.RoundToInt(t.currentQuantity * dist * dist *0.0005f);
            GeneratedOres.Add(t.gameObject);
        }

    }
}