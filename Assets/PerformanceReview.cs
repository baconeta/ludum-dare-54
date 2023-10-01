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

    void OnEnable()
    {
        PerformanceManager.OnPerformanceComplete += ShowReview;
    }

    void OnDisable()
    {
        PerformanceManager.OnPerformanceComplete -= ShowReview;
    }

    void ShowReview(float score)
    {
        //TODO Set newspaperHeader.text and newspaperImage.sprite
        reviewGO.SetActive(true);
    }
}
