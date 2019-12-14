using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    [SerializeField] float speed;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;

    //Welcome to my camera script! I like to call it "Now it's time to completely remove keyboard-only support! YAYYYYY!!!!! (not a real yay)
    //The goal is to create a script which will handle the camera moving. This should also handle zoom levels. Y'know, someday I need to make a tutorial
    float camSize = 10;
    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        camSize = Mathf.Clamp(cam.orthographicSize, 5, 40);

    }
    void FixedUpdate()
    {
        //move cam based on axis. Shouldn't need TOO much more than just that.
        //obv move in fixedupdate
        Vector3 posModifier = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speed;
        transform.position += posModifier;
    }
    private void Update()
    {
        //do the zooming here. Only when leftShift is held so that it does not cause mouse rotation.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            camSize = Mathf.Clamp(cam.orthographicSize + -Input.mouseScrollDelta.y, minZoom, maxZoom);
            cam.orthographicSize = camSize;
        }
    }
}
