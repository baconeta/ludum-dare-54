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

    [ContextMenu("Test me")]
    public void FadeElements()
    {
        foreach (Image star in stars)
        {
            if (star is not null)
            {
                Color starColor = star.color;
                starColor.a = 0.2f;
                star.color = starColor;
            }
        }
        
        Color textColor = nightText.color;
        textColor.a = 0.2f;
        nightText.color = textColor;
        
        textColor = questText.color;
        textColor.a = 0.2f;
        questText.color = textColor;
        
        textColor = composerNameText.color;
        textColor.a = 0.2f;
        composerNameText.color = textColor;
    }
}
