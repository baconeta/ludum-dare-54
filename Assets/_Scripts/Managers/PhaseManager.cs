using System;
using System.Linq;
using UnityEngine;

/**
 * Tracks the current phase of the game, and manages switching between phases.
 * 
 * Stores references to GameObjects that are related to a particular game phase, and shows/hides them based on the current game phase.
 * 
 * Dynamically-created objects should be added to this tracker with AddObjectToCurrentPhase when those objects are created.
 */
public class PhaseManager : MonoBehaviour
{
    [System.Serializable]
    public enum GamePhase
    {
        None = 0,
        NightSelection = 1,
        MusicianSelection = 2,
        Performance = 3,
        Review = 4,
    }

    [Header("All objects to activate/deactivate when the game phase changes.")]
    [Header("Add to multiple lists to keep them active between phases.")]
    [SerializeField] private GameObject[] NightSelectionPhaseObjects;
    [SerializeField] private GameObject[] MusicianSelectionPhaseObjects;
    [SerializeField] private GameObject[] PerformancePhaseObjects;
    [SerializeField] private GameObject[] ReviewPhaseObjects;
    [SerializeField] private GamePhase testPhase;

    public static event Action<GamePhase> OnGamePhaseChange;

    private GamePhase currentPhase = GamePhase.None;
    
    public GamePhase GetCurrentPhase()
    {
        return currentPhase;
    }

    public void SetCurrentPhase(GamePhase newPhase)
    {
        int p = (int)(currentPhase = newPhase);
        // Enabled/disable game objects to match the new phase.
        foreach (var obj in NightSelectionPhaseObjects)
        {
            obj?.SetActive(p == 1);
        }
        foreach (var obj in MusicianSelectionPhaseObjects)
        {
            obj?.SetActive(p == 2);
        }
        foreach (var obj in PerformancePhaseObjects)
        {
            obj?.SetActive(p == 3);
        }
        foreach (var obj in ReviewPhaseObjects)
        {
            obj?.SetActive(p == 4);
        }

        // Handle some tasks based on the new phase.
        switch (newPhase)
        {
            case GamePhase.NightSelection:
                break;
            case GamePhase.MusicianSelection:
                break;
            case GamePhase.Performance:
                Debug.LogWarning("The Show is Starting!");
                break;
            case GamePhase.Review:
                Debug.LogWarning("The Show has Ended!");
                //Add a lil clap sound :)
                break;
        }

        // Let other classes handle the new phase.
        OnGamePhaseChange?.Invoke(newPhase);
    }

    [ContextMenu("SetCurrentPhase to testPhase")]
    public void SetTestPhase()
    {
        SetCurrentPhase(testPhase);
    }

    public void AddObjectToCurrentPhase(GameObject obj)
    {
        switch (currentPhase) {
            case GamePhase.NightSelection:
                NightSelectionPhaseObjects = NightSelectionPhaseObjects.Concat(new GameObject[] { obj }).ToArray();
                break;
            case GamePhase.MusicianSelection:
                MusicianSelectionPhaseObjects = MusicianSelectionPhaseObjects.Concat(new GameObject[] { obj }).ToArray();
                break;
            case GamePhase.Performance:
                PerformancePhaseObjects = PerformancePhaseObjects.Concat(new GameObject[] { obj }).ToArray();
                break;
            case GamePhase.Review:
                ReviewPhaseObjects = ReviewPhaseObjects.Concat(new GameObject[] { obj }).ToArray();
                break;
        }
    }

    // When the scene is loaded etc.
    public void OnEnable()
    {
        // PhaseManager.cs
        SetCurrentPhase(GamePhase.NightSelection);
    }

    // When the scene is unloaded etc.
    public void OnDisable()
    {
        SetCurrentPhase(GamePhase.None);
    }
}
