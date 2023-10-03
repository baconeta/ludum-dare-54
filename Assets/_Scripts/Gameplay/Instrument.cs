using _Scripts.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Instrument : MonoBehaviour
{
    public InstrumentDataSO data;

    public Image cardSprite;
    public InstrumentType instrumentType;
    public InstrumentPointer worldObject;

    [SerializeField] private GameObject namePopup;

    private void Awake()
    {
        worldObject = GetComponentInChildren<InstrumentPointer>();
        worldObject.parentInstrument = this;
    }
    public Instrument GenerateInstrument()
    {
        // Randomly select an instrument.
        instrumentType = (InstrumentType)Random.Range(0, (int) Enum.GetNames(typeof(InstrumentType)).Length);
        return this;
    }

    public void SetInstrumentData(InstrumentDataSO instrumentData)
    {
        data = instrumentData;
        instrumentType = data.instrumentType;
        GetComponent<Image>().sprite = data.uiSprite;
        SpriteRenderer worldSprite = worldObject?.GetComponent<SpriteRenderer>();
        if (worldSprite is not null)
        {
            worldSprite.sprite = data.inGameSprite;
        }

        PopupManager popupManager = FindObjectOfType<PopupManager>();
        popupManager.AddPressPopup(new PopupManager.PopupPair(namePopup, gameObject), false);
        namePopup.GetComponentInChildren<TMP_Text>().text = data.instrumentName;
        GetComponent<PopupManager.PressListenerForPopup>().SetCallBack(HideInstrumentNameTimer);
    }

    public void HideInstrumentNameTimer()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1.5f);

        namePopup.GetComponent<PopupManager.PopupStatus>().state = 0;
    }
}

public enum InstrumentType
{
    Violin, ElectricGuitar, BassGuitar, Trumpet, Piano, Xylophone, Saxophone, Triangle, Cymbals, Drums, Tambourine, Organ, Synth, Flute, Bagpipes, Cello, Maracas
}
