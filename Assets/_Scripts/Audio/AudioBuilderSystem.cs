using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioBuilderSystem : MonoBehaviour
    {
        [SerializeField] private CustomAudioSource _customAudioSource;
        private List<AudioClip> _builtClips;

        private void Awake()
        {
            _builtClips = new List<AudioClip>();
        }

        public void AddClipToBuilder(AudioClip clip)
        {
            _builtClips.Add(clip);
        }
        
        public float PlayBuiltClips()
        {
            float longestClip = 0;
            foreach (AudioClip clip in _builtClips)
            {
                if(clip.length > longestClip) longestClip = clip.length;
                _customAudioSource.PlayOnce(clip);
            }
            return longestClip;
        }
    }
}