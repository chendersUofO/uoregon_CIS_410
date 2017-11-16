using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FMOD_MouseClick : MonoBehaviour, IPointerClickHandler {
    [FMODUnity.EventRef]
    private string menuClick = "event:/HC_Menu_Click";

    public void OnPointerClick(PointerEventData eventData) {
        FMODUnity.RuntimeManager.PlayOneShot(menuClick);
    }
}