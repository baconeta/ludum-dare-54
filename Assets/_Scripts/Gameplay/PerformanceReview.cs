using UnityEngine;

public class PerformanceReview : MonoBehaviour
{
    public GameObject reviewGO;

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
            CurtainsUI.Instance.CloseCurtains();
            reviewGO.SetActive(true);
        }
    }
}
