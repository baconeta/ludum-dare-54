using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public StarRating getPerformanceRating()
    {

    }
}
