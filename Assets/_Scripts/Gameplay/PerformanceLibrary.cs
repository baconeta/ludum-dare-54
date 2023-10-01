using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Gameplay;
using UnityEngine;

public class PerformanceLibrary : MonoBehaviour
{
    public PerformanceDataSO[] performances;
    public static PerformanceDataSO[] performancesStatic;

    private void Start()
    {
        performancesStatic = performances;
    }
}
