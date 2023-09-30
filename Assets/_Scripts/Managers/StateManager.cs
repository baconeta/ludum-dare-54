using System;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public enum GameState
    {
        None = 0,
        IntroBrief = 1,
        MusicianSelection = 2,
        Performance = 3,
        Review = 4,
    }

    [Header("All objects to activate/deactivate when the game state changes. Add to multiple lists to keep them active between modes.")]
    [SerializeField] public GameObject[] stageOneObjects;
    [SerializeField] public GameObject[] stageTwoObjects;
    [SerializeField] public GameObject[] stageThreeObjects;
    [SerializeField] public GameObject[] stageFourObjects;

    private GameState currentState;
    public GameState GetCurrentState()
    {
        return currentState;
    }
    public void SetCurrentState(GameState newState)
    {
        int s = (int) (currentState = newState);
        foreach (var obj in stageOneObjects)
        {
            obj.SetActive(s == 1);
        }
        foreach (var obj in stageTwoObjects)
        {
            obj.SetActive(s == 2);
        }
        foreach (var obj in stageThreeObjects)
        {
            obj.SetActive(s == 3);
        }
        foreach (var obj in stageFourObjects)
        {
            obj.SetActive(s == 4);
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
