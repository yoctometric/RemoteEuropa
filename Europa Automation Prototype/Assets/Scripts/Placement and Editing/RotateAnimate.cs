using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimate : MonoBehaviour
{
    [SerializeField] float speed;
    private void FixedUpdate()
    {
        transform.Rotate(0, 0, speed);
    }
}
