using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/**
 * When the trigger element is hovered-over, the popup will be shown.
 * 
 * If the cursor moves to the popup before leaving the trigger element, the popup will stay up. Leaving both the popup and the trigger element will close the popup.
 * 
 * There are also programmatic hooks for some actions such as hiding all popups, or enabling/disabling the popup system.
 * 
 * Popups are hidden automatically when the popup manager is loaded, and when the popup system is disabled.
 */

public class PopupManager : MonoBehaviour
{
    [SerializeField] private List<PopupPair> hoverPopups;
    [SerializeField] private List<PopupPair> pressPopups;
    [SerializeField] private bool startEnabled = true;

    void OnEnable()
    {
        MusicianManager.OnMusiciansGenerated += SetPopups;
    }
    
    void OnDisable()
    {
        MusicianManager.OnMusiciansGenerated -= SetPopups;
    }
    
    // Start is called before the first frame update
    public void Start()
    {
        // Hide all popups.
        HideAll();
        // Add hover listeners.
        foreach (PopupPair pair in hoverPopups)
        {
            pair.PopupTriggerer.AddComponent<HoverListenerForPopup>().SetPopup(pair.Popup).SetEnabled(startEnabled);
            pair.Popup.AddComponent<HoverListenerForPopup>().SetPopup(pair.Popup).SetEnabled(startEnabled);
            pair.Popup.AddComponent<PopupStatus>();
        }
        // Add press listeners.
        foreach (PopupPair pair in pressPopups)
        {
            pair.PopupTriggerer.AddComponent<PressListenerForPopup>().SetPopup(pair.Popup).SetEnabled(startEnabled);
            pair.Popup.AddComponent<PressListenerForPopup>().SetPopup(pair.Popup).SetEnabled(startEnabled);
            pair.Popup.AddComponent<PopupStatus>();
        }
    }

    void SetPopups(List<Musician> musicianList)
    {
        foreach (Musician musician in musicianList)
        {
            PopupPair popup = new PopupPair(musician.transform.GetChild(0).gameObject,musician.gameObject );
            hoverPopups.Add(popup);
            popup.PopupTriggerer.AddComponent<HoverListenerForPopup>().SetPopup(popup.Popup).SetEnabled(startEnabled);
            popup.Popup.AddComponent<HoverListenerForPopup>().SetPopup(popup.Popup).SetEnabled(startEnabled);
            popup.Popup.AddComponent<PopupStatus>();
        }
    }

    /*
     * Make all popups disappear. Popups can appear again if they are triggered again.
     */
    public void HideAll()
    {
        foreach (PopupPair pair in hoverPopups)
        {
            pair.Popup.SetActive(false);
        }
    }

    /*
     * Allow popups to be triggered again.
     */
    public void EnableAll()
    {
        foreach (PopupPair pair in hoverPopups)
        {
            pair.PopupTriggerer.GetComponent<HoverListenerForPopup>().SetEnabled(true);
            pair.Popup.GetComponent<HoverListenerForPopup>().SetEnabled(true);
        }
    }

    /*
     * Make all popups disappear and be unable to be triggered.
     */
    public void DisableAll()
    {
        foreach (PopupPair pair in hoverPopups)
        {
            pair.PopupTriggerer.GetComponent<HoverListenerForPopup>().SetEnabled(false);
            pair.Popup.GetComponent<HoverListenerForPopup>().SetEnabled(false);
            pair.Popup.SetActive(false);
        }
    }

    [System.Serializable]
    public struct PopupPair
    {
        [Tooltip("The GameObject that will appear as a popup.")]
        [SerializeField] public GameObject Popup;
        [Tooltip("The GameObject that will trigger the popup when hovered over.")]
        [SerializeField] public GameObject PopupTriggerer;

        public PopupPair(GameObject popupPrefab, GameObject triggerer)
        {
            Popup = popupPrefab;
            PopupTriggerer = triggerer;
        }
    }

    /*
     * This will be added to HoverTrigger objects PROGRAMMATICALLY.
     * Do NOT add via the Unity Editor.
     */
    public class HoverListenerForPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private GameObject popup = null;
        private new bool enabled = false;

        public HoverListenerForPopup SetPopup(GameObject p) {
            popup = p;
            enabled = true;
            return this;
        }

        public HoverListenerForPopup SetEnabled(bool state)
        {
            enabled = state;
            return this;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enabled && popup != null)
            {
                popup.SetActive(true);
                popup.GetComponent<PopupStatus>().state += 1;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (popup != null)
            {
                popup.GetComponent<PopupStatus>().state -= 1;
            }
        }
    }

    /*
     * This will be added to HoverTrigger objects PROGRAMMATICALLY.
     * Do NOT add via the Unity Editor.
     */
    public class PressListenerForPopup : MonoBehaviour, IPointerDownHandler
    {
        private GameObject popup = null;
        private new bool enabled = false;

        public PressListenerForPopup SetPopup(GameObject p)
        {
            popup = p;
            enabled = true;
            return this;
        }

        public PressListenerForPopup SetEnabled(bool state)
        {
            enabled = state;
            return this;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (enabled && popup != null)
            {
                popup.SetActive(!popup.activeInHierarchy); // NOTE: Perhaps this should be popup.activeSelf instead?
                popup.GetComponent<PopupStatus>().state += 1;
            }
        }
    }

    public class PopupStatus : MonoBehaviour
    {
        /*
         * state <= 0 to be hidden.
         * state >= 1 to be shown.
         * state == 2 is only possible in the event of race conditions. It should not persist that way for longer than a frame.
         */
        public int state = 0;

        public void LateUpdate()
        {
            if (state == 0)
            {
                gameObject.SetActive(false);
            } else if (state < 0)
            {
                state = 0;
            }
        }
    }
}
