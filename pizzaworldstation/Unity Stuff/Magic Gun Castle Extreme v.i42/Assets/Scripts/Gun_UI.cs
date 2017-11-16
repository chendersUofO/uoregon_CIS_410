using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gun_UI : MonoBehaviour {

	private int ammo;
	private bool isAWizzard;

	public Text ammoCount;

	// Use this for initialization
	void Start () 
	{
		if (ModularWorldGenerator.playerChoice == 2)    //transform.name == "Mage(Clone)") 
		{
			isAWizzard = true;
		} 
		else if (ModularWorldGenerator.playerChoice == 1)  //transform.name == "Gunner(Clone)") 
		{
			isAWizzard = false;
		}

		ammo = 0;
		SetAmmo ();
	}

	// Update is called once per frame
	void Update () 
	{
		/*if (PlayerController.isDefault && isAWizzard) 
		{
			gameObject.SetActive (false);
		}*/

		ammo = PlayerController.ammo;

		SetAmmo ();
	}

	void SetAmmo()
	{
		if (ammo == 0 && isAWizzard == false) 
		{
			ammoCount.text = "INF";
		} 
		else 
		{
			ammoCount.text = ammo.ToString ();
		}
	}
}
