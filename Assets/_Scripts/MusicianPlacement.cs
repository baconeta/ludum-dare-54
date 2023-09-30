using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicianPlacement : MonoBehaviour
{
    [SerializeField] private GameObject occupyingMusician;
    public static event Action OnMusicianPlaced; 

    public bool SetMusician(GameObject musician)
    {
        if (occupyingMusician) return false;

        occupyingMusician = musician;
        occupyingMusician.transform.SetPositionAndRotation(transform.position, transform.rotation);
        occupyingMusician.transform.parent = transform;
        OnMusicianPlaced?.Invoke();
        return true;
    }

    public bool IsOccupied()
    {
        return occupyingMusician != null;
    }

}
