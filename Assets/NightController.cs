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

    public static event Action<PerformanceDataSO> OnPerformanceSelected;
    public static event Action OnNightStarted;
    public static event Action OnNightEnded;
    
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
    public List<PerformanceDataSO> GeneratePerformancesForGame(int easy, int medium, int hard)
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

        for(int i = 0; i < performanceNights.Count; i++)
        {
            PlayerPrefs.SetInt($"Night{i}", performanceNights[i].performanceKey);
        }
        return (performanceNights);
    }

    public List<PerformanceDataSO> GetExistingPerformancesForGame()
    {
        List<PerformanceDataSO> loadedPerformances = new List<PerformanceDataSO>();
        int totalNights = PlayerPrefs.GetInt("TotalNights");
        for (int i = 0; i < totalNights; i++)
        {
            int serial = PlayerPrefs.GetInt($"Night{i}");
            Debug.Log($"Found Existing Night: {serial}");
            //TODO Search through all performances, match their serial code, then load it.
            foreach (PerformanceDataSO performanceDataSo in PerformanceLibrary.performancesStatic)
            {
                if (performanceDataSo.performanceKey == serial)
                {
                    loadedPerformances.Add(performanceDataSo);
                    break;
                }
            }
        }
        
        //If empty, generate new performances
        if (loadedPerformances.Count <= 0)
            loadedPerformances = GeneratePerformancesForGame(numOfEasyPerformances, numOfMediumPerformances, numOfHardPerformances);
        return loadedPerformances;
    }
    #endregion

    void OnEnable()
    {
        PhaseManager.OnGamePhaseChange += StartNightSystem;
        NightSelection.OnPerformanceSelected += StartNight;
    }
    void OnDisable()
    {
        NightSelection.OnPerformanceSelected -= StartNight;
        PhaseManager.OnGamePhaseChange -= StartNightSystem;
    }

    void StartNightSystem(PhaseManager.GamePhase phase)
    {
        if (phase != PhaseManager.GamePhase.NightSelection) return;
        
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
        //Has saved data
        if (PlayerPrefs.GetInt("NightsComplete") > 0)
        {
            nights = GetExistingPerformancesForGame();
        }
        else //No saved data
        {
            PlayerPrefs.SetInt("NightsComplete", 0);
            nights = GeneratePerformancesForGame(numOfEasyPerformances, numOfMediumPerformances, numOfHardPerformances);
        }
        
        PlayerPrefs.SetInt("TotalNights", nights.Count);
        
        nightSelectionUI.ShowNightSelection(nights);
    }

    public List<PerformanceDataSO> GetNights()
    {
        return nights;
    }

    void StartNight(PerformanceDataSO performanceDataSo)
    {
        //TODO Can currently just repeat night 1 and progress
        currentNight++;
        OnPerformanceSelected?.Invoke(performanceDataSo);
        //Go to next phase
        //TODO This should really be an event, not a singleton reference
        OnNightStarted?.Invoke();
    }

    public void EndNight()
    {
        PlayerPrefs.SetInt("NightsComplete", currentNight);
        OnNightEnded?.Invoke();
        nightSelectionUI.ShowNightSelection(nights);
    }
}
