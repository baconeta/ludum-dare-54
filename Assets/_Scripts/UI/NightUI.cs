using _Scripts.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NightUI : MonoBehaviour
{
    public PerformanceDataSO performance;
    public TextMeshProUGUI nightText;
    public TextMeshProUGUI questText;
    public TextMeshProUGUI composerNameText;
    public Button button;
    public Image[] stars;
    public Sprite emptyStar;
    public Sprite halfStar;
    public Sprite fullStar;

    public void FillStars(ReviewManager.StarRating value)
    {
        // TODO fill the stars based on the previous scores / recent score
    }
}
