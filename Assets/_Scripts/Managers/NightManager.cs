using System;
using System.Collections.Generic;
using _Scripts.Gameplay;
using Audio;
using Managers;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class NightManager : MonoBehaviour
{
    public NightSelection nightSelectionUI;
    [SerializeField] List<PerformanceDataSO> nights;
    public int numOfEasyPerformances;
    public int numOfMediumPerformances;
    public int numOfHardPerformances;

    public int currentNight = 0;

    public static event Action<PerformanceDataSO> OnPerformanceSelected;
    public static event Action OnAllNightsEnded;

    private PhaseManager phaseManager;

    public AudioWrapper audioWrapper;
    private CustomAudioSource musicSource;

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
            //Search through all performances, match their serial code, then load it.
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

    public void OnEnable()
    {
        PhaseManager.OnGamePhaseChange += StartNightSystem;
        NightSelection.OnPerformanceSelected += StartNight;

        // Get a reference to the phase manager.
        phaseManager = FindObjectOfType<PhaseManager>();
        if (phaseManager == null)
        {
            Debug.LogError("PerformanceManager.cs couldn't get PhaseManager!");
        }
    }
    public void OnDisable()
    {
        NightSelection.OnPerformanceSelected -= StartNight;
        PhaseManager.OnGamePhaseChange -= StartNightSystem;
    }

    public void StartNightSystem(PhaseManager.GamePhase phase)
    {
        if (phase != PhaseManager.GamePhase.NightSelection) return;
        
        musicSource = audioWrapper.PlaySound("NightMusic");
        // Generate Nights once at the very start of the game, and then saves progress.
        GenerateNights();
    }

    public void NextWeek()
    {
        PlayerPrefs.SetInt("NightsComplete", 0);
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
        // Has saved data
        currentNight = PlayerPrefs.GetInt("NightsComplete");
        if (currentNight > 0)
        {
            nights = GetExistingPerformancesForGame();
        }
        else // No saved data
        {
            nights = GeneratePerformancesForGame(numOfEasyPerformances, numOfMediumPerformances, numOfHardPerformances);
        }
        
        PlayerPrefs.SetInt("TotalNights", nights.Count);
        
        nightSelectionUI.ShowNightSelection(nights);
    }

    public List<PerformanceDataSO> GetNights()
    {
        return nights;
    }

    public void StartNight(PerformanceDataSO performanceDataSo)
    {
        // TODO Can currently just repeat night 1 and progress
        currentNight++;
        if (currentNight > nights.Count) currentNight = nights.Count;
        OnPerformanceSelected?.Invoke(performanceDataSo);
        musicSource.StopAudio();
        phaseManager.SetCurrentPhase(PhaseManager.GamePhase.MusicianSelection);
    }

    public void EndNight()
    {
        PlayerPrefs.SetInt("NightsComplete", currentNight);
        if (currentNight >= nights.Count)
        {
            // All nights finished!
            nightSelectionUI.ShowNextWeek();
            OnAllNightsEnded?.Invoke();
        }

        
        nightSelectionUI.ShowNightSelection(nights);

        // End the review phase and return to night selection.
        phaseManager.SetCurrentPhase(PhaseManager.GamePhase.NightSelection);
    }
}
