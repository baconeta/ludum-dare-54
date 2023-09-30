using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

#region Demographics
[Serializable]
public enum Instrument
{
    Violin, Viola, Cello, Bass, Flute, Oboe, Clarinet, Bassoon, Horn, Trumpet, Trombone, Tuba, Harp, Piano, Timpani, Xylophone, Marimba, Celesta, Piccolo, Contrabassoon, Saxophone, Triangle, Cymbals, Drum, Tambourine, Gong, Vibraphone, Organ, NumOfElements
}
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
#endregion

public class Musician : MonoBehaviour
{
    [Header("Musician Bio")]
    public string musicianNameFirst;
    public string musicianNameNickname;
    public string musicianNameLast;
    public Instrument instrument;
    public int age;
    public Gender gender;
    public string bio;

    public static event Action<Musician> MusicianDragged;

    [Header("Tooltip Popup")] 
    [SerializeField] private TextMeshProUGUI popupNameText;
    [SerializeField] private TextMeshProUGUI popupInstrumentText;
    [SerializeField] private TextMeshProUGUI popupAgeText;
    [SerializeField] private TextMeshProUGUI popupGenderText;
    [SerializeField] private TextMeshProUGUI popupBioText;

    // Start is called before the first frame update
    public Musician GenerateMusician()
    {
        musicianNameFirst = ((FirstNames)Random.Range(0, (int)FirstNames.NumOfElements)).ToString();
        musicianNameNickname = ((Nicknames)Random.Range(0, (int)Nicknames.NumOfElements)).ToString();
        musicianNameLast = ((LastNames)Random.Range(0, (int)LastNames.NumOfElements)).ToString();
        
        //Set Tooltip Information
        popupNameText.text = $"{musicianNameFirst} '{musicianNameNickname}' {musicianNameLast}";
        popupInstrumentText.text = $"{instrument = (Instrument)Random.Range(0, (int)Instrument.NumOfElements)}";
        popupAgeText.text = $"A: {age = Random.Range(18, 100)}";
        popupGenderText.text = $"G: {gender = (Gender)Random.Range(0, (int)Gender.NumOfElements)}";
        popupBioText.text = GenerateBio();
        return this;
    }

    string GenerateBio()
    {
        return "I like horses.";
    }

    public void CopyMusician(Musician musicianToCopy)
    {
        musicianNameFirst = musicianToCopy.musicianNameFirst;
        musicianNameNickname = musicianToCopy.musicianNameNickname;
        musicianNameLast = musicianToCopy.musicianNameLast;
        instrument = musicianToCopy.instrument;
        age = musicianToCopy.age;
        gender = musicianToCopy.gender;
        bio = musicianToCopy.bio;
    }
    
    //Informs the MusicianDragger that this card has been grabbed.
    public void MusicianCardDragged()
    {
        MusicianDragged?.Invoke(this);
    }

}
