﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour
{

    [SerializeField] List<GameObject> placeables;
    [SerializeField] float rotSpeed;
    [SerializeField] float overlapRadius;
    [SerializeField] LayerMask overlapLayerMask;
    [SerializeField] LayerMask editingLayerMask;
    
    public bool canPlace = true;
    bool canEdit = false;
    bool UIOpened = false;
    bool overUI = false;
    int placeableIndex = 0;
    int previousIndex = 0;
    //cooldown for editing so that the window isnt immideately opened
    int editCoolDown = 4;
    GameObject editingObject;
    Transform rotTarget = null;
    EditRotation eRot = null;
    Camera cam;
    GameObject HUP;
    void Start()
    {
        //can only find objs when they are active, so set it inactive immideately
        HUP = GameObject.FindWithTag("HeadsUpPanel");
        HUP.SetActive(false);
        cam = Camera.main;
        Cursor.visible = false;
        SetIndex(0);
    }

    void Update()
    {
        overUI = EventSystem.current.IsPointerOverGameObject();

        //place the placable when able to
        if (((Input.GetKeyDown(KeyCode.Space) && canPlace) || (Input.GetMouseButtonDown(0) && canPlace)) && !overUI)
        {
            if (placeables[placeableIndex].GetComponent<Placeable>())
            {
                Place(placeables[placeableIndex].GetComponent<Placeable>().RealObject);
            }
            editCoolDown = 4;
        }
        //lower cooldown so that editing is eventually re-enabled
        if(editCoolDown > -1)
        {
            editCoolDown -= 1;
        }
        //if ur over an editable object open the ui
        if ((Input.GetMouseButtonDown(0) && editCoolDown < 0 && Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask) || Input.GetKeyDown(KeyCode.Space) && editCoolDown < 0 && Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask)) && !overUI)
        {

            UIOpened = true;
            HUP.SetActive(true);
            if (Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask))
            {
                if(Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask).gameObject.GetComponent<EditorValues>().hasPanel)
                {
                    Cursor.visible = true;

                    HUP.GetComponent<HUPCONT>().OpenEditor(Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask).gameObject);
                }
            }
        }
        //check if an object is placable or not using an overlap circle
        if(!(placeableIndex == 2))
        {
            //if u dont have the miner selected do this
            canPlace = !Physics2D.OverlapCircle(transform.position, overlapRadius, overlapLayerMask);
        }
        else
        {
            //if you have a miner, u need an ore. U can stack miners this way. This is not a bug. Shut up
            if(Physics2D.OverlapCircle(transform.position, 1, overlapLayerMask))
            {
                canPlace = Physics2D.OverlapCircle(transform.position, 1, overlapLayerMask).GetComponent<OreController>();
            }
            else
            {
                canPlace = false;
            }
        }
        if (Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask))
        {
            editingObject = Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask).gameObject;
            if (editingObject.GetComponent<EditRotation>())
            {
                eRot = editingObject.GetComponent<EditRotation>();
            }
        }
        else
        {
            eRot = null;
        }
        //now rotate the rotatable (only if Lshift isnt held because that is for moving the camera)
        if (eRot && !Input.GetKey(KeyCode.LeftShift))
        {
            eRot.rotateable.Rotate(new Vector3(0, 0, Input.mouseScrollDelta.y * rotSpeed));
            //eRot.rotateable.Rotate(new Vector3(0, 0, Input.GetAxis("Horizontal") * -rotSpeed));
        }

        //make the ""cursor"" follow the "cursor" if that makes any sense whatsoever
        if (!UIOpened)
        {
            Vector2 mPos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mPos;
        }

        //make the placable follow the cursor
        placeables[placeableIndex].transform.position = gameObject.transform.position;
        //rotate cursor with mouse or h axis
        if (!canEdit && !Input.GetKey(KeyCode.LeftShift))
        {
            transform.Rotate(new Vector3(0, 0, Input.mouseScrollDelta.y * rotSpeed));
            //transform.Rotate(new Vector3(0, 0, Input.GetAxis("Horizontal") * -rotSpeed));
        }

        //if you are in editing mode however, instead find the rotatable launcher bit and rotate that too


        //if you are editing and over an object and rightclick, destroy it.
        if ((canEdit && Input.GetMouseButtonDown(1)) || (canEdit && Input.GetKeyDown(KeyCode.Backspace)))
        {
            //note requires the mouse collider to be a direct child of the parent. Keep that in mind when creating prefabs.
            //it also requires that the hard collider of the object be the only obj tagged ground. keep that in mind as well
            //make sure it is actually destructible. ALL EDITORS NEED AN EDITORVALUES SCRIPT
            EditorValues eVals = Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask).GetComponent<EditorValues>();
            if (eVals.canBeDestroyed)
            {
                Destroy(Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask).transform.parent.transform.gameObject);
            }
            else
            {
            }

            //destroy parent
        }
        //if the same is true and you left click, select it


    }

    public void CloseUI()
    {
        UIOpened = false;
        Cursor.visible = false;
        HUP.SetActive(false);
    }

    void Place(GameObject obj)
    {
        //check if there is a placeable attatched
        if (obj)
        {
            Instantiate(obj, transform.position, transform.rotation);
        }
    }

    //change from placing mode to editing mode when hovered over an editable object
    //THIS MEANS THAT EVERY EDITABLE OBJ NEEDS AN RB IDIOT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! (I got stuck for like a day because I forgot this jeez)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EditCollider")
        {
            //enter edit mode
            ToggleEditMode(true, other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "EditCollider")
        {
            //exit edit mode
            ToggleEditMode(false, other.gameObject);
        }
    }

    void ToggleEditMode(bool editing, GameObject editReader)
    {
        if(placeableIndex != 1)
        {
            //remembers what you had selected previously
            previousIndex = placeableIndex;
        }
        if (editing)
        {
            //puts you in edit mode if thats what was planned
            canEdit = true;
            SetIndex(1);
            //make it highlight
            editReader.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
        }
        else
        {
            //takes you back out of edit mode if that was what was planned
            canEdit = false;
            SetIndex(previousIndex);
            //make it un-highlight
            editReader.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0f);
        }

    }

    //change the index and set all others unactive
    public void SetIndex(int newIndex)
    {
        placeableIndex = newIndex;
        for(int i = 0; i < placeables.Count; i++)
        {
            if (i != newIndex)
            {
                placeables[i].SetActive(false);
            }
            else
            {
                placeables[i].SetActive(true);
                canPlace = true;
            }
        }
    }
}