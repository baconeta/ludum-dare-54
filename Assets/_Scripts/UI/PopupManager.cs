using UnityEngine;
using UnityEngine.EventSystems;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private PopupPair[] popups;
    [SerializeField] private bool PopupsEnabled = true;

    // Start is called before the first frame update
    public void Start()
    {
        HideAll();
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
     * Make all popups disappear and be unable to be triggered.
     */
    public void DisableAll()
    {
        PopupsEnabled = false;
        foreach (PopupPair pair in popups)
        {
            pair.HoverTrigger.GetComponent<HoverListenerForPopup>().enabled = true;
            pair.Popup.SetActive(false);
        }
    }

    /*
     * Allow popups to be triggered again.
     */
    public void EnableAll()
    {
        PopupsEnabled = true;
        foreach (PopupPair pair in popups)
        {
            pair.HoverTrigger.GetComponent<HoverListenerForPopup>().enabled = true;
        }
    }

    /*
     * This will be added to HoverTrigger objects PROGRAMMATICALLY. Do NOT add via the Unity Editor.
     */
    public class HoverListenerForPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private GameObject popup = null;
        public new bool enabled = false;

        public void SetPopup(GameObject p) { popup = p; enabled = true; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enabled && popup != null)
            {
                popup.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (popup != null)
            {
                popup.SetActive(false);
            }
        }
    }

    public struct PopupPair
    {
        public GameObject Popup { get; private set; }
        public GameObject HoverTrigger { get; private set; }
        public PopupPair(GameObject popupPrefab, GameObject hoverTrigger) {
            Popup = popupPrefab;
            HoverTrigger = hoverTrigger;
            HoverTrigger.AddComponent<HoverListenerForPopup>();
            HoverTrigger.GetComponent<HoverListenerForPopup>().SetPopup(Popup);
        }
    }
}
