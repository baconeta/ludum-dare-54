using System;
using UnityEngine;
using _Scripts.Gameplay;
using Managers;
using static Managers.PerformanceManager;


public class ReviewManager : MonoBehaviour
{
    public NewspaperUI reviewNewspaper;
    [Serializable]
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

    [SerializeField] public ReviewDataSO TutorialReviewData;

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
        
        PerformanceDataSO currentPerformanceData = FindObjectOfType<PerformanceManager>().GetCurrentPerformanceData();
        ReviewDataSO reviewDataSo = currentPerformanceData.reviewData;
        
        if (TutorialController.IsTutorial)
        {
            reviewDataSo = TutorialReviewData;
        }
        
        reviewNewspaper.SetNewspaperUI(reviewDataSo, latestRating);
        return latestRating;
    }

    // Used for lazy-evaluation of ratings.
    private void CalculateRating()
    {
        // Get a score based on each element. This implicitly will range between minScore and maxScore.
        int score = 0;
        score += latestPerformance.correctInstrumentCount;
        score += latestPerformance.instrumentExpertiseCount;
        score -= latestPerformance.instrumentFumbleCount;
        score += latestPerformance.synergisticMusicianCount;
        score -= latestPerformance.unsuitableMusicianCount;
        // Make scores all-positive.
        float result = Map(score, minScore, maxScore, 0f, 10f);
        Debug.Log(minScore + " " + maxScore + " " + score + " " + result);
        // Convert the numeric value into a star rating.
        latestRating = (StarRating) (int) result;
        // Update personal highscores.
        UpdatePersonalHighscores();
    }

    private void UpdatePersonalHighscores()
    {
        // Determine which PlayerPref to access.
        PerformanceDataSO currentPerformanceData = FindObjectOfType<PerformanceManager>().GetCurrentPerformanceData();
        int currentNight = currentPerformanceData.performanceKey;
        string identifier = $"night_{currentNight}_personal_highscore";

        // Store only the greatest score.
        int storedRating = PlayerPrefs.GetInt(identifier);
        if (storedRating < (int) latestRating)
        {
            PlayerPrefs.SetInt(identifier, (int) latestRating);
        }
    }
    
    // Map a value from one range to another.
    public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // First, make sure the value is within the source range.
        value = Mathf.Clamp(value, fromMin, fromMax);

        // Calculate the percentage of value within the source range.
        float percentage = (value - fromMin) / (fromMax - fromMin);

        // Map the percentage to the target range.
        float mappedValue = Mathf.Lerp(toMin, toMax, percentage);

        return mappedValue;
    }
}
