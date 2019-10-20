using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShootTubeAiming : MonoBehaviour
{
    public bool laserSight = false;
    LineRenderer lr;

    [SerializeField] float lineW;

    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (laserSight)
        {
            Vector3 tPos = transform.up * 1000;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, tPos);
        }
    }

    public void ToggleAimerOn()
    {
        lr.endWidth = lineW;
        lr.startWidth = lineW;
        laserSight = true;
    }
    public void ToggleAimerOff()
    {
        lr.endWidth = 0;
        lr.startWidth = 0;
        laserSight = false;
    }
}
