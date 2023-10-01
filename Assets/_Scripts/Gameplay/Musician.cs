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
    Male, Female, Transgender, Nonbinary, Genderfluid, TwoSpirit, Agender, Bigender, Demigender, Androgynous, Pangender, Nonconforming, Intersex, Cisgender, Genderqueer, Questioning, Other
}

[Serializable]
public enum FirstNames
{
    Emma, Liam, Olivia, Noah, Sophia, Jackson, Ava, Aiden, Isabella, Lucas, Mia, Ethan, Harper, Oliver, Amelia, Elijah, Charlotte, Logan, Mason
}

[Serializable]
public enum Nicknames
{
    Bubbles, Sunshine, Sparky, Tinkerbell, Chuckles, Snickers, Dizzy, Giggles, Wiggles, Jellybean, Noodle, Peaches, Bouncy, Tootsie, Pickles, Twinkle, Doodlebug, Cupcake, Pudding, Sprout
}

[Serializable]
public enum LastNames
{
    Smith, Johnson, Williams, Jones, Brown, Davis, Miller, Wilson, Moore, Taylor, Anderson, Thomas, Jackson, White, Harris, Martin, Thompson, Garcia, Martinez, Robinson
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

        PopupManager popupManager = FindObjectOfType<PopupManager>();
        popupManager.AddPressPopup(new PopupManager.PopupPair(popupManager.musicianPopup, gameObject));
    }
    
    public Musician GenerateMusician()
    {
        // Randomly select data.
        data.musicianNameFirst = ((FirstNames) Random.Range(0, (int) Enum.GetNames(typeof(FirstNames)).Length)).ToString();
        data.musicianNameNickname = ((Nicknames) Random.Range(0, (int) Enum.GetNames(typeof(Nicknames)).Length)).ToString();
        data.musicianNameLast = ((LastNames) Random.Range(0, (int) Enum.GetNames(typeof(LastNames)).Length)).ToString();

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