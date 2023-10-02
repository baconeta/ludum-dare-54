using UnityEngine;
using System;

public class StagePlacement : MonoBehaviour
{
    [SerializeField] private Musician occupyingMusician;
    [SerializeField] private Instrument occupyingInstrument;
    [SerializeField] private FacingDirection stageSide;

    public static event Action<StagePlacement> OnMusicianPlaced; 
    public static event Action<StagePlacement> OnInstrumentPlaced;

    public void Start()
    {
        if (transform.position.x < -1) stageSide = FacingDirection.Left;
        else if (transform.position.x > 1) stageSide = FacingDirection.Right;
        else stageSide = FacingDirection.Forward;
    }

    public bool SetMusician(Musician musician, Transform worldMusician)
    {
        if (occupyingMusician) return false;

        occupyingMusician = musician;
        //If stage side doesnt match, and its not forward, then flip the sprite.
        if (stageSide == occupyingMusician.GetAllMusicianData().worldFacingDirection && stageSide != FacingDirection.Forward)
        {
            worldMusician.GetComponent<SpriteRenderer>().flipX = true;
        }
        worldMusician.SetPositionAndRotation(transform.position, transform.rotation);
        worldMusician.parent = transform;
        OnMusicianPlaced?.Invoke(this);
        return true;
    }
    
    public bool SetInstrument(Instrument instrument, Transform worldInstrument)
    {
        if (occupyingInstrument) return false;

        occupyingInstrument = instrument;
        SpriteRenderer worldSprite = worldInstrument.GetComponent<SpriteRenderer>();
        //If stage side doesnt match, and its not forward, then flip the sprite.
        if (stageSide == occupyingInstrument.data.facingDirection && stageSide != FacingDirection.Forward)
        {
            worldSprite.flipX = true;
        }

        if (occupyingInstrument.data.isBehindMusician)
        {
            worldSprite.sortingOrder = -1;
        }
        worldInstrument.SetPositionAndRotation(transform.position, transform.rotation);
        worldInstrument.parent = transform;
        OnInstrumentPlaced?.Invoke(this);
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

    public void Clear()
    {
        if(occupyingInstrument)
            Destroy(occupyingInstrument.worldObject.gameObject);
        if (occupyingMusician)
            Destroy(occupyingMusician.worldObject.gameObject);
        
        occupyingInstrument = null;
        occupyingMusician = null;

    }

    /// <summary>
    /// Returns if the Stage Placement is full.
    /// </summary>
    /// <returns>(bool, bool) of (hasMusician, hasInstrument)</returns>
    public (bool,bool) IsOccupied()
    {
        return (occupyingMusician != null, occupyingInstrument != null);
    }

    public Musician GetMusician()
    {
        return occupyingMusician;
    }
    
    public Instrument GetInstrument()
    {
        return occupyingInstrument;
    }
}
