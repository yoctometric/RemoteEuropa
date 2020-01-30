using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
    [SerializeField] Animator dropPodAnim;

    public int ironSinceLastCheck = 0;
    public int copperSinceLastCheck = 0;
    public int brickSinceLastCheck = 0;
    public int pycreteSinceLastCheck = 0;

    float ironRate = 0;
    float copperRate = 0;
    float brickRate = 0;
    float pycreteRate = 0;

    void Start()
    {
        StartCoroutine(AnalyzeLoop()); //always run it. It will update twice per minute
    }

    IEnumerator AnalyzeLoop()
    {
        ironSinceLastCheck = 0;
        copperSinceLastCheck = 0;
        brickSinceLastCheck = 0;
        pycreteSinceLastCheck = 0;

        yield return new WaitForSeconds(10f);

        ironRate = ironSinceLastCheck / 10f; //iron per second
        copperRate = copperSinceLastCheck / 10f; //cops per second
        brickRate = brickSinceLastCheck / 10f; //bricks per second
        pycreteRate = pycreteSinceLastCheck / 10f; //pycrete per second

        print("Iron: " + ironRate.ToString() + " Copper: " + copperRate.ToString() + " Brick: " + brickRate.ToString() + " Pycrete: " + pycreteRate.ToString());
        StartCoroutine(AnalyzeLoop()); //always run it. It will update twice per minute
    }
}
