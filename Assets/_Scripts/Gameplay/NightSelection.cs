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

    private PerformanceDataSO selectedNight;
   

    #region Night Selection

    [Header("Night UI")] 
    public string night1Text;
    public string nightMiddleText;
    public string lastNightText;
    public Transform nightHolder;
    public GameObject nightUIPrefab;
    public GameObject nextWeekButton;
    public static event Action<PerformanceDataSO> OnPerformanceSelected;
    
    public void ShowNextWeek()
    {
        nextWeekButton.SetActive(true);
    }

    public void ClearNightsUI()
    {
        selectedNight = null;
        for (int i = nightHolder.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(nightHolder.transform.GetChild(i).gameObject);
        }
    }

    public void GenerateNightsUI(List<PerformanceDataSO> nights)
    {
        for (int i = 0; i < nights.Count; i++)
        {
            NightUI night = Instantiate(nightUIPrefab, nightHolder).GetComponent<NightUI>();
            //Set the night title text
            if (i == 0) night.nightText.text = night1Text;
            else if (i == nights.Count - 1) night.nightText.text = lastNightText;
            else night.nightText.text = $"{nightMiddleText} {i}";
            //Set the quest title text
            night.questText.text = nights[i].trackData.questName;
            //Save the performance data
            night.performance = nights[i];
            //Add button to select performance
            night.button.onClick.AddListener(() => SelectNight(night.performance));

            //If not unlocked, then hide/non-interactable
            if (i > PlayerPrefs.GetInt("NightsComplete"))
            {
                night.button.interactable = false;
            }
            else
            {
                night.button.interactable = true;
            }
        }
    }

    public void SelectNight(PerformanceDataSO nightToSelect)
    {
        selectedNight = nightToSelect;
        //Hide Night UI
        HideNightSelection();
    }

    public void ShowNightSelection(List<PerformanceDataSO> nights)
    {
        ClearNightsUI();
        GenerateNightsUI(nights);
        //Lock/Unlock night ui elements based on completion progress
        
        gameObject.SetActive(true);
    }

    void HideNightSelection()
    {
        gameObject.SetActive(false);
        OnPerformanceSelected?.Invoke(selectedNight);
        CurtainsUI.Instance.OpenCurtains();
    }
    
    #endregion 
}
