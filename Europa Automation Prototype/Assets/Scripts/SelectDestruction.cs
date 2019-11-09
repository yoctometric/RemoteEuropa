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
    [SerializeField] ParticleSystem deathEffect;

    Vector2 boxSize;
    void Start()
    {
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
        if (Input.GetMouseButtonDown(0))
        {
            DestroyOverlaps(transform.position, size);
        }//if the right click, cancel
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);

        }
    }

    //function to get all objects in overlap
    void DestroyOverlaps(Vector3 mid, Vector2 size)
    {
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(mid, size, 0, itemLayerMask,Mathf.NegativeInfinity, Mathf.Infinity);
        foreach (Collider2D obj in overlaps)
        {
            if (obj.gameObject.tag == "Item" || obj.gameObject.tag == "Egg")
            {

                Instantiate(deathEffect, obj.transform.position, Quaternion.identity);
                Destroy(obj.gameObject);
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
