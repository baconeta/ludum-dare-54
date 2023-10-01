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
        PhaseManager.OnGamePhaseChange += ShowReview;
    }

    public void OnDisable()
    {
        PhaseManager.OnGamePhaseChange -= ShowReview;
    }

    public void ShowReview(PhaseManager.GamePhase newState)
    {
        if (newState == PhaseManager.GamePhase.Review)
        {
            //TODO Set newspaperHeader.text and newspaperImage.sprite
            CurtainsUI.Instance.CloseCurtains();
            reviewGO.SetActive(true);
        }
    }
}
