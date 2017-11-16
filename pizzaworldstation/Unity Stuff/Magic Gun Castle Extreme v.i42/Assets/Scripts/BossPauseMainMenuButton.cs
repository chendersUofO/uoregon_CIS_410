using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BossPauseMainMenuButton : MonoBehaviour, IPointerClickHandler {

	public void OnPointerClick(PointerEventData eventData) 
	{
		Application.LoadLevel ("MainLoad");
		//Application.loadedLevel ("Game Over");	
	}
}