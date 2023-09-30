using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class MusicianDragger : MonoBehaviour
{
    
    public RectTransform musicianCardSelected;
    private GameObject musicianSelectedGameObject;
    public GameObject musicianPrefab;

    [Header("Character Movement")]
    private bool moveAnimationActive;
    public float grabMoveDuration;
    public AnimationCurve grabMoveCurve;
    public float placementDropRange;

    void OnEnable()
    {
        InputManager.OnPointerPrimary += DropMusician;
    }

    void OnDisable()
    {
        InputManager.OnPointerPrimary -= DropMusician;
    }

    // Update is called once per frame
    void Update()
    {
        if (musicianSelectedGameObject)
        {
            if (!moveAnimationActive)
            {
                musicianSelectedGameObject.transform.position = InputManager.PointerPositionWorldSpace;
            }
        }
    }

    void DropMusician(bool pointerActivity)
    {
        //Only if the pointer action is false
        if (!pointerActivity)
        {
            if (!musicianSelectedGameObject) return;
            bool success = false;
            //Check if the musician is on a spot
            RaycastHit2D[] rayHits = Physics2D.CircleCastAll((Vector2)InputManager.PointerPositionWorldSpace, placementDropRange, Vector2.zero);
            foreach (var rayHit in rayHits)
            {
                if (rayHit.transform.gameObject.CompareTag("MusicianPlacement"))
                {
                   //Success condition, only if the placement is unoccupied.
                   if (rayHit.transform.GetComponent<MusicianPlacement>().SetMusician(musicianSelectedGameObject))
                   {
                       success = true;
                   };
                   break;
                }
            }
            //Fail Condition, remove object
            if (!success)
            {
                Destroy(musicianSelectedGameObject);
                //TODO Play effect on card??
            }
            //Deselect the Musician
            musicianSelectedGameObject = null;
            musicianCardSelected = null;
        }
    }

    public void PickupMusician(RectTransform musicianToPickup)
    {
        musicianCardSelected = musicianToPickup;
        musicianSelectedGameObject = Instantiate(musicianPrefab,
            InputManager.PointerPositionWorldSpace, Quaternion.identity);
        musicianSelectedGameObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        StartCoroutine(MoveMusicianToPointer());
    }

    IEnumerator MoveMusicianToPointer()
    {
        moveAnimationActive = true;
        float t = 0;
        Vector3 startPos = musicianSelectedGameObject.transform.position;
        Vector3 cursorPos;
        while (t < grabMoveDuration)
        {
            cursorPos = InputManager.PointerPositionWorldSpace;
            musicianSelectedGameObject.transform.position =
                Vector3.Lerp(startPos, cursorPos, grabMoveCurve.Evaluate(t / grabMoveDuration));
            t += Time.deltaTime;
            yield return null;
        }
        musicianSelectedGameObject.transform.position = InputManager.PointerPositionWorldSpace;
        moveAnimationActive = false;
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(InputManager.PointerPositionWorldSpace, placementDropRange);
    }
}
