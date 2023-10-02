using System;
using UnityEngine;
using static Managers.PerformanceManager;


public class ReviewManager : MonoBehaviour
{
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
        return latestRating;
    }

    // Used for lazy-evaluation of ratings.
    private void CalculateRating()
    {
        // Get a score based on each element. This implicitly will range between minScore and maxScore.
        int score = 0;
        score += latestPerformance.correctInstrumentCount;
        score += latestPerformance.instrumentExpertiseCount - latestPerformance.instrumentFumbleCount;
        score += latestPerformance.synergisticMusicianCount - latestPerformance.unsuitableMusicianCount;
        // Make scores all-positive.
        score += minScore;
        maxScore += minScore;
        minScore = 0;
        // Convert to a 0-to-10 scale.
        float scalar = 10 / maxScore;
        score = (int) Math.Floor(score * 10.0 / maxScore);
        maxScore = (int) Math.Floor(maxScore * 10.0 / maxScore);
        // Convert the numeric value into a star rating.
        latestRating = (StarRating) score;
        // Update personal highscores.
        UpdatePersonalHighscores();
    }

    private void UpdatePersonalHighscores()
    {
        // Determine which PlayerPref to access.
        int currentNight = GetComponent<NightManager>().currentNight;
        string identifier = $"night_{currentNight}_personal_highscore";

        // Store only the greatest score.
        int storedRating = PlayerPrefs.GetInt(identifier);
        if (storedRating < (int) latestRating)
        {
            PlayerPrefs.SetInt(identifier, (int) latestRating);
        }
    }
}
