using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDestruction : MonoBehaviour
{
    Camera cam;
    Vector3 startP;
    Vector3 endP;

    BoxCollider2D box;
    LineRenderer line;
    [SerializeField] LayerMask itemLayerMask;
    [SerializeField] LayerMask objectLayerMask;
    [SerializeField] ParticleSystem deathEffect;
    [SerializeField] GameObject shiftIndicator;

    Vector2 boxSize;
    bool showObjDestIndicator = false;

    void Start()
    {
        if (GameObject.FindObjectsOfType<SelectDestruction>().Length > 1)
        {
            Destroy(gameObject);
        }
        cam = Camera.main;
        startP = transform.position;
        box = gameObject.GetComponent<BoxCollider2D>();
        line = gameObject.GetComponent<LineRenderer>();
        transform.position = Vector3.zero;
    }
    private void Update()
    {
        //sets the size of the boxcollider
        endP = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mid = (startP + endP) / 2;
        transform.position = new Vector3(mid.x, mid.y, 0);
        Vector2 size = new Vector2(Mathf.Abs(endP.x - startP.x), Mathf.Abs(endP.y - startP.y));
        boxSize = size;
        //handles the linerenderer outlining the deconstruction area
        Vector3[] points = new Vector3[] { startP, (new Vector3(startP.x - (startP.x - endP.x), startP.y, 0)), endP, new Vector3(endP.x - (endP.x - startP.x), endP.y, 0), startP};
        line.SetPositions(points);
        //if the player left clicks, destroy
        //if they hold leftShift, dest objects instead
        if (Input.GetKey(KeyCode.LeftShift))
        {
            showObjDestIndicator = true;
            if (Input.GetMouseButtonDown(0))
            {
                DestroyOverlaps(transform.position, size, true);
            }
        }
        else
        {
            showObjDestIndicator = false;
            if (Input.GetMouseButtonDown(0))
            {
                DestroyOverlaps(transform.position, size, false);
            }
        }
        shiftIndicator.SetActive(showObjDestIndicator);

        //if the right click, cancel
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject, 0.1f);
        }
    }

    //function to get all objects in overlap
    void DestroyOverlaps(Vector3 mid, Vector2 size, bool destObjects)
    {
        Collider2D[] overlaps;

        if (destObjects)
        {
            overlaps = Physics2D.OverlapBoxAll(mid, size, 0, objectLayerMask, Mathf.NegativeInfinity, Mathf.Infinity);
            foreach (Collider2D obj in overlaps)
            {
                if (obj.transform.parent)
                {
                    if ((obj.transform.parent.tag != "Ore") && obj.transform.parent.tag != "Core")
                    {
                        Instantiate(deathEffect, obj.transform.position, Quaternion.identity);
                        Destroy(obj.transform.parent.gameObject); //if not ore and there is a parent, destroy
                    }
                }
                else
                {
                    Instantiate(deathEffect, obj.transform.position, Quaternion.identity);
                    Destroy(obj.gameObject);
                }
            }
        }
        else
        {
            overlaps = Physics2D.OverlapBoxAll(mid, size, 0, itemLayerMask, Mathf.NegativeInfinity, Mathf.Infinity);
            foreach (Collider2D obj in overlaps)
            {
                if (obj.gameObject.tag == "Item" || obj.gameObject.tag == "Egg")
                {

                    Instantiate(deathEffect, obj.transform.position, Quaternion.identity);
                    Destroy(obj.gameObject);
                }
            }
        }

        Destroy(gameObject);

    }
    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //this is copy pasted from the unity documentation
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
