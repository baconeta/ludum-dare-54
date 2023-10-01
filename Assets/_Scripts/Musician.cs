using System;
using System.Collections.Generic;
using _Scripts.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

#region Demographics

[Serializable]
public enum Gender
{
    Male, Female, Transgender, Nonbinary, Genderfluid, TwoSpirit, Agender, Bigender, Demigender, Androgynous, Pangender, Nonconforming, Intersex, Cisgender, Genderqueer, Questioning, Other, NumOfElements
}

[Serializable]
public enum FirstNames
{
    Emma, Liam, Olivia, Noah, Sophia, Jackson, Ava, Aiden, Isabella, Lucas, Mia, Ethan, Harper, Oliver, Amelia, Elijah, Charlotte, Logan, Mason, NumOfElements
}

[Serializable]
public enum Nicknames
{
    Bubbles, Sunshine, Sparky, Tinkerbell, Chuckles, Snickers, Dizzy, Giggles, Wiggles, Jellybean, Noodle, Peaches, Bouncy, Tootsie, Pickles, Twinkle, Doodlebug, Cupcake, Pudding, Sprout, NumOfElements
}

[Serializable]
public enum LastNames
{
    Smith, Johnson, Williams, Jones, Brown, Davis, Miller, Wilson, Moore, Taylor, Anderson, Thomas, Jackson, White, Harris, Martin, Thompson, Garcia, Martinez, Robinson, NumOfElements
}

[Serializable]
public enum FacingDirection
{
    Left, Right, Forward
}

#endregion

public class Musician : MonoBehaviour
{
    public MusicianPointer worldObject;

    [Header("Musician Bio")]
    [SerializeField] private MusicianDataSO data;

    private void Awake()
    {
        worldObject = GetComponentInChildren<MusicianPointer>();
        worldObject.parentMusician = this;
    }
    
    public Musician GenerateMusician()
    {
        data.musicianNameFirst = ((FirstNames) Random.Range(0, (int) FirstNames.NumOfElements)).ToString();
        data.musicianNameNickname = ((Nicknames) Random.Range(0, (int) Nicknames.NumOfElements)).ToString();
        data.musicianNameLast = ((LastNames) Random.Range(0, (int) LastNames.NumOfElements)).ToString();

        return this;
    }

    public void SetMusicianData(MusicianDataSO musicianData)
    {
        data = musicianData;
        GetComponent<Image>().sprite = data.portraitImage;
        SpriteRenderer worldSprite = worldObject?.GetComponent<SpriteRenderer>();
        if(worldSprite) worldSprite.sprite = data.gameImage;
    }

    public List<InstrumentType> GetProficientInstruments()
    {
        return data.proficientInstruments;
    }
    
    public List<InstrumentType> GetBadInstruments()
    {
        return data.badInstruments;
    }

    public MusicianDataSO GetAllMusicianData()
    {
        return data;
    }

    private string GenerateBio()
    {
        return "I like horses.";
    }
}