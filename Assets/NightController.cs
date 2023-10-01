using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

public class NightController : MonoBehaviour
{
    public NightSelection nightSelectionUI;
    [SerializeField] List<PerformanceDataSO> nights;
    public int numOfEasyPerformances;
    public int numOfMediumPerformances;
    public int numOfHardPerformances;

    public int currentNight = 0;

    public static event Action<PerformanceDataSO> OnNightStarted;
    
    #region Performance Data
    [Header("Available Performances")]
    public PerformanceDataSO[] easyPerformances;
    public PerformanceDataSO[] mediumPerformances;
    public PerformanceDataSO[] hardPerformances;

    public PerformanceDataSO GetEasyPerformance()
    {
        return easyPerformances[Random.Range(0, easyPerformances.Length)];
    }

    public PerformanceDataSO GetMediumPerformance()
    {
        return mediumPerformances[Random.Range(0, mediumPerformances.Length)];
    }

    public PerformanceDataSO GetHardPerformance()
    {
        return hardPerformances[Random.Range(0, hardPerformances.Length)];
    }

    /// <summary>
    /// Get a set of performances for the game
    /// </summary>
    /// <returns>PerformanceData tuple of Easy/Medium/Hard Performances</returns>
    public List<PerformanceDataSO> GetPerformancesForGame(int easy, int medium, int hard)
    {
        List<PerformanceDataSO> performanceNights = new List<PerformanceDataSO>();
        for (int i = 0; i < easy; i++)
        {
            performanceNights.Add(GetEasyPerformance());
        }

        for (int i = 0; i < medium; i++)
        {
            performanceNights.Add(GetMediumPerformance());
        }

        for (int i = 0; i < hard; i++)
        {
            performanceNights.Add(GetHardPerformance());
        }
        return (performanceNights);
    }
    #endregion

    void OnEnable()
    {
        NightSelection.OnPerformanceSelected += StartNight;
    }
    void OnDisable()
    {
        NightSelection.OnPerformanceSelected -= StartNight;
    }

    void Start()
    {
        //TODO Start() Replace with scene loaded/first entry/check playerPrefs (If we add saving)
        //Generate Nights once at the very start of the game.
        GenerateNights();
    }
    
    /// <summary>
    /// Generates a new set of nights and starts Night Selection.
    /// Only run once at the very start of the game.
    ///
    /// Could be added to include PlayerPrefs saving/loading of progression
    /// </summary>
    public void GenerateNights()
    {
        PlayerPrefs.SetInt("NightsComplete", 0);
        nights = GetPerformancesForGame(numOfEasyPerformances, numOfMediumPerformances, numOfHardPerformances);
        nightSelectionUI.ShowNightSelection(nights);
    }

    public List<PerformanceDataSO> GetNights()
    {
        return nights;
    }

    void StartNight(PerformanceDataSO performanceDataSo)
    {
        currentNight++;
        OnNightStarted?.Invoke(performanceDataSo);
    }

    public void EndNight()
    {
        PlayerPrefs.SetInt("NightsComplete", currentNight);
        nightSelectionUI.ShowNightSelection(nights);
    }
}
