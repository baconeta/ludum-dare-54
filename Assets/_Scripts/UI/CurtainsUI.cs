using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class CurtainsUI : Singleton<CurtainsUI>
{
    public static event Action OnCurtainsClosed;
    public static event Action OnCurtainsOpened;
    public Animator animator;

    void OnEnable()
    {
        
    }
    public void CurtainsOpened()
    {
        OnCurtainsOpened?.Invoke();
    }
    public void CurtainsClosed()
    {
        OnCurtainsClosed?.Invoke();
    }

    public void OpenCurtains()
    {
        animator.SetTrigger("Open");
    }

    public void CloseCurtains()
    {
        animator.SetTrigger("Close");
    }
}
