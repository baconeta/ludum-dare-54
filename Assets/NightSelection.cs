using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Gameplay;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class NightSelection : MonoBehaviour
{

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
    public List<PerformanceDataSO> GetPerformancesForGame(int easy, int medium, int hard)
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
        return (performanceNights);
    }
    #endregion

    #region Night Selection

    [Header("Night UI")] 
    public Animator animator;
    public string night1Text;
    public string nightMiddleText;
    public string lastNightText;
    public Transform nightHolder;
    public GameObject nightUIPrefab;
    public static event Action<PerformanceDataSO> OnPerformanceSelected;

    private PerformanceDataSO selectedNight;

    void Start()
    {
        GenerateNights();
        ShowNightSelection();
    }
    public void GenerateNights()
    {
        List<PerformanceDataSO> nights = GetPerformancesForGame(1, 1, 1);

        for (int i = 0; i < nights.Count; i++)
        {
            NightUI night = Instantiate(nightUIPrefab, nightHolder).GetComponent<NightUI>();
            //Set the night title text
            if (i == 0) night.nightText.text = night1Text;
            else if (i == nights.Count - 1) night.nightText.text = lastNightText;
            else night.nightText.text = $"{nightMiddleText} {i}";
            //Set the quest title text
            night.questText.text = nights[i].trackData.questName;
            int nightNum = i;
            //Save the performance data
            night.performance = nights[i];
            //Add button to select performance
            night.button.onClick.AddListener(() => SelectNight(night.performance));
        }
    }

    public void SelectNight(PerformanceDataSO nightToSelect)
    {
        selectedNight = nightToSelect;
        //Hide Night UI
        HideNightSelection();
    }

    public void ShowNightSelection()
    {
        selectedNight = null;
        gameObject.SetActive(true);
        animator.SetTrigger("Close");
    }

    void HideNightSelection()
    {
        animator.SetTrigger("Open");
    }

    void HideComplete()
    {
        gameObject.SetActive(false);
        OnPerformanceSelected?.Invoke(selectedNight);
    }
    #endregion 
}
