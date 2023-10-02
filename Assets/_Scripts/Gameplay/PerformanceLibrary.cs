using _Scripts.Gameplay;
using UnityEngine;

public class PerformanceLibrary : MonoBehaviour
{
    [Header("A Library Class for ALL performances.")]
    [Header("Used to search for performanceKeys in saving/loading")]
    public PerformanceDataSO[] performances;
    public static PerformanceDataSO[] performancesStatic;

    private void Awake()
    {
        performancesStatic = performances;
    }
}
