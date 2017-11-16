using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseButton : MonoBehaviour 
{
	GameObject[] pauseObjects;

	// Use this for initialization
	void Start () 
	{
		Time.timeScale = 1;
		pauseObjects = GameObject.FindGameObjectsWithTag("UIPause");
		hidePaused();
	}

	// Update is called once per frame
	void Update () {

		//uses the p button to pause and unpause the game
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(Time.timeScale == 1)
			{
				Time.timeScale = 0;
				showPaused();
			} else if (Time.timeScale == 0){
				Debug.Log ("high");
				Time.timeScale = 1;
				hidePaused();
			}
		}
	}

/*
	//Reloads the Level
	public void Reload()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	//controls the pausing of the scene
	void pauseControl()
	{
		if(Time.timeScale == 1)
		{
			Time.timeScale = 0;
			showPaused();
		} else if (Time.timeScale == 0)
		{
			Time.timeScale = 1;
			hidePaused();
		}
	}*/

	//shows objects with ShowOnPause tag
	void showPaused()
	{
		foreach(GameObject g in pauseObjects)
		{
			g.SetActive(true);
		}
	}

	//hides objects with ShowOnPause tag
	void hidePaused()
	{
		foreach(GameObject g in pauseObjects)
		{
			g.SetActive(false);
		}
	}

//	//loads inputted level
//	public void LoadLevel(string level){
//		Application.LoadLevel(level);
//	}





	/*
	//GameObject pauseScreen;
	bool isPaused = false;
	public GameObject PauseGui;

	// Use this for initialization
	void Awake () 
	{
		PauseGui.SetActive (isPaused);
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.P))//tougle for paused 
		{ 
			isPaused = !isPaused;
			//gameObject.SetActive(isPaused);
		}
		
		if (isPaused = true) 
		{ //Changing the game time speed for the pause
			Time.timeScale = 0;
			PauseGui.SetActive (true);
		} 
		else 
		{
			Time.timeScale = 1;
			PauseGui.SetActive (false);
		}
	}*/
}