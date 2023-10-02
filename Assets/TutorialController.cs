using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialController : MonoBehaviour
{
    public static bool IsTutorial = true;

    public UnityEvent OnPerformanceEnd;

    public void PerformanceEnded()
    {
        OnPerformanceEnd?.Invoke();
        PlayerPrefs.SetInt("TutorialComplete", 1);
    }


}
