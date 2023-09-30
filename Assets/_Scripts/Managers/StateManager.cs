using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        None = 0,
        IntroBrief = 1,
        MusicianSelection = 2,
        Performance = 3,
        Review = 4,
    }

    [Header("All objects to activate/deactivate when the game state changes.")]
    [Header("Add to multiple lists to keep them active between modes.")]
    [SerializeField] private GameObject[] IntroBriefStateObjects;
    [SerializeField] private GameObject[] MusicianSelectionStateObjects;
    [SerializeField] private GameObject[] PerformanceStateObjects;
    [SerializeField] private GameObject[] ReviewStateObjects;

    private GameState currentState;
    public GameState GetCurrentState()
    {
        return currentState;
    }
    public void SetCurrentState(GameState newState)
    {
        int s = (int) (currentState = newState);
        foreach (var obj in IntroBriefStateObjects)
        {
            obj?.SetActive(s == 1);
        }
        foreach (var obj in MusicianSelectionStateObjects)
        {
            obj?.SetActive(s == 2);
        }
        foreach (var obj in PerformanceStateObjects)
        {
            obj?.SetActive(s == 3);
        }
        foreach (var obj in ReviewStateObjects)
        {
            obj?.SetActive(s == 4);
        }
    }

    public void AddObjectToCurrentState(GameObject obj)
    {
        switch (currentState) {
            case GameState.IntroBrief:
                IntroBriefStateObjects = IntroBriefStateObjects.Concat(new GameObject[] { obj }).ToArray();
                break;
            case GameState.MusicianSelection:
                MusicianSelectionStateObjects = MusicianSelectionStateObjects.Concat(new GameObject[] { obj }).ToArray();
                break;
            case GameState.Performance:
                PerformanceStateObjects = PerformanceStateObjects.Concat(new GameObject[] { obj }).ToArray();
                break;
            case GameState.Review:
                ReviewStateObjects = ReviewStateObjects.Concat(new GameObject[] { obj }).ToArray();
                break;
        }
    }

    // When the scene is loaded etc.
    public void OnEnable()
    {
        currentState = GameState.IntroBrief;
    }

    // When the scene is unloaded etc.
    public void OnDisable()
    {
        
    }

    public void Update()
    {
        
    }
}
