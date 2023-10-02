using System;
using System.Collections.Generic;
using _Scripts.Gameplay;
using UI.Popups;
using UnityEngine;

public class NightSelection : MonoBehaviour
{
    private PerformanceDataSO selectedNight;
    private PopupManager popupManager;

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
        popupManager = FindObjectOfType<PopupManager>();
        for (int i = 0; i < nights.Count; i++)
        {
            NightUI night = Instantiate(nightUIPrefab, nightHolder).GetComponent<NightUI>();
            // Set the night title text
            if (i == 0) night.nightText.text = night1Text;
            else if (i == nights.Count - 1) night.nightText.text = lastNightText;
            else night.nightText.text = $"{nightMiddleText} {i}";
            // Set the quest title text
            night.questText.text = nights[i].trackData.questName;
            // Set the composer name text
            night.composerNameText.text = nights[i].trackData.composerName;
            // Save the performance data
            night.performance = nights[i];

            // If not unlocked, then hide/non-interactable
            if (i > PlayerPrefs.GetInt("NightsComplete"))
            {
                night.button.interactable = false;
                night.FadeElements();
            }
            else
            {
                night.button.interactable = true;
                // On all interactable buttons, set a listener for us to open the popup with on press
                popupManager.AddPressPopup(new PopupManager.PopupPair(popupManager.performanceInfoPopup, night.gameObject));
                night.GetComponent<PopupManager.PressListenerForPopup>().SetCallBack(() =>
                {
                    PerformanceInfoPopup mPopup = FindObjectOfType<PerformanceInfoPopup>();
                    mPopup?.SetPerformanceCardInfo(night.performance, true);
                });
            }
        }
    }

    public void SelectNight(PerformanceDataSO nightToSelect)
    {
        selectedNight = nightToSelect;
        popupManager.HideAll();
        
        // Hide Night UI
        HideNightSelection();
    }

    public void ShowNightSelection(List<PerformanceDataSO> nights)
    {
        ClearNightsUI();
        GenerateNightsUI(nights);
        // Lock/Unlock night ui elements based on completion progress
        
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
