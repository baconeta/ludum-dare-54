using UnityEngine;

public class Instrument : MonoBehaviour
{
    public enum InstrumentType
    {
        Violin, ElectricGuitar, BassGuitar, Trumpet, Piano, Xylophone, Saxophone, Triangle, Cymbals, Drums, Tambourine, Organ, Synth, NumOfElements
    }

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
}
