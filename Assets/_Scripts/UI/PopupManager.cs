using UnityEngine;
using UnityEngine.EventSystems;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private PopupPair[] popups;
    [SerializeField] private bool startEnabled = true;

    // Start is called before the first frame update
    public void Start()
    {
        // Hide all popups.
        HideAll();
        // Add hover listeners.
        foreach (PopupPair pair in popups)
        {
            pair.HoverTrigger.AddComponent<HoverListenerForPopup>().SetPopup(pair.Popup).SetEnabled(startEnabled);
            pair.Popup.AddComponent<HoverListenerForPopup>().SetPopup(pair.Popup).SetEnabled(startEnabled);
            pair.Popup.AddComponent<PopupStatus>();
        }
    }

    /*
     * Make all popups disappear. Popups can appear again if they are triggered again.
     */
    public void HideAll()
    {
        foreach (PopupPair pair in popups)
        {
            pair.Popup.SetActive(false);
        }
    }

    /*
     * Allow popups to be triggered again.
     */
    public void EnableAll()
    {
        foreach (PopupPair pair in popups)
        {
            pair.HoverTrigger.GetComponent<HoverListenerForPopup>().SetEnabled(true);
            pair.Popup.GetComponent<HoverListenerForPopup>().SetEnabled(true);
        }
    }

    /*
     * Make all popups disappear and be unable to be triggered.
     */
    public void DisableAll()
    {
        foreach (PopupPair pair in popups)
        {
            pair.HoverTrigger.GetComponent<HoverListenerForPopup>().SetEnabled(false);
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
        [SerializeField] public GameObject HoverTrigger;

        public PopupPair(GameObject popupPrefab, GameObject hoverTrigger)
        {
            Popup = popupPrefab;
            HoverTrigger = hoverTrigger;
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

    public class PopupStatus : MonoBehaviour
    {
        /*
         * 0 = to be hidden.
         * 1+ = to be shown.
         * 2 is only possible in the event of race conditions. it will not persist for longer than a frame.
         */
        public int state = 0;

        public void LateUpdate()
        {
            if (state == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
