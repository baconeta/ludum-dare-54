using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    public PopupManager popupManager;
    public GameObject touchObject;
    void Awake()
    {
        popupManager.AddPressPopup(new PopupManager.PopupPair(popupManager.tutorialInfoPopup, touchObject));
    }
}
