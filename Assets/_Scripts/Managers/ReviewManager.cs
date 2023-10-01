using UnityEngine;
using static Managers.PerformanceManager;

public class ReviewManager : MonoBehaviour
{
    public enum StarRating
    {
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

    public void UpdatePerformanceData(AffinityScores data)
    {
        latestPerformance = data;
    }

    public StarRating getPerformanceRating()
    {
        // TODO Add weighting to affinity scores.
        return StarRating.Wonderful;
    }
}
