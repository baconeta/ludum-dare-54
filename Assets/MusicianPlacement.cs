using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicianPlacement : MonoBehaviour
{
    [SerializeField] private GameObject occupyingMusician;
    
    public bool SetMusician(GameObject musician)
    {
        if (occupyingMusician) return false;

        occupyingMusician = musician;
        occupyingMusician.transform.SetPositionAndRotation(transform.position, transform.rotation);
        return true;
    }

}
