using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicianDragger : MonoBehaviour
{
    public RectTransform musicianCardSelected;
    private GameObject musicianSelectedGameObject;
    public GameObject musicianPrefab;
    private Camera mainCamera;

    public float grabMoveDuration;
    public AnimationCurve grabMoveCurve;
    private bool moveAnimationActive;

    void Start()
    {
        mainCamera = Camera.current;
    }

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
        if (!pointerActivity)
        {
            //Check if the musician is on a spot
            //Deselect the Musician
            musicianSelectedGameObject = null;
        }
    }

    public void PickupMusician(RectTransform musicianToPickup)
    {
        musicianCardSelected = musicianToPickup;
        musicianSelectedGameObject = Instantiate(musicianPrefab,
            InputManager.PointerPositionWorldSpace, Quaternion.identity);
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
}
