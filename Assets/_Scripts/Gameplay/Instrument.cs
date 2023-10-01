using _Scripts.Gameplay;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Instrument : MonoBehaviour
{
    public InstrumentDataSO data;

    public Image cardSprite;
    public InstrumentType instrumentType;
    public InstrumentPointer worldObject;

    private void Awake()
    {
        worldObject = GetComponentInChildren<InstrumentPointer>();
        worldObject.parentInstrument = this;
    }
    public Instrument GenerateInstrument()
    {
        // Randomly select an instrument.
        instrumentType = (InstrumentType)Random.Range(0, (int) Enum.GetNames(typeof(InstrumentType)).Length);
        return this;
    }

    public void SetInstrumentData(InstrumentDataSO instrumentData)
    {
        data = instrumentData;
        instrumentType = data.instrumentType;
        GetComponent<Image>().sprite = data.uiSprite;
        SpriteRenderer worldSprite = worldObject?.GetComponent<SpriteRenderer>();
        if (worldSprite is not null)
        {
            worldSprite.sprite = data.inGameSprite;
        }

    }
}

public enum InstrumentType
{
    Violin, ElectricGuitar, BassGuitar, Trumpet, Piano, Xylophone, Saxophone, Triangle, Cymbals, Drums, Tambourine, Organ, Synth, Flute, Bagpipes, Cello
}
