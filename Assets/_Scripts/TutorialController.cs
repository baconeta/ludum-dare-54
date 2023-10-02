using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Gameplay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static bool IsTutorial = false;

    public UnityEvent OnPerformanceEnd;
    public UnityEvent OnStageFull;

    private void Awake()
    {
        IsTutorial = true;
    }

    private void OnEnable()
    {
        StageManager.OnStageFull += StageFull;
    }
    
    private void OnDisable()
    {
        StageManager.OnStageFull += StageFull;
    }

    public void PerformanceEnded()
    {
        OnPerformanceEnd?.Invoke();
        PlayerPrefs.SetInt("TutorialComplete", 1);
        IsTutorial = false;
    }

    public void StageFull()
    {
        OnStageFull?.Invoke();
    }



}
