﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [Serializable]
    public struct TrackInstrumentPairs
    {
        public InstrumentType instrumentType;
        public AudioClip audioClip;
    }

    [Serializable] [CreateAssetMenu]
    public class TrackDataSO : ScriptableObject
    {
        public string trackName;
        public string questName;
        public int numberOfMusiciansToPlay;
        public string composerName;
        public string info;
        public List<TrackInstrumentPairs> correctTrackInstrumentPairsList;
        public List<TrackInstrumentPairs> intentionalBadInstrumentPairsList;
    }
}