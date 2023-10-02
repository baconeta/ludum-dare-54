using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public UnityEvent OnAnimationEnd;
    // Start is called before the first frame update
    public void AnimationEnded()
    {
        OnAnimationEnd?.Invoke();
    }

}
