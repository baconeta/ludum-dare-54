using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

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
    [SerializeField] private GameObject fadeVeil;
    [SerializeField] private bool startEnabled = true;

    [SerializeField] public GameObject musicianPopup;
    [SerializeField] public GameObject instrumentPopup;
    [SerializeField] public GameObject performanceInfoPopup;
    [SerializeField] public GameObject tutorialInfoPopup;

    public void Start()
    {
        // The veil is effectively a popup, too.
        fadeVeil.AddComponent<PopupStatus>();
        fadeVeil.AddComponent<PressListenerForPopup>().SetIsVeil().SetPopup(fadeVeil);
        // Set the opacity of the veil to 10%.
        Color c = fadeVeil.GetComponent<UnityEngine.UI.Image>().color;
        c.a = 0.9f;
        fadeVeil.GetComponent<UnityEngine.UI.Image>().color = c;
        // And hide it.
        fadeVeil.SetActive(false);
        
        // Hide all popups.
        HideAll();
        // Add hover listeners.
        foreach (PopupPair pair in hoverPopups)
        {
            RegisterHoverListeners(pair);
        }

        // Add press listeners.
        foreach (PopupPair pair in pressPopups)
        {
            RegisterPressListeners(pair);
        }
    }

    public void AddHoverPopups(List<PopupPair> pairs)
    {
        foreach (PopupPair pair in pairs)
        {
            hoverPopups.Add(RegisterHoverListeners(pair));
        }
    }

    public void AddHoverPopups(PopupPair[] pairs)
    {
        AddHoverPopups(pairs.ToList());
    }

    public void AddHoverPopup(PopupPair pair)
    {
        pressPopups.Add(RegisterHoverListeners(pair));
    }

    public void AddPressPopups(List<PopupPair> pairs)
    {
        foreach (PopupPair pair in pairs)
        {
            pressPopups.Add(RegisterPressListeners(pair));
        }
    }

    public void AddPressPopups(PopupPair[] pairs)
    {
        AddPressPopups(pairs.ToList());
    }

    public void AddPressPopup(PopupPair pair, bool useVeil=true)
    {
        pressPopups.Add(RegisterPressListeners(pair, useVeil));
    }

    private PopupPair RegisterPressListeners(PopupPair pair, bool useVeil=true)
    {
        if (pair.PopupTriggerer.GetComponent<PressListenerForPopup>() is null)
        {
            if (useVeil)
            {
                pair.PopupTriggerer.AddComponent<PressListenerForPopup>().SetPopup(pair.Popup).SetVeil(fadeVeil).SetEnabled(startEnabled);
            }
            else
            {
                pair.PopupTriggerer.AddComponent<PressListenerForPopup>().SetPopup(pair.Popup).SetEnabled(startEnabled);
            }
            
        }
        
        if (pair.Popup.GetComponent<PressListenerForPopup>() is null)
        {
            if (useVeil)
            {
                pair.Popup.AddComponent<PressListenerForPopup>().SetPopup(pair.Popup).SetVeil(fadeVeil).SetEnabled(startEnabled);
            }
            else
            {
                pair.Popup.AddComponent<PressListenerForPopup>().SetPopup(pair.Popup).SetEnabled(startEnabled);
            }
        }

        if (pair.Popup.GetComponent<PopupStatus>() is null)
        {
            pair.Popup.AddComponent<PopupStatus>();
        }

        return pair;
    }

    private PopupPair RegisterHoverListeners(PopupPair pair)
    {
        pair.PopupTriggerer.AddComponent<HoverListenerForPopup>().SetPopup(pair.Popup).SetVeil(fadeVeil).SetEnabled(startEnabled);
        if (pair.Popup.AddComponent<PressListenerForPopup>() is null)
        {
            pair.Popup.AddComponent<HoverListenerForPopup>().SetPopup(pair.Popup).SetVeil(fadeVeil).SetEnabled(startEnabled);
        }
        if (pair.Popup.GetComponent<PopupStatus>() is null)
        {
            pair.Popup.AddComponent<PopupStatus>();
        }

        return pair;
    }

/*
 * Make all popups disappear. Popups can appear again if they are triggered again.
 */
    public void HideAll()
    {
        foreach (PopupPair pair in hoverPopups)
        {
            pair.Popup.SetActive(false);
            pair.Popup.GetComponent<PopupStatus>().state = 0;
        }
        foreach (PopupPair pair in pressPopups)
        {
            pair.Popup.SetActive(false);
            pair.Popup.GetComponent<PopupStatus>().state = 0;
        }

        if (fadeVeil is not null)
        {
            fadeVeil.SetActive(false);
            fadeVeil.GetComponent<PopupStatus>().state = 0;
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
        [Tooltip("The GameObject that will appear as a popup.")] [SerializeField]
        public GameObject Popup;

        [Tooltip("The GameObject that will trigger the popup when hovered over.")] [SerializeField]
        public GameObject PopupTriggerer;

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
    public class BaseListenerForPopup : MonoBehaviour
    {
        protected GameObject popup = null;
        protected GameObject veil = null;
        protected bool canPopup = false;

        public BaseListenerForPopup SetPopup(GameObject popup)
        {
            this.popup = popup;
            return this;
        }

        public BaseListenerForPopup SetVeil(GameObject veil)
        {
            this.veil = veil;
            return this;
        }

        public BaseListenerForPopup SetEnabled(bool canPopup)
        {
            this.canPopup = canPopup;
            return this;
        }
    }

    public class HoverListenerForPopup : BaseListenerForPopup, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (canPopup && popup != null)
            {
                popup.SetActive(true);
                popup.GetComponent<PopupStatus>().state += 1;
                if (veil is not null)
                {
                   veil.SetActive(true);
                   veil.GetComponent<PopupStatus>().state += 1; 
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (popup != null)
            {
                popup.GetComponent<PopupStatus>().state -= 1;
                if (veil is not null)
                {
                    veil.GetComponent<PopupStatus>().state -= 1;
                }
            }
        }
    }

/*
 * This will be added to PressTrigger objects PROGRAMMATICALLY.
 * Do NOT add via the Unity Editor.
 */
    public class PressListenerForPopup : BaseListenerForPopup, IPointerUpHandler, IPointerDownHandler
    {
        private bool _isVeil;
        private Action _callback;
        public void OnPointerUp(PointerEventData eventData)
        {
            if (enabled && canPopup && popup != null)
            {
                FindObjectOfType<PopupManager>().HideAll();
                if (_isVeil)
                {
                    return;
                }
                
                if (veil is not null)
                {
                    veil.GetComponent<PopupStatus>().state = veil.GetComponent<PopupStatus>().state == 1 ? 0 : 1;
                    veil.SetActive(!veil.activeInHierarchy);
                }
                popup.GetComponent<PopupStatus>().state = popup.GetComponent<PopupStatus>().state == 1 ? 0 : 1;
                popup.SetActive(!popup.activeInHierarchy);
                if (popup.gameObject.activeInHierarchy)
                {
                    _callback?.Invoke();
                }
            }
        }

        public void SetCallBack(Action callback)
        {
            _callback = callback;
        }

        public PressListenerForPopup SetIsVeil()
        {
            _isVeil = true;
            return this;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isVeil)
            {
                FindObjectOfType<PopupManager>().HideAll();
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
            }
            else if (state < 0)
            {
                state = 0;
            }
        }
    }
}