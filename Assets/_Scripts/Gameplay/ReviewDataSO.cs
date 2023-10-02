using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Gameplay
{
    [Serializable]
    [CreateAssetMenu]
    public class ReviewDataSO : ScriptableObject
    {
        public MusicianAppropriatenessFeedback musicianChoiceFeedback;
        public InstrumentFeedback instrumentChoiceFeedback;
        public AffinityFeedback affinityFeedback;
        public ReviewImageChoices reviewImageChoices;
        public int halfStars;
        public string date;
        public string caption;
        public int issueNumber;
        public int volNumber;
        public string reviewTitle;
        public string reviewSubTitle;
        public Sprite reviewImage;

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
        
        [Serializable]
        public struct ReviewImageChoices
        {
            public Sprite perfectVariation;
            public Sprite averageVariation;
            public Sprite terribleVariation;
        }
    }
}
