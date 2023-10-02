using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
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

    public void SetNewspaperUI(ReviewDataSO review)
    {
        title.text = review.reviewTitle;
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
