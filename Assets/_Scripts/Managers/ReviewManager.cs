using _Scripts.Gameplay;
using Managers;
using System.Collections;
using System.Collections.Generic;
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
        return StarRating.Wonderful;
    }
    // Correct musicians
    // Correct instruments
    // Musician-Instrument proficiency
}
