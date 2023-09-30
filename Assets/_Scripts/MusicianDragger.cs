using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class MusicianDragger : MonoBehaviour
{
    
    public Musician musicianCardSelected;
    private GameObject musicianHeld;
    public GameObject musicianPrefab;

    [Header("Character Movement")]
    private bool moveAnimationActive;
    public float grabMoveDuration;
    public AnimationCurve grabMoveCurve;
    public float placementDropRange;

    void OnEnable()
    {
        InputManager.OnPointerPrimary += DropMusician;
        Musician.MusicianDragged += PickupMusician;
    }

    void OnDisable()
    {
        InputManager.OnPointerPrimary -= DropMusician;
        Musician.MusicianDragged -= PickupMusician;

    }

    // Update is called once per frame
    void Update()
    {
        if (musicianHeld)
        {
            if (!moveAnimationActive)
            {
                musicianHeld.transform.position = InputManager.PointerPositionWorldSpace;
            }
        }
    }

    void DropMusician(bool pointerActivity)
    {
        //Only if the pointer action is false
        if (!pointerActivity)
        {
            if (!musicianHeld) return;
            bool success = false;
            //Check if the musician is on a spot
            RaycastHit2D[] rayHits = Physics2D.CircleCastAll((Vector2)InputManager.PointerPositionWorldSpace, placementDropRange, Vector2.zero);
            foreach (var rayHit in rayHits)
            {
                if (rayHit.transform.gameObject.CompareTag("MusicianPlacement"))
                {
                   //Success condition, only if the placement is unoccupied.
                   if (rayHit.transform.GetComponent<MusicianPlacement>().SetMusician(musicianHeld))
                   {
                       success = true;
                       MusicianManager.Instance.RemoveMusicianFromHand(musicianCardSelected);
                   };
                   break;
                }
            }
            //Fail Condition, remove object
            if (!success)
            {
                StopAllCoroutines();
                Destroy(musicianHeld);
                //TODO Play effect on card??
            }
            //Deselect the Musician
            musicianHeld = null;
            musicianCardSelected = null;
        }
    }

    public void PickupMusician(Musician musicianToPickup)
    {
        //Save reference to the CARD that has just been dragged
        musicianCardSelected = musicianToPickup;
        //Generate the In-World Musician and copy the information
        musicianHeld = Instantiate(musicianPrefab,
            InputManager.PointerPositionWorldSpace, Quaternion.identity);
        musicianHeld.GetComponent<Musician>().CopyMusician(musicianCardSelected);
        
        //Temp set colour of sprite for variation
        musicianHeld.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        
        //Animation of snapping to mouse
        StartCoroutine(MoveMusicianToPointer());
    }

    IEnumerator MoveMusicianToPointer()
    {
        moveAnimationActive = true;
        float t = 0;
        Vector3 startPos = musicianHeld.transform.position;
        Vector3 cursorPos;
        while (t < grabMoveDuration)
        {
            cursorPos = InputManager.PointerPositionWorldSpace;
            musicianHeld.transform.position =
                Vector3.Lerp(startPos, cursorPos, grabMoveCurve.Evaluate(t / grabMoveDuration));
            t += Time.deltaTime;
            yield return null;
        }
        musicianHeld.transform.position = InputManager.PointerPositionWorldSpace;
        moveAnimationActive = false;
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(InputManager.PointerPositionWorldSpace, placementDropRange);
    }
}
