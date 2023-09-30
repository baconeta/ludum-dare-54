using System;
using System.Collections.Generic;
using UnityEngine;
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

    public void ClearAllMusicians()
    {
        musiciansInHand.Clear();
        for (int i = musicianBarUI.childCount - 1; i >= 0; i--)
        {
            Destroy(musicianBarUI.GetChild(i).gameObject);
        }

        isStageFull = false;
    }

    public void GenerateMusicians(int numToGenerate = -1)
    {
        if (numToGenerate == -1)
        {
            numToGenerate = (musicianPlacementPoints.Length * 2);
        }
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

    public void CheckIsFull()
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
