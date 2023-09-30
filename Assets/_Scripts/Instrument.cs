using UnityEngine;

public class Instrument : MonoBehaviour
{
    public enum InstrumentType
    {
        Violin, Viola, Cello, Bass, Flute, Oboe, Clarinet, Bassoon, Horn, Trumpet, Trombone, Tuba, Harp, Piano, Timpani, Xylophone, Marimba, Celesta, Piccolo, Contrabassoon, Saxophone, Triangle, Cymbals, Drum, Tambourine, Gong, Vibraphone, Organ, NumOfElements
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
