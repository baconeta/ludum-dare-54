using _Scripts.Gameplay;
using UnityEngine;
using static Managers.PerformanceManager;


public class ReviewManager : MonoBehaviour
{
    public NewspaperUI reviewNewspaper;
    [System.Serializable]
    public enum StarRating
    {
        TBD = 0,
        Bombed = 1,
        Bad = 2, // 1 star
        Passable = 3,
        Mediocre = 4, // 2 star
        Good = 5,
        Entertaining = 6, // 3 star
        Wonderful = 7,
        Excellent = 8, // 4 star
        Awe_Inspiring = 9,
        Life_Changing = 10, // 5 star
    }

    private AffinityScores latestPerformance;
    private StarRating latestRating = StarRating.TBD;
    private int maxScore;
    private int minScore;

    public void UpdatePerformanceData(AffinityScores performanceData, int maxScore, int minScore)
    {
        latestPerformance = performanceData;
        latestRating = StarRating.TBD;
        this.maxScore = maxScore;
        this.minScore = minScore;
    }

    public StarRating GetPerformanceRating()
    {
        if (latestRating == StarRating.TBD)
        {
            CalculateRating();
        }

        //TODO I need the real ReviewDataSO.
        ReviewDataSO reviewDataSo = new ReviewDataSO();
        reviewNewspaper.SetNewspaperUI(reviewDataSo);
        return latestRating;
    }

    // Used for lazy-evaluation of ratings.
    private void CalculateRating()
    {
        // TODO Add weighting to affinity scores.
        latestRating = StarRating.Wonderful;
        
        UpdatePersonalHighscores();
    }

    private void UpdatePersonalHighscores()
    {
        // Determine which PlayerPref to access.
        NightManager nightManager = FindObjectOfType<NightManager>();
        int currentNight = (nightManager ? nightManager.currentNight : -1);
        string identifier = $"night_{currentNight}_personal_highscore";

        // Store only the greatest score.
        int storedRating = PlayerPrefs.GetInt(identifier);
        if (storedRating < (int) latestRating)
        {
            PlayerPrefs.SetInt(identifier, (int) latestRating);
        }
    }
}
