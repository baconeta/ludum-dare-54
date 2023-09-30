using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Tracks the current state of the game, and manages switching between states.
 * 
 * Stores references to GameObjects that are related to a particular game state, and shows/hides them based on the current game state.
 * 
 * Dynamically-created objects should be added to this tracker with AddObjectToCurrentState when those objects are created.
 */
public class StateManager : MonoBehaviour
{
    [System.Serializable]
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
    [SerializeField] private GameState testState;

    private GameState currentState;
    
    public GameState GetCurrentState()
    {
        return currentState;
    }
    [ContextMenu("SetCurrentState to testState")]
    public void SetCurrentState()
    {
        SetCurrentState(testState);
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
        // StateManager.cs
        currentState = GameState.IntroBrief;
    }
}
