using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GunnerSelect : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData) {

        ModularWorldGenerator.playerChoice = 1;
        cameraController.playerChoice = 1;
        PlayerController.playerHealth = 100;
    }
}