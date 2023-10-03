using UnityEngine;

public class WaitThenStartAudio : MonoBehaviour
{
    public float durationToWait;

    private void Start()
    {
        GetComponent<AudioSource>().PlayDelayed(durationToWait);
    }
}
