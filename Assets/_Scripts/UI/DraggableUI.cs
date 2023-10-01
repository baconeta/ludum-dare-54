using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(EventTrigger))]
public class DraggableUI : MonoBehaviour
{
    public bool isHeld;
    [SerializeField] private GameObject dragObject;
    private bool moveAnimationActive;
    public float onDragMoveDuration;
    public AnimationCurve onDragMoveCurve;
    public float dropRange;

    void OnEnable()
    {
        InputManager.OnPointerPrimary += DropAtPointer;
    }

    void OnDisable()
    {
        InputManager.OnPointerPrimary -= DropAtPointer;
    }

    private void Start()
    {
        dragObject.SetActive(false);
    }

    public void OnDrag()
    {
        isHeld = true;
        dragObject.transform.SetParent(null, false);
        dragObject.SetActive(true);
        StartCoroutine(MoveToPointer());
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isHeld)
        {
            if (!moveAnimationActive)
            {
                dragObject.transform.position = InputManager.PointerPositionWorldSpace;
            }
        }
    }
    
    IEnumerator MoveToPointer()
    {
        yield return new WaitForEndOfFrame();
        
        moveAnimationActive = true;
        float t = 0;
        Vector3 startPos = dragObject.transform.position;
        while (t < onDragMoveDuration)
        {
            dragObject.transform.position =
                Vector3.Lerp(startPos, InputManager.PointerPositionWorldSpace, onDragMoveCurve.Evaluate(t / onDragMoveDuration));
            t += Time.deltaTime;
            yield return null;
        }
        
        dragObject.transform.position = InputManager.PointerPositionWorldSpace;
        moveAnimationActive = false;
        yield return null;
    }
    
    void DropAtPointer(bool pointerActivity)
    {
        //Only if the pointer action is false (Pointer/Mouse Up)
        if (!pointerActivity)
        {
            //If not holding, how can drop?
            if (!isHeld) return;
            
            isHeld = false;
            bool success = false;
            
            //Check if valid drop location
            RaycastHit2D[] rayHits = Physics2D.CircleCastAll((Vector2)InputManager.PointerPositionWorldSpace, dropRange, Vector2.zero);
            foreach (var rayHit in rayHits)
            {
                if (rayHit.transform.gameObject.CompareTag("StagePlacement"))
                {
                    //Success condition, only if the placement is unoccupied.
                     if (rayHit.transform.GetComponent<StagePlacement>().SetObject(dragObject))
                     {
                         success = true;
                         gameObject.SetActive(false);
                     };
                    break;
                }
            }
            //Fail Condition, remove object
            if (!success)
            {
                StopAllCoroutines();
                dragObject.SetActive(false);
                dragObject.transform.parent = transform;
                dragObject.transform.localScale = Vector3.one;
                dragObject.transform.localPosition = Vector3.zero;
                //TODO Play effect on card??
            }
        }
    }
}
