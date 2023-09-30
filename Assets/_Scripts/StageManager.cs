using System;
using System.Collections.Generic;
using _Scripts.Gameplay;
using UnityEngine;
using Utils;

public class StageManager : Singleton<StageManager>
{
    [Header("Musicians")] public GameObject MusicianUIPrefab;
    public Transform musicianBarUI;
    public List<Musician> musiciansInRound = new List<Musician>();
    public static event Action<List<Musician>> OnMusiciansGenerated;

    [Header("Instruments")] public GameObject instrumentUIPrefab;
    public Transform instrumentsBarUI;
    public List<Instrument> instrumentsInRound = new List<Instrument>();
    public static event Action<List<Instrument>> OnInstrumentsGenerated;

    [Header("Stage")] [SerializeField] private StagePlacement[] stagePlacementPoints;
    [SerializeField] private bool isStageFull = false;
    public static event Action OnStageFull;

    [Header("Temp Generation Parameters")] public int placementPointsThisRound;
    public int additionalMusiciansThisRound;

    private void OnEnable()
    {
        StagePlacement.OnInstrumentPlaced += CheckIsFull;
    }

    private void OnDisable()
    {
        StagePlacement.OnInstrumentPlaced -= CheckIsFull;
    }

    private void Start()
    {
        //GenerateStage(placementPointsThisRound, true);
    }

    public void ClearStage()
    {
        //Clear Musicians
        musiciansInRound.Clear();
        for (int i = musicianBarUI.childCount - 1; i >= 0; i--)
        {
            Destroy(musicianBarUI.GetChild(i).gameObject);
        }

        //Clear Instruments
        instrumentsInRound.Clear();
        for (int i = instrumentsBarUI.childCount - 1; i >= 0; i--)
        {
            Destroy(instrumentsBarUI.GetChild(i).gameObject);
        }

        //Reset Full Flag
        isStageFull = false;
    }

    /// <summary>
    /// Generates the stage, then generates musicians.
    /// </summary>
    /// <param name="numOfPlacementPoints">The number of positions on the stage. There is *LIMITED SPACE*</param>
    /// <param name="isTest">Will generate fake instruments and musicians only</param>
    public void GenerateStage(int numOfPlacementPoints, bool isTest = false)
    {
        ClearStage();
        if (numOfPlacementPoints > stagePlacementPoints.Length)
        {
            numOfPlacementPoints = stagePlacementPoints.Length;
            Debug.LogWarning(
                "Trying to generate more Placement Points than existing game objects! Either add more Placement Points, or ask for less.");
        }

        //Turn off additional placement points
        for (int i = stagePlacementPoints.Length - 1; i >= numOfPlacementPoints; i--)
        {
            stagePlacementPoints[i].gameObject.SetActive(false);
        }

        if (isTest)
        {
            GenerateTestMusicians(numOfPlacementPoints);
            GenerateTestInstruments(numOfPlacementPoints);
        }
    }

    private void GenerateTestMusicians(int numToGenerate)
    {
        //This should be handled dynamically based on the current track/level design TODO
        //Generate numToGenerate Musicians, these are UI cards.
        for (int i = 0; i < numToGenerate; i++)
        {
            Musician newMusicianUi = Instantiate(MusicianUIPrefab, musicianBarUI).GetComponent<Musician>();
            musiciansInRound.Add(newMusicianUi.GenerateMusician());
        }


        OnMusiciansGenerated?.Invoke(musiciansInRound);
    }

    public void AddMusician(MusicianDataSO data)
    {
        Musician newMusicianUi = Instantiate(MusicianUIPrefab, musicianBarUI).GetComponent<Musician>();
        newMusicianUi.SetMusicianData(data);
        musiciansInRound.Add(newMusicianUi);
    }

    private void GenerateTestInstruments(int numToGenerate)
    {
        //Generate numToGenerate Musicians, these are UI cards.
        for (int i = 0; i < numToGenerate; i++)
        {
            Instrument newInstrumentUi = Instantiate(instrumentUIPrefab, instrumentsBarUI).GetComponent<Instrument>();
            instrumentsInRound.Add(newInstrumentUi.GenerateInstrument());
        }

        OnInstrumentsGenerated?.Invoke(instrumentsInRound);
    }
    
    public void AddInstrument(InstrumentDataSO data)
    {
        Instrument newInstrumentUi = Instantiate(instrumentUIPrefab, instrumentsBarUI).GetComponent<Instrument>();
        newInstrumentUi.SetInstrumentData(data);
        instrumentsInRound.Add(newInstrumentUi);
    }

    public void RemoveMusicianFromHand(Musician musicianToRemove)
    {
        musiciansInRound.Remove(musicianToRemove);
        Destroy(musicianToRemove.gameObject);
    }

    public void RemoveInstrumentFromHand(Instrument instrumentToRemove)
    {
        instrumentsInRound.Remove(instrumentToRemove);
        Destroy(instrumentToRemove.gameObject);
    }

    private void CheckIsFull(StagePlacement placement)
    {
        isStageFull = true;
        foreach (var placementPoint in stagePlacementPoints)
        {
            //If an empty spot (and turned on!), then its not full.
            (bool, bool) pointStatus = placementPoint.IsOccupied();
            if (pointStatus is {Item1: false, Item2: false} && placementPoint.gameObject.activeSelf)
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