using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditRotation : MonoBehaviour
{
    public Transform rotateable;
    public float rotBounds;
    Vector3 rotOfChild;
    private void Update()
    {

        if(rotBounds != 0)
        {
            float zRot = Mathf.Clamp(rotateable.localRotation.eulerAngles.z, -180, 180);
            rotOfChild = new Vector3(0, 0, zRot);
        }
        else
        {
            rotOfChild = new Vector3(0, 0, rotateable.localRotation.eulerAngles.z);

        }
        rotateable.localRotation = Quaternion.Euler(rotOfChild);
    }
}
