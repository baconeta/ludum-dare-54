using System;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [Serializable]
    [CreateAssetMenu]
    public class ReviewDataSO : ScriptableObject
    {
        public MusicianAppropriatenessFeedback musicianChoiceFeedback;
        public InstrumentFeedback instrumentChoiceFeedback;
        public AffinityFeedback affinityFeedback;

        [System.Serializable]
        public struct MusicianAppropriatenessFeedback
        {
            // Hi = 4+ stars.
            // Lo = 3.5 stars or lower.
            public string appropriateHi;
            public string appropriateLo;
            public string inappropriateHi;
            public string inappropriateLo;
            public string mixedHi;
            public string mixedLo;
            public string neither;
        }

        [Serializable]
        public struct InstrumentFeedback
        {
            public string perfectVariation;
            public string averageVariation;
            public string terribleVariation;
        }

        [Serializable]
        public struct AffinityFeedback
        {
            public string perfectVariation;
            public string averageVariation;
            public string terribleVariation;
        }
    }
}
