using _Scripts.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class MusicianPopup : MonoBehaviour
    {
        [SerializeField] private Image characterPhotoCard;
        [SerializeField] private TMP_Text charName;
        [SerializeField] private TMP_Text charAge;
        [SerializeField] private TMP_Text funFact;
        [SerializeField] private TMP_Text bio;

        public void SetProfileCardInfo(MusicianDataSO data)
        {
            characterPhotoCard.sprite = data.photoCardImage;
            // TODO rest
        }
    }
}