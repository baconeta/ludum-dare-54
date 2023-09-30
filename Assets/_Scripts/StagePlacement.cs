using UnityEngine;
using System;

public class StagePlacement : MonoBehaviour
{
    [SerializeField] private Musician occupyingMusician;
    [SerializeField] private Instrument occupyingInstrument;
    public static event Action OnMusicianPlaced; 
    public static event Action OnInstrumentPlaced; 

    public bool SetMusician(Musician musician, Transform worldMusician)
    {
        if (occupyingMusician) return false;

        occupyingMusician = musician;
        worldMusician.SetPositionAndRotation(transform.position, transform.rotation);
        worldMusician.parent = transform;
        OnMusicianPlaced?.Invoke();
        return true;
    }
    
    public bool SetInstrument(Instrument instrument, Transform worldInstrument)
    {
        if (occupyingInstrument) return false;

        occupyingInstrument = instrument;
        worldInstrument.SetPositionAndRotation(transform.position, transform.rotation);
        worldInstrument.parent = transform;
        OnInstrumentPlaced?.Invoke();
        return true;
    }
    
    public bool SetObject(GameObject obj)
    {
        InstrumentPointer instrument = obj.GetComponent<InstrumentPointer>();
        if (instrument) return SetInstrument(instrument.parentInstrument, instrument.transform);
        
        MusicianPointer musician = obj.GetComponent<MusicianPointer>();
        if (musician) return SetMusician(musician.parentMusician, musician.transform);

        return false;
    }

    /// <summary>
    /// Returns if the Stage Placement is full.
    /// </summary>
    /// <returns>(bool, bool) of (hasMusician, hasInstrument)</returns>
    public (bool,bool) IsOccupied()
    {
        return (occupyingMusician != null, occupyingInstrument != null);
    }

}
