using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#region Demographics

[Serializable]
public enum Gender
{
    Male,
    Female,
    Transgender,
    Nonbinary,
    Genderfluid,
    TwoSpirit,
    Agender,
    Bigender,
    Demigender,
    Androgynous,
    Pangender,
    Nonconforming,
    Intersex,
    Cisgender,
    Genderqueer,
    Questioning,
    Other,
    NumOfElements
}

[Serializable]
public enum FirstNames
{
    Emma,
    Liam,
    Olivia,
    Noah,
    Sophia,
    Jackson,
    Ava,
    Aiden,
    Isabella,
    Lucas,
    Mia,
    Ethan,
    Harper,
    Oliver,
    Amelia,
    Elijah,
    Charlotte,
    Logan,
    Mason,
    NumOfElements
}

[Serializable]
public enum Nicknames
{
    Bubbles,
    Sunshine,
    Sparky,
    Tinkerbell,
    Chuckles,
    Snickers,
    Dizzy,
    Giggles,
    Wiggles,
    Jellybean,
    Noodle,
    Peaches,
    Bouncy,
    Tootsie,
    Pickles,
    Twinkle,
    Doodlebug,
    Cupcake,
    Pudding,
    Sprout,
    NumOfElements
}

[Serializable]
public enum LastNames
{
    Smith,
    Johnson,
    Williams,
    Jones,
    Brown,
    Davis,
    Miller,
    Wilson,
    Moore,
    Taylor,
    Anderson,
    Thomas,
    Jackson,
    White,
    Harris,
    Martin,
    Thompson,
    Garcia,
    Martinez,
    Robinson,
    NumOfElements
}

[Serializable]
public enum FacingDirection
{
    Left,
    Right,
    Forward
}

#endregion

public class Musician : MonoBehaviour
{
    private MusicianPointer worldObject;
    [Header("Musician Bio")] public string musicianNameFirst;
    public string musicianNameNickname;
    public string musicianNameLast;
    public int age;
    public Gender gender;
    public string bio;
    public FacingDirection worldFacingDirection = FacingDirection.Forward;

    [SerializeField] public List<Instrument.InstrumentType> proficientInstruments;
    [SerializeField] public List<Instrument.InstrumentType> badInstruments;

    private void Awake()
    {
        worldObject = GetComponentInChildren<MusicianPointer>();
        worldObject.parentMusician = this;
    }

    // Start is called before the first frame update
    public Musician GenerateMusician()
    {
        musicianNameFirst = ((FirstNames) Random.Range(0, (int) FirstNames.NumOfElements)).ToString();
        musicianNameNickname = ((Nicknames) Random.Range(0, (int) Nicknames.NumOfElements)).ToString();
        musicianNameLast = ((LastNames) Random.Range(0, (int) LastNames.NumOfElements)).ToString();

        return this;
    }

    string GenerateBio()
    {
        return "I like horses.";
    }
}