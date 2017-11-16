using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PauseTextScript : MonoBehaviour {
	private int magicA;
	private int weaponA;
	private int health;

	public Text screen;

	// Use this for initialization
	void Start () 
	{
		magicA = 0;
		weaponA = 0;
		health = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		magicA = PlayerController.magicAmmo;
		weaponA = PlayerController.ammo;
		health = PlayerController.playerHealth;

		screen.text = health.ToString() + "\n" + "\n" + weaponA.ToString() + "\n" + "\n" + magicA.ToString();
	}
}
