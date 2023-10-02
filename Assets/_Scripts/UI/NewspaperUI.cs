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

    public Sprite halfStar;

    public void SetNewspaperUI(ReviewDataSO review, ReviewManager.StarRating starRating)
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
        column1.text = review.musicianChoiceFeedback.ToString(); // TODO
        column2.text = review.instrumentChoiceFeedback.ToString(); //TODO
        column3.text = review.affinityFeedback.ToString(); //TODO
        caption.text = review.caption;
        date.text = review.date;
        issueNumber.text = $"Issue #{review.issueNumber}";
        volNumber.text = $"Vol. {review.volNumber}";
        //TODO Set image
        image.sprite = review.reviewImage;
        
        Debug.Log("Stars " + (int) starRating);
        foreach (var s in stars)
        {
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
