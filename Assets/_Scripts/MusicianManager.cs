using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicianManager : MonoBehaviour
{
    private MusicianPlacement[] musicianPlacementPoints;

    [SerializeField] private bool isFull = false;

    public event Action OnStageFull; 

    // Start is called before the first frame update
    void OnEnable()
    {
        MusicianPlacement.OnMusicianPlaced += CheckIsFull;
    }

    private void OnDisable()
    {
        MusicianPlacement.OnMusicianPlaced -= CheckIsFull;

    }

    private void Awake()
    {
        musicianPlacementPoints = GetComponentsInChildren<MusicianPlacement>();
    }

    void CheckIsFull()
    {
        isFull = true;
        foreach (var placementPoint in musicianPlacementPoints)
        {
            //If an empty spot, then its not full.
            if (!placementPoint.IsOccupied())
            {
                isFull = false;
            }
        }

        if (isFull)
        {
            OnStageFull?.Invoke();
        }
    }
}
