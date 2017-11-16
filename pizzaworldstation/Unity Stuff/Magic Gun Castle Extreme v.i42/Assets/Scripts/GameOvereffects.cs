using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Timers;

public class GameOvereffects : MonoBehaviour 
{
	private Color color;
	private Color defaultC;
	private float timeStamp;
	private float timeTriggered;
	private bool secondT;

	void Start () 
	{
		color = GetComponent<Image> ().color;
		defaultC = color;
		Random.seed = 10;
		timeStamp = Time.time + 2;
		secondT = false;
	}

	// Update is called once per frame
	void Update () 
	{
		//color = new Color (0, 0, 0);

		if (timeStamp < Time.time) 
		{
			color = new Color(255,255,255,255);
			GetComponent<Image> ().color = color;
			timeTriggered = Time.time + 1;
			timeStamp = Time.time + Random.Range(0, 10) + 4;

			secondStrike ();

			//Put trigger here for Thunder sound
		} 
		else 
		{
			color = defaultC;
			GetComponent<Image> ().color = color;
		}

		if ((timeTriggered < Time.time) && secondT)
		{
			color = new Color(255,255,255,255);
			GetComponent<Image> ().color = color;

			secondT = false;

			//Put trigger here for Thunder sound
		} 
		/*else 
		{
			color = defaultC;
			GetComponent<Image> ().color = color;
		}*/
	}

	private void secondStrike()
	{
		if (Random.value > 5) 
		{
			secondT = true;
		}
	}
}
