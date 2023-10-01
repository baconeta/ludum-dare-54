using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [Serializable] [CreateAssetMenu]
    public class MusicianDataSO : ScriptableObject
    {
        public Sprite portraitImage;
        public Sprite gameImage;
        
        public string musicianNameFirst;
        public string musicianNameNickname;
        public string musicianNameLast;
        public int age;
        public Gender gender;
        public string bio;
        public FacingDirection worldFacingDirection = FacingDirection.Forward; // Will determine if we should flip a char based on stage position

        [SerializeField] public List<InstrumentType> proficientInstruments;
        [SerializeField] public List<InstrumentType> badInstruments;
    }
}