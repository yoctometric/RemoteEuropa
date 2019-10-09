using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateWorld : MonoBehaviour{

    //allows the placement of an environment obj spread over a radial area. How do I stop overlaps? Should I?
    public void Populate(GameObject obj, int count, float radius, bool randomRot)
    {
        for(int i = 0; i < count; i++)
        {
            Quaternion rot = Quaternion.identity;
            if (randomRot)
            {
                rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }
            Instantiate(obj, Random.insideUnitCircle * radius, rot);
        }
    }
}
