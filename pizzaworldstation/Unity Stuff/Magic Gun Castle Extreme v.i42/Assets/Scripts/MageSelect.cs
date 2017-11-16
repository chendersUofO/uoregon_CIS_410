using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MageSelect : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData) {

        ModularWorldGenerator.playerChoice = 2;
        cameraController.playerChoice = 2;
        PlayerController.playerHealth = 100;
    }
}