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
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] LayerMask oreMask;
    [SerializeField] LayerMask holeMask;
    [SerializeField] GameObject escMenu;
    [SerializeField] GameObject placeParticle;
    [SerializeField] GameObject settingsMenu;

    public bool canPlace = true;
    bool canEdit = false;
    bool UIOpened = false;
    public bool freezeMovement = false;
    bool overUI = false;
    bool escMenuOpen = false;
    int placeableIndex = 0;
    [HideInInspector] public int previousIndex = 0;
    //cooldown for editing so that the window isnt immideately opened
    int editCoolDown = 4;
    GameObject editingObject;
    Transform rotTarget = null;
    EditRotation eRot = null;
    Camera cam;
    GameObject HUP;
    Core core;
    void Start()
    {
        cam = Camera.main;
        core = GameObject.FindObjectOfType<Core>();
        //can only find objs when they are active, so set it inactive immideately
        HUP = GameObject.FindWithTag("HeadsUpPanel");
        //HUP needs to always be active at the start of the scene
        HUP.SetActive(false);
        Cursor.visible = false;
        SetIndex(1);
        //set sensitivity of scroll
        rotSpeed = PlayerPrefs.GetFloat("ScrollSense");
    }

    void Update()
    {
        //help
        //input

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetIndex(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetIndex(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetIndex(3);
            }
            if (core.level > 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    SetIndex(0);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    SetIndex(6);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    SetIndex(7);
                }
            }
            if (core.level > 1)
            {
                //third tier items
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    SetIndex(9);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    SetIndex(8);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    SetIndex(10);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    SetIndex(11);
                }
            }
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Alpha0))
            {
                SetIndex(5);
            } else if (Input.GetKeyDown(KeyCode.Escape))
            {
                //open pause men if hupcont isnt open
                if (!UIOpened)
                {
                    if (escMenuOpen)
                    {
                        escMenu.SetActive(false);
                        Cursor.visible = false;
                        escMenu.GetComponent<PauseMenu>().FreezeTime(false);
                        escMenuOpen = false;
                        settingsMenu.SetActive(false);
                    }
                    else
                    {
                        escMenu.SetActive(true);
                        escMenu.GetComponent<PauseMenu>().FreezeTime(true);
                        escMenuOpen = true;
                    }
                }
            } else if (Input.GetKeyDown(KeyCode.F))
            {
                //activate mouse aim
                if (eRot)
                {
                    eRot.StartMouseAim();
                }
            }
            

        }
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

        //check if an object is placable or not using an overlap circle
        if(!(placeableIndex == 2) && !(placeableIndex == 10))
        {
            //if u dont have the miner selected do this
            if(placeableIndex != 11)
            {
                canPlace = !Physics2D.OverlapCircle(transform.position, overlapRadius, overlapLayerMask);
            }
            else
            {
                //this means you are placing a core, make canplace radius way larger
                canPlace = !Physics2D.OverlapCircle(transform.position, overlapRadius * 4, overlapLayerMask);
            }
        }
        else if (placeableIndex == 2)
        {
            //if you have a miner, u need an ore. U can no longer stack miners gg ez 
            if(Physics2D.OverlapCircle(transform.position, 1, oreMask))
            {
                //also if u arent over a hardcollider
                if(!Physics2D.OverlapCircle(transform.position, 0.5f, groundLayerMask))
                {
                    canPlace = Physics2D.OverlapCircle(transform.position, 1, oreMask).GetComponent<OreController>();
                }
                else
                {
                    canPlace = false;
                }
            }
            else
            {
                canPlace = false;
            }
        }else if (placeableIndex == 10) //Special circumstances apply for the pump just like the miner
        {
            //With pump one must have water
            if (Physics2D.OverlapCircle(transform.position, 0.25f, holeMask))
            {
                //also if u arent over a hardcollider
                if (!Physics2D.OverlapCircle(transform.position, 0.5f, groundLayerMask))
                {
                    canPlace = true; //hopefully
                }
                else
                {
                    canPlace = false;
                }
            }
            else
            {
                canPlace = false;
            }
        }
        
        if (Physics2D.OverlapCircle(transform.position, 0.5f, editingLayerMask))
        {
            Collider2D[] overlappedEditors = Physics2D.OverlapCircleAll(transform.position, 0.5f, editingLayerMask);
            float closestDistanceSqr = Mathf.Infinity;
            Transform closestTrans = null;
            foreach (Collider2D editor in overlappedEditors)
            {
                Transform edit = editor.transform;
                Vector3 direction = edit.position - transform.position;
                float dSqrToTarget = direction.sqrMagnitude;
                if(dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestTrans = edit;
                }
            }
            //now you have the closest one
            editingObject = closestTrans.gameObject;

            if (editingObject.GetComponent<EditRotation>())
            {
                eRot = editingObject.GetComponent<EditRotation>();
            }

            //now, hupcont logic
            ///we already know we are over a valid col, so
            ///but only open the hupcont if the editing index is active, ie if index is 1
            ///in fact, dont even rotate if index isnt one
            if ((Input.GetMouseButtonDown(0) && editCoolDown < 0 || Input.GetKeyDown(KeyCode.Space) && editCoolDown < 0) && !overUI && placeableIndex == 1)
            {
                UIOpened = true;
                HUP.SetActive(true);

                if (editingObject.GetComponent<EditorValues>().hasPanel)
                {
                    HUP.GetComponent<HUPCONT>().OpenEditor(editingObject);
                }
            }
        }
        else
        {
            //not over any colliders, cant be an erot
            eRot = null;
        }

        //now rotate the rotatable (only if Lshift isnt held because that is for moving the camera)
        ///also only if index is one because thats the editing index
        if (eRot && !Input.GetKey(KeyCode.LeftShift) && placeableIndex == 1)
        {
            eRot.rotateable.Rotate(new Vector3(0, 0, Input.mouseScrollDelta.y * rotSpeed));
            if (Input.GetKey(KeyCode.Q))
            {
                eRot.rotateable.Rotate(new Vector3(0, 0, 0.1f * rotSpeed));
            }
            else if (Input.GetKey(KeyCode.E))
            {
                eRot.rotateable.Rotate(new Vector3(0, 0, -0.1f * rotSpeed));
            }
            //eRot.rotateable.Rotate(new Vector3(0, 0, Input.GetAxis("Horizontal") * -rotSpeed));
        }

        //make the ""cursor"" follow the "cursor" if that makes any sense whatsoever
        if (!UIOpened && !freezeMovement)
        {
            Vector2 mPos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mPos;
            //if not in edit mode and right click, select empty cursor
            if(!canEdit && Input.GetMouseButtonDown(1))
            {
                SetIndex(1);
            }
        }

        //make the placable follow the cursor
        placeables[placeableIndex].transform.position = gameObject.transform.position;
        //rotate cursor with mouse or h axis
        if (!canEdit && !Input.GetKey(KeyCode.LeftShift))
        {
            transform.Rotate(new Vector3(0, 0, Input.mouseScrollDelta.y * rotSpeed));
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(new Vector3(0, 0, 0.1f * rotSpeed));
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(new Vector3(0, 0, -0.1f * rotSpeed));
            }
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

            //destroy parent
        }
        //if the same is true and you left click, select it


        //if you hover over the ui element, set cursor to visible
        
        bool onUI = EventSystem.current.IsPointerOverGameObject();
        if (onUI || freezeMovement)
        {
            Cursor.visible = true;
        }
        else if(!UIOpened)
        {
            Cursor.visible = false;
        }
    }

    public void CloseUI()
    {
        UIOpened = false;
        //Cursor.visible = false;
        HUP.SetActive(false);
    }
    public void ResetPosition()
    {
        freezeMovement = true;
        IEnumerator back()
        {
            yield return new WaitForSecondsRealtime(2f);
            transform.position = Vector2.zero;
            cam.transform.position = new Vector3(0, 0, -10);
            freezeMovement = false;
        }
        StartCoroutine(back());
    }
    void Place(GameObject obj)
    {
        //check if there is a placeable attatched
        if (obj)
        {
            Instantiate(obj, transform.position, placeables[placeableIndex].transform.rotation);//place it
            Instantiate(placeParticle, transform.position, Quaternion.identity).transform.localScale *= 0.15f;
            //activate the objs place function so that it can lose me money

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
            //if the object has a laser sight to do shit with...
            if (editReader.GetComponent<EditorValues>().aimer)
            {
                editReader.GetComponent<EditorValues>().aimer.ToggleAimerOn();
            }
        }
        else
        {
            //takes you back out of edit mode if that was what was planned
            canEdit = false;
            SetIndex(previousIndex);
            //make it un-highlight
            editReader.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0f);
            //if the object has a laser sight to do shit with...
            if (editReader.GetComponent<EditorValues>().aimer)
            {
                editReader.GetComponent<EditorValues>().aimer.ToggleAimerOff();
            }
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
