using System;
using System.Collections.Generic;
using _Scripts.Gameplay;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NightUI : MonoBehaviour
{
    public PerformanceDataSO performance;
    public TextMeshProUGUI nightText;
    public TextMeshProUGUI questText;
    public TextMeshProUGUI composerNameText;
    public Button button;
    [SerializeField] public List<Image> stars;
    [SerializeField] public GameObject StarContainer;
    public Sprite emptyStar;
    public Sprite halfStar;
    public Sprite fullStar;

    public ReviewManager.StarRating starsAchieved;

    public void Start()
    {
        //AUDIO on spawn
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        AudioWrapper audioWrapper = FindObjectOfType<AudioWrapper>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        //Add selection sound
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { audioWrapper.PlaySound("Selection"); });
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { audioWrapper.PlaySound("Hover"); });
        eventTrigger.triggers.Add(entry);

        //Fill stars
        stars = new List<Image>(StarContainer.GetComponentsInChildren<Image>());
        FillStars(starsAchieved);
    }


    public void FillStars(ReviewManager.StarRating value)
    {
        foreach (Image s in stars)
        {
            s.sprite = emptyStar;
        }
        
        if ((int) value % 2 == 0)
        {
            for (int i = 0; i < (int) value / 2; i++)
            {
                stars[i].sprite = fullStar;
            }
        }
        else
        {
            for (int i = 0; i < Math.Ceiling((float) value / 2f); i++)
            {
                stars[i].sprite = fullStar;
            }

            stars[(int) Math.Floor((float) value / 2)].sprite = halfStar;
        }
    }
    
    public void FadeElements()
    {
        foreach (Image star in stars)
        {
            if (star is not null)
            {
                Color starColor = star.color;
                starColor.a = 0.2f;
                star.color = starColor;
            }
        }

        Color textColor = nightText.color;
        textColor.a = 0.2f;
        nightText.color = textColor;

        textColor = questText.color;
        textColor.a = 0.2f;
        questText.color = textColor;

        textColor = composerNameText.color;
        textColor.a = 0.2f;
        composerNameText.color = textColor;
    }
}