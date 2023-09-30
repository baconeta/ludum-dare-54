using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = System.Random;

public class MusicianManager : Singleton<MusicianManager>
{ 
    [Header("UI Elements")]
    public GameObject MusicianUIPrefab;
    public Transform musicianBarUI;
    public List<Musician> musiciansInHand;
    
    [Header("Stage")]
    private MusicianPlacement[] musicianPlacementPoints;
    [SerializeField] private bool isStageFull = false;

    public static event Action OnStageFull;
    public static event Action<List<Musician>> OnMusiciansGenerated;


    // Start is called before the first frame update
    void OnEnable()
    {
        MusicianPlacement.OnMusicianPlaced += CheckIsFull;
    }

    private void OnDisable()
    {
        MusicianPlacement.OnMusicianPlaced -= CheckIsFull;

    }

    private void Awake()
    {
        musicianPlacementPoints = GetComponentsInChildren<MusicianPlacement>();
    }

    void Start()
    {
        //TODO replace with GAME START
        GenerateMusicians(musicianPlacementPoints.Length * 2);
        
    }

    public void ClearAllMusicians()
    {
        musiciansInHand.Clear();
        for (int i = musicianBarUI.childCount - 1; i >= 0; i--)
        {
            Destroy(musicianBarUI.GetChild(i).gameObject);
        }

        isStageFull = false;
    }
    
    public void GenerateMusicians(int numToGenerate)
    {
        ClearAllMusicians();
        //Generate numToGenerate Musicians, these are UI cards.
        for (int i = 0; i < numToGenerate; i++)
        {
            Musician newMusicianUi = Instantiate(MusicianUIPrefab, musicianBarUI).GetComponent<Musician>();
            musiciansInHand.Add(newMusicianUi.GenerateMusician());
        }
        
        OnMusiciansGenerated?.Invoke(musiciansInHand);
    }

    public void RemoveMusicianFromHand(Musician musicianCardToRemove)
    {
        musiciansInHand.Remove(musicianCardToRemove);
        Destroy(musicianCardToRemove.gameObject);
    }

    void CheckIsFull()
    {
        isStageFull = true;
        foreach (var placementPoint in musicianPlacementPoints)
        {
            //If an empty spot, then its not full.
            if (!placementPoint.IsOccupied())
            {
                isStageFull = false;
            }
        }

        if (isStageFull)
        {
            OnStageFull?.Invoke();
        }
    }
}
