using System;
using _Scripts.Gameplay;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewspaperUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI subtitle;
    public TextMeshProUGUI column1;
    public TextMeshProUGUI column2;
    public TextMeshProUGUI column3;
    public TextMeshProUGUI caption;
    public TextMeshProUGUI date;
    public TextMeshProUGUI issueNumber;
    public TextMeshProUGUI volNumber;
    public Image image;
    public Image[] stars;

    public Sprite fullStar;
    public Sprite halfStar;

    public void SetNewspaperUI(ReviewDataSO review, ReviewManager.StarRating starRating, PerformanceManager.AffinityScores performanceAffinity, PerformanceDataSO data)
    {
        title.text = starRating switch
        {
            ReviewManager.StarRating.TBD => review.reviewTitle1,
            ReviewManager.StarRating.Bombed => review.reviewTitle1,
            ReviewManager.StarRating.Bad => review.reviewTitle1,
            ReviewManager.StarRating.Passable => review.reviewTitle2,
            ReviewManager.StarRating.Mediocre => review.reviewTitle2,
            ReviewManager.StarRating.Good => review.reviewTitle4,
            ReviewManager.StarRating.Entertaining => review.reviewTitle3,
            ReviewManager.StarRating.Wonderful => review.reviewTitle4,
            ReviewManager.StarRating.Excellent => review.reviewTitle4,
            ReviewManager.StarRating.Awe_Inspiring => review.reviewTitle5,
            ReviewManager.StarRating.Life_Changing => review.reviewTitle5,
            _ => throw new ArgumentOutOfRangeException(nameof(stars), stars, null)
        };

        subtitle.text = review.reviewSubTitle;

        switch (performanceAffinity)
        {
            // column one is based on the PEOPLE
            case {synergisticMusicianCount: > 0, unsuitableMusicianCount: 0} when (int) starRating >= 8:
                column1.text = review.musicianChoiceFeedback.appropriateHi;
                break;
            case {synergisticMusicianCount: > 0, unsuitableMusicianCount: 0} when (int) starRating < 8:
                column1.text = review.musicianChoiceFeedback.appropriateLo;
                break;
            case {synergisticMusicianCount: 0, unsuitableMusicianCount: > 0} when (int) starRating >= 8:
                column1.text = review.musicianChoiceFeedback.inappropriateHi;
                break;
            case {synergisticMusicianCount: 0, unsuitableMusicianCount: > 0} when (int) starRating < 8:
                column1.text = review.musicianChoiceFeedback.inappropriateLo;
                break;
            case {synergisticMusicianCount: > 0, unsuitableMusicianCount: > 0} when (int) starRating >= 8:
                column1.text = review.musicianChoiceFeedback.mixedHi;
                break;
            case {synergisticMusicianCount: > 0, unsuitableMusicianCount: > 0} when (int) starRating < 8:
                column1.text = review.musicianChoiceFeedback.mixedLo;
                break;
            default:
                column1.text = review.musicianChoiceFeedback.neither;
                break;
        }
        
        // column two is based on the INSTRUMENTS
        if (performanceAffinity.correctInstrumentCount == data.trackData.numberOfMusiciansToPlay)
        {
            column2.text = review.instrumentChoiceFeedback.perfectVariation;
        }
        else if (performanceAffinity.correctInstrumentCount <= 1 || (performanceAffinity.correctInstrumentCount == 2 && data.trackData.numberOfMusiciansToPlay > 4))
        {
            column2.text = review.instrumentChoiceFeedback.terribleVariation;
        }
        else
        {
            column2.text = review.instrumentChoiceFeedback.averageVariation;
        }
        
        // column three is based on the PERFORMANCE
        if (performanceAffinity.instrumentExpertiseCount == data.trackData.numberOfMusiciansToPlay)
        {
            column3.text = review.affinityFeedback.perfectVariation;
        }
        else if (performanceAffinity.instrumentExpertiseCount <= 1 || (performanceAffinity.instrumentExpertiseCount == 2 && data.trackData.numberOfMusiciansToPlay > 4))
        {
            column3.text = review.affinityFeedback.terribleVariation;
        }
        else
        {
            column3.text = review.affinityFeedback.averageVariation;
        }

        caption.text = review.caption;
        date.text = review.date;
        issueNumber.text = $"Issue #{review.issueNumber}";
        volNumber.text = $"Vol. {review.volNumber}";

        //Set sprite based on reviews
        switch ((int)starRating)
        {
            case (< 4):
                image.sprite = review.reviewImageChoices.terribleVariation;
                break;
            case (> 8):
                image.sprite = review.reviewImageChoices.perfectVariation;
                break;
            default:
                image.sprite = review.reviewImageChoices.averageVariation;
                break;
        }
        
        Debug.Log("Stars " + (int) starRating);
        foreach (var s in stars)
        {
            s.sprite = fullStar;
            s.gameObject.SetActive(false);
        }
        
        
        if ((int) starRating % 2 == 0)
        {
            for (int i = 0; i < (int)starRating / 2; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < Math.Ceiling((float) starRating / 2f); i++)
            {
                stars[i].gameObject.SetActive(true);
            }

            stars[(int) Math.Floor((float) starRating / 2)].sprite = halfStar;
        }
    }
}
