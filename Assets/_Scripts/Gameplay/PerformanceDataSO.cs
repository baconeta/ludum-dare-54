using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [Serializable] [CreateAssetMenu]
    public class PerformanceDataSO : ScriptableObject
    {
        // A performance needs to know about:
        // Which track should be playing and all relevant info
        // Which musicians are available to be chosen for this track
        // Which instruments can be chosen for this track
        [SerializeField] public TrackDataSO trackData;
        [SerializeField] public List<MusicianDataSO> musicians;
        [SerializeField] public List<MusicianDataSO> correctMusicians;
        [SerializeField] public List<MusicianDataSO> incorrectMusicians;
        [SerializeField] public List<InstrumentDataSO> instruments;
    }
}
