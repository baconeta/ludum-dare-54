using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceReview : MonoBehaviour
{
    public GameObject reviewGO;
    [Header("Newspaper")]
    public TextMeshProUGUI newspaperHeader;
    public Image newspaperImage;

    public void OnEnable()
    {
        PerformanceManager.OnPerformanceComplete += ShowReview;
    }

    public void OnDisable()
    {
        PerformanceManager.OnPerformanceComplete -= ShowReview;
    }

    public void ShowReview(PhaseManager.GamePhase newState)
    {
        //TODO Set newspaperHeader.text and newspaperImage.sprite
        CurtainsUI.Instance.CloseCurtains();
        reviewGO.SetActive(true);
    }
}
