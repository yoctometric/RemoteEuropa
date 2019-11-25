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
    public OreWeightPair[] ores;
    [SerializeField] int numberOf;
    [SerializeField] int r;
    [SerializeField] float distBon;
    private void Start()
    { 
        StartCoroutine(ScatterOres(numberOf, r, distBon));
    }

    IEnumerator ScatterOres(int amount, int radius, float distBonus)
    {
        yield return new WaitForSeconds(4);
        //check if there are already ores
        if (GameObject.FindObjectOfType<OreController>())
        {
            yield break;
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
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForEndOfFrame();

            int choice = Random.Range(0, maxVal);
            //go over every key. if it is less than the count, instantiate
            for(int j = 0; j < weights.Count; j++)
            {
                if(weights[j] >= choice)
                {
                    PlaceOre(ores[j].ore);
                    break;
                }
            }
        }

    }

    void PlaceOre(OreController o)
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

            t.quantity = Mathf.RoundToInt(t.quantity * dist * dist *0.0005f);
            t.currentQuantity = Mathf.RoundToInt(t.currentQuantity * dist * dist *0.0005f);

        }

    }
}