using _Scripts.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class Instrument : MonoBehaviour
{
    public InstrumentDataSO data;

    public InstrumentType instrumentType;
    private InstrumentPointer worldObject;

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
        Image image = worldObject?.GetComponent<Image>();
        if (image is not null)
        {
            image.sprite = data.inGameSprite;
        }
    }
}

public enum InstrumentType
{
    Violin, ElectricGuitar, BassGuitar, Trumpet, Piano, Xylophone, Saxophone, Triangle, Cymbals, Drums, Tambourine, Organ, Synth, NumOfElements
}
