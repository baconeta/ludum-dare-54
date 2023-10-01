using UnityEngine;
using static Managers.PerformanceManager;

public class ReviewManager : MonoBehaviour
{
    public enum StarRating
    {
        Bombed = 1,
        Mediocre = 2, // I think that this one should be something like "bad" instead - bombed to mediocre seems like a big jump to me.
        Entertaining = 3,
        Wonderful = 4,
        Awe_Inspiring = 5,
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
