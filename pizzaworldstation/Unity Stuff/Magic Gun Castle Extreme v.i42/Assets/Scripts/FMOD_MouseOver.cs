using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FMOD_MouseOver : MonoBehaviour, IPointerEnterHandler {

    [FMODUnity.EventRef]
    private string menuClick = "event:/HC_Menu_Click";

    public void OnPointerEnter(PointerEventData eventData) {
        FMODUnity.RuntimeManager.PlayOneShot(menuClick);
    }
}