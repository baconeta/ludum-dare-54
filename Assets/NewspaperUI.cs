using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
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

    public void SetNewspaperUI(GameObject review)
    {
        title.text = review.name;
        subtitle.text = review.name;
        column1.text = review.name;
        column2.text = review.name;
        column3.text = review.name;
        caption.text = review.name;
        date.text = review.name;
        issueNumber.text = review.name;
        volNumber.text = review.name;
        image = null;
        //TODO Set stars based on the review score
        bool[] starsTemp = new bool[5];
        for(int i = 0; i < starsTemp.Length; i++)
        {
            //Set sprite to stars score
            //stars[i].sprite = starsTemp[i];
        }
    }
}
