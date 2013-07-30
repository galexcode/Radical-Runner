using UnityEngine;
using System.Collections;

/*
 *  Class controls the pause/settings menu
 *  Attached to gameobject: PauseMenu
 * 	Class instantiated/excecuted after hitting the pause button
 * 	Displays the pause screen
 * 	Destroyed on resume
 */ 

public class PauseMenu : MonoBehaviour 
{
	//Assign menus in Inspector
	public GameObject settingsScreen;
	public GameObject menuPointer;
	public GameObject PauseText;
	
	private PauseButton pause;
	private float savedTimeScale;
	private TapDetector recognizer;
	
	
	void Start () 
	{
		pause = GameObject.FindGameObjectWithTag("PauseButton").GetComponent<PauseButton>();
		recognizer = GameObject.FindGameObjectWithTag("Recognizer").GetComponent<TapDetector>();
		pause.renderer.enabled = false;
		menuPointer.renderer.enabled = false;
		//Save previous timeScale
		if(PlayerPrefs.HasKey("timeScale"))
		{
			savedTimeScale = PlayerPrefs.GetFloat("timeScale");	
		}
		
	}
	
	void OnFingerDown()
	{
		RaycastHit hit;
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(camRay, out hit, 11))
		{
			if(hit.collider.tag == "ResumeButton")
			{
				Time.timeScale = savedTimeScale;
				if(Application.loadedLevel == 1)
					pause.renderer.enabled = true;
				pause.setPaused(false);
				recognizer.setOnSettingsScreen(false);
				clearMenu();
				Destroy(GameObject.FindGameObjectWithTag("PauseMenu"));
			}
			
			if(hit.collider.tag == "InstructionsButton" && GameObject.FindGameObjectWithTag("InstructionsSceneMain") == null)
			{
				PauseText.renderer.enabled = false;
				menuPointer.renderer.enabled = true;
				menuPointer.transform.position = new Vector3(-225, menuPointer.transform.position.y, menuPointer.transform.position.z);
				clearMenu(settingsScreen);
				Instantiate(settingsScreen);
				
			}
			
			if(hit.collider.tag == "ButtonPositionButton" && GameObject.FindGameObjectWithTag("ButtonPositionScreen") == null)
			{
				PauseText.renderer.enabled = false;
				menuPointer.renderer.enabled = true;
				menuPointer.transform.position = new Vector3(155, menuPointer.transform.position.y, menuPointer.transform.position.z);
				clearMenu(settingsScreen);
				Instantiate(settingsScreen);
			}
			
			if(hit.collider.tag == "SettingsButton" && GameObject.FindGameObjectWithTag("SettingsScreen") == null)
			{
				PauseText.renderer.enabled = false;
				menuPointer.renderer.enabled = true;
				menuPointer.transform.position = new Vector3(-467, menuPointer.transform.position.y, menuPointer.transform.position.z);
				clearMenu(settingsScreen);
				Instantiate(settingsScreen);
			}
			
			if(hit.collider.tag == "MenuButton")
			{
				//TODO- Save game settings? (position, score, bonuses)
				clearMenu();
				Time.timeScale = savedTimeScale;
				Application.LoadLevel(0);
			}
		}	
	}
	
	//Clear all menu screens
	private void clearMenu()
	{
		if (GameObject.FindGameObjectWithTag("ButtonPositionScreen") != null)
		{
			Destroy(GameObject.FindGameObjectWithTag("ButtonPositionScreen"));
		}
		
		if (GameObject.FindGameObjectWithTag("SettingsScreen") != null)
		{
			Destroy(GameObject.FindGameObjectWithTag("SettingsScreen"));
		}
	
		if (GameObject.FindGameObjectWithTag("InstructionsSceneMain") != null)
		{
			Destroy(GameObject.FindGameObjectWithTag("InstructionsSceneMain"));
		}
		
	}
	
	//Clear non-current menu screens
	private void clearMenu(GameObject objectToKeep)
	{
		if (objectToKeep.tag == "InstructionsSceneMain")
		{
			if (GameObject.FindGameObjectWithTag("ButtonPositionScreen") != null)
			{
				Destroy(GameObject.FindGameObjectWithTag("ButtonPositionScreen"));
			}
			
			if (GameObject.FindGameObjectWithTag("SettingsScreen") != null)
			{
				Destroy(GameObject.FindGameObjectWithTag("SettingsScreen"));
			}
		}
		
		else if (objectToKeep.tag == "ButtonPositionScreen")
		{
			if (GameObject.FindGameObjectWithTag("InstructionsSceneMain") != null)
			{
				Destroy(GameObject.FindGameObjectWithTag("InstructionsSceneMain"));
			}
			
			if (GameObject.FindGameObjectWithTag("SettingsScreen") != null)
			{
				Destroy(GameObject.FindGameObjectWithTag("SettingsScreen"));
			}
		}
		
		else if (objectToKeep.tag == "SettingsScreen")
		{			
			if (GameObject.FindGameObjectWithTag("ButtonPositionScreen") != null)
			{
				Destroy(GameObject.FindGameObjectWithTag("ButtonPositionScreen"));
			}
			
			if (GameObject.FindGameObjectWithTag("InstructionsSceneMain") != null)
			{
				Destroy(GameObject.FindGameObjectWithTag("InstructionsSceneMain"));
			}
		}
			
		

	}
	
	public void setSavedTimeScale(float s)
	{
		savedTimeScale = s;	
	}
	
}
