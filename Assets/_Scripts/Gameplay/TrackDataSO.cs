using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [Serializable]
    public struct TrackInstrumentPairs
    {
        public Instrument.InstrumentType instrumentType;
        public AudioClip audioClip;
    }

    [Serializable] [CreateAssetMenu]
    public class TrackDataSO : ScriptableObject
    {
        public string trackName;
        public string questName;
        public int numberOfMusiciansToPlay;
        public List<TrackInstrumentPairs> correctTrackInstrumentPairsList;
    }
}