using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitThenStartAudio : MonoBehaviour
{
    public float durationToWait;

    void Start()
    {
        GetComponent<AudioSource>().PlayDelayed(durationToWait);
        
    }

}
