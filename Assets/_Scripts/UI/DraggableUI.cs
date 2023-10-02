using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(EventTrigger))]
public class DraggableUI : MonoBehaviour
{
    [Header("Drag")]
    public bool isHeld;
    [SerializeField] private GameObject dragObject;
    private bool moveAnimationActive;
    public float onDragMoveDuration;
    public AnimationCurve onDragMoveCurve;
    [Header("Drop")]
    public float dropRange;
    private bool dropAnimationActive;
    public float dropMoveDuration;
    public AnimationCurve dropMoveXCurve;
    public AnimationCurve dropMoveYCurve;
    
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
        var pressUI = GetComponent<PopupManager.PressListenerForPopup>();
        pressUI?.SetEnabled(false);
        
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

            Vector3 tempScale = dragObject.transform.localScale;
            
            //Check if valid drop location
            RaycastHit2D[] rayHits = Physics2D.CircleCastAll((Vector2)InputManager.PointerPositionWorldSpace, dropRange, Vector2.zero);
            foreach (var rayHit in rayHits)
            {
                if (rayHit.transform.gameObject.CompareTag("StagePlacement"))
                {
                    
                    //Success condition, only if the placement is unoccupied.
                    if (rayHit.transform.GetComponent<StagePlacement>().SetObject(dragObject))
                    {
                        StartCoroutine(Drop(rayHit.transform));
                        success = true;
                    }
                    break;
                }
            }
            //Fail Condition, remove object
            if (!success)
            {
                StopAllCoroutines();
                dragObject.SetActive(false);
                dragObject.transform.parent = transform;
                dragObject.transform.localScale = tempScale;
                dragObject.transform.localPosition = Vector3.zero;

                StartCoroutine(ResetPopupPressState());
                //TODO Play effect on card??
            }

        }
    }

    private IEnumerator Drop(Transform hitTarget)
    {
        dropAnimationActive = true;
        float t = 0;
        Vector3 startPos = dragObject.transform.position;
        Vector3 endPos = hitTarget.transform.position;
        while (t < dropMoveDuration)
        {
            float nextX = Mathf.Lerp(startPos.x, endPos.x, dropMoveXCurve.Evaluate(t / dropMoveDuration));
            float nextY = Mathf.Lerp(startPos.y, endPos.y, dropMoveYCurve.Evaluate(t / dropMoveDuration));
            dragObject.transform.position = new Vector3(nextX, nextY, dragObject.transform.position.z);
            t += Time.deltaTime;
            yield return null;
        }

        //dragObject.transform.position = endPos;
        dropAnimationActive = false;
        //Turn off card object.
        gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator ResetPopupPressState()
    {
        yield return new WaitForSeconds(0.2f);
        
        var pressUI = GetComponent<PopupManager.PressListenerForPopup>();
        pressUI?.SetEnabled(true);
    }
}
