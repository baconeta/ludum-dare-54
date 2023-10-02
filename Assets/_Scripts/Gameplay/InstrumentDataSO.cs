﻿using System;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [Serializable] [CreateAssetMenu]
    public class InstrumentDataSO : ScriptableObject
    {
        public bool isBehindMusician;
        public InstrumentType instrumentType;
        public Sprite inGameSprite;
        public Sprite uiSprite;
        public FacingDirection facingDirection;

        public AudioClip backupGoodClip;
        public AudioClip backupBadClip;
    }
}