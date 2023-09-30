using System;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [Serializable] [CreateAssetMenu]
    public class InstrumentDataSO : ScriptableObject
    {
        public InstrumentType instrumentType;
        public Sprite inGameSprite;
        public FacingDirection facingDirection;
    }
}