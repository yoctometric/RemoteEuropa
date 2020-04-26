using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    LineRenderer lr;

    [SerializeField] Sprite editSprite;
    [SerializeField] List<Vector3> overrideStartPoints;
    private void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();

        Draw(overrideStartPoints);

    }

    public void Draw(List<Vector3> vertices)
    {
        //setup prev point as identical to start
        Vector2 previous = vertices[0];

        //setup lr
        lr.positionCount = vertices.Count;
        lr.SetPositions(vertices.ToArray());

        for(int i = 0; i < vertices.Count; i++)
        {
            MakePoint(vertices[i], previous);

            previous = vertices[i]; // now previous is actually the previous one
        }
    }


    Transform MakePoint(Vector2 pos, Vector2 previousPos)
    {
        GameObject g = new GameObject("point");
        g.transform.position = pos;

        //add collider only if other point was not on top of this one
        float dist = Vector2.Distance(pos, previousPos);
        if (dist != 0)
        {
            CapsuleCollider2D b = g.AddComponent<CapsuleCollider2D>();

            //rotate to old pos
            Vector2 diff = pos - previousPos;
            float rot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            g.transform.rotation = Quaternion.Euler(0, 0, rot + 90);

            //setup collider
            b.size = new Vector2(1, dist + 0.5f);
            b.offset = new Vector2(0, dist / 2);

        }
        g.transform.SetParent(this.transform);
        return g.transform;
    }
}
