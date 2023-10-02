using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Gameplay;
using Audio;
using UI.Popups;
using UnityEngine;
using Utils;

public class StageManager : Singleton<StageManager>
{
    [Header("Musicians")] public GameObject MusicianUIPrefab;
    public Transform musicianBarUI;
    public List<Musician> musiciansInRound = new();
    public static event Action<List<Musician>> OnMusiciansGenerated;

    [Header("Instruments")] public GameObject instrumentUIPrefab;
    public Transform instrumentsBarUI;
    public List<Instrument> instrumentsInRound = new();
    public static event Action<List<Instrument>> OnInstrumentsGenerated;

    [Header("Stage")] 
    [SerializeField] private StagePlacement[] allPlacementPoints;
    [SerializeField] private StagePlacement[] stagePlacementPoints2;
    [SerializeField] private StagePlacement[] stagePlacementPoints3;
    [SerializeField] private StagePlacement[] stagePlacementPoints4;
    [SerializeField] private StagePlacement[] stagePlacementPoints5;
    [SerializeField] private StagePlacement[] stagePlacementPoints6;
    [SerializeField] private bool isStageFull = false;
    public static event Action OnStageFull;
    [SerializeField] private GameObject openShowButton;
    [SerializeField] private GameObject musicScoreButton;

    [Header("Temp Generation Parameters")] 
    public int placementPointsThisRound;
    public int additionalMusiciansThisRound;

    private void OnEnable()
    {
        StagePlacement.OnInstrumentPlaced += CheckIsFull;
        StagePlacement.OnMusicianPlaced += CheckIsFull;
    }

    private void OnDisable()
    {
        StagePlacement.OnInstrumentPlaced -= CheckIsFull;
        StagePlacement.OnMusicianPlaced -= CheckIsFull;
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

        foreach (var placement in allPlacementPoints)
        {
            placement.Clear();
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
        if (numOfPlacementPoints > 6)
        {
            numOfPlacementPoints = 6;
            Debug.LogWarning(
                "Trying to generate more Placement Points than existing game objects! Either add more Placement Points, or ask for less.");
        }
                
        //Turn off all placement points
        foreach (var pp in allPlacementPoints)
        {
            pp.gameObject.SetActive(false);
        }
        
        // Enable only those we are using
        switch (numOfPlacementPoints)
        {
            case 2:
                foreach (var placement in stagePlacementPoints2)
                {
                    placement.gameObject.SetActive(true);
                }
                break;
            case 3:
            foreach (var placement in stagePlacementPoints3)
            {
                placement.gameObject.SetActive(true);
            }
            break;
            case 4:
                foreach (var placement in stagePlacementPoints4)
                {
                    placement.gameObject.SetActive(true);
                }
                break;
            case 5:
                foreach (var placement in stagePlacementPoints5)
                {
                    placement.gameObject.SetActive(true);
                }
                break;
            case 6:
                foreach (var placement in stagePlacementPoints6)
                {
                    placement.gameObject.SetActive(true);
                }
                break;
        }

        if (isTest)
        {
            GenerateTestMusicians(numOfPlacementPoints);
            GenerateTestInstruments(numOfPlacementPoints);
        }

        StartCoroutine(ActivateMusicScoreButton());
    }

    private IEnumerator ActivateMusicScoreButton()
    {
        yield return new WaitForSeconds(2);
        
        musicScoreButton.SetActive(true);
        PopupManager popupManager = FindObjectOfType<PopupManager>();
        popupManager.AddPressPopup(new PopupManager.PopupPair(popupManager.performanceInfoPopup, musicScoreButton.gameObject));
        musicScoreButton.GetComponent<PopupManager.PressListenerForPopup>().SetCallBack(() =>
        {
            PerformanceInfoPopup mPopup = FindObjectOfType<PerformanceInfoPopup>();
            mPopup?.HideStartButton();
        });
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
        foreach (var placementPoint in allPlacementPoints)
        {
            //If an empty spot (and turned on!), then its not full.
            (bool, bool) pointStatus = placementPoint.IsOccupied();
            if ((!pointStatus.Item1 || !pointStatus.Item2) && placementPoint.gameObject.activeSelf)
            {
                isStageFull = false;
            }
        }

        if (isStageFull)
        {
            OnStageFull?.Invoke();
            openShowButton.SetActive(true);
            musicianBarUI.gameObject.SetActive(false);
            instrumentsBarUI.gameObject.SetActive(false);
            musicScoreButton.SetActive(false);

        }
        else if(openShowButton.activeSelf) openShowButton.SetActive(false);
    }

}