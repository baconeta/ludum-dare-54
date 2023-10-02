using System;
using _Scripts.Gameplay;
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

    public void SetNewspaperUI(ReviewDataSO review, ReviewManager.StarRating stars)
    {
        switch (stars)
        {
            case ReviewManager.StarRating.TBD:
                title.text = review.reviewTitle1;
                break;
            case ReviewManager.StarRating.Bombed:
                title.text = review.reviewTitle1;
                break;
            case ReviewManager.StarRating.Bad:
                title.text = review.reviewTitle1;
                break;
            case ReviewManager.StarRating.Passable:
                title.text = review.reviewTitle2;
                break;
            case ReviewManager.StarRating.Mediocre:
                title.text = review.reviewTitle2;
                break;
            case ReviewManager.StarRating.Good:
                title.text = review.reviewTitle4;
                break;
            case ReviewManager.StarRating.Entertaining:
                title.text = review.reviewTitle3;
                break;
            case ReviewManager.StarRating.Wonderful:
                title.text = review.reviewTitle4;
                break;
            case ReviewManager.StarRating.Excellent:
                title.text = review.reviewTitle4;
                break;
            case ReviewManager.StarRating.Awe_Inspiring:
                title.text = review.reviewTitle5;
                break;
            case ReviewManager.StarRating.Life_Changing:
                title.text = review.reviewTitle5;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stars), stars, null);
        }
        
        subtitle.text = review.reviewSubTitle;
        column1.text = review.musicianChoiceFeedback.ToString();
        column2.text = review.instrumentChoiceFeedback.ToString();
        column3.text = review.affinityFeedback.ToString();
        caption.text = review.caption;
        date.text = review.date;
        issueNumber.text = $"Issue #{review.issueNumber}";
        volNumber.text = $"Vol. {review.volNumber}";
        //TODO Set image
        image.sprite = review.reviewImage;
        //TODO Set stars based on the review score
        //0 = 0 star
        //1 = .5 star
        //2 = 1 star
        //10 = 5 star
        for(int i = 0; i < review.halfStars; i++)
        {
            //Set sprite to stars score
            //stars[i].sprite = starSprites?;
        }
    }
}
