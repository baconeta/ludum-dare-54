using _Scripts.Gameplay;
using UnityEngine;
using UnityEngine.UI;

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
        instrumentType = (InstrumentType)Random.Range(0, (int)InstrumentType.NumOfElements);
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
    Violin, ElectricGuitar, BassGuitar, Trumpet, Piano, Xylophone, Saxophone, Triangle, Cymbals, Drums, Tambourine, Organ, Synth, NumOfElements
}
