using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditRotation : MonoBehaviour
{
    public Transform rotateable;
    public float rotBounds;
    Vector3 rotOfChild;
    public bool mouseAim = false;
    Camera cam;
    [SerializeField] float rotOff;
    //EditorValues eVal;
    ObjectPlacer mouse;
    SpriteRenderer sp;
    EditorValues evals;
    private void Start()
    {

        sp = gameObject.GetComponent<SpriteRenderer>();
        cam = Camera.main;
        mouseAim = false;
        evals = gameObject.GetComponent<EditorValues>();//for the laser aiming
        mouse = GameObject.FindObjectOfType<ObjectPlacer>();
    }
    public void StartMouseAim()
    {
        mouseAim = true;
        mouse.previousIndex = 1;
        mouse.SetIndex(1);
        if (evals.aimer)
        {
            print("on");
            evals.aimer.ToggleAimerOn();
        }
    }
    private void Update()
    {
        if (!mouseAim)
        {
            //rotation is edited in objectplacer.
            if (rotBounds != 0)
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
        else
        {
            //now point to the mouse
            Vector3 mP = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 aimDir = (mP - transform.position)/*.normalized*/;
            
            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            print(angle);
            angle += rotOff;
            print(angle);
            rotateable.rotation = Quaternion.Euler(0, 0, angle);
            if(rotBounds != 0)
            {
                if (rotateable.localEulerAngles.z > -180 && rotateable.localEulerAngles.z < 180)
                {
                    sp.color = new Color(0, 1, 0, 0.25f);
                }
                else
                {
                    sp.color = new Color(1, 0, 0, 0.25f);
                }
            }
            //rotateable.rotation = Quaternion.Euler(0, 0, rotateable.rotation.eulerAngles.z);
            //Debug.Log(rotateable.rotation.eulerAngles);
            //now cancel on click
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                mouseAim = false;
                GameObject.FindObjectOfType<ToolTipDisplay>().UnSetToolTip();
                sp.color = new Color(0, 0, 0, 0);
                if (evals.aimer)
                {
                    evals.aimer.ToggleAimerOff();
                }
            }
        }
    }
}
