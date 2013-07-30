using UnityEngine;
using System.Collections;

/*
 * Class controls the pause button, allowing the pause menu to appear
 */ 
public class PauseButton: MonoBehaviour 
{
	public GameObject pauseMenu, curObject;
	private TapDetector recognizer;
	private bool paused;
	
	void Start()
	{
		paused = false;
		recognizer = GameObject.FindGameObjectWithTag("Recognizer").GetComponent<TapDetector>();
	}
	
	void OnApplicationPause(bool pause)
	{
		if(pause)
			pauseGame();	
	}
	
	void OnFingerDown()
	{
		if(Application.loadedLevel == 1)
		pauseGame();
	}
	
	void OnFingerUp()
	{
		if(Application.loadedLevel == 0)
			pauseGame();
	}
	
	void pauseGame()
	{
		if(!paused)
		{
			curObject = Instantiate(pauseMenu) as GameObject;
			recognizer.setOnSettingsScreen(true);
			PlayerPrefs.SetFloat("timeScale", Time.timeScale);
			PlayerPrefs.Save();
			Time.timeScale = 0.0f;
			paused = true;
		}
	}
	
	public bool getPaused()
	{
		return paused;	
	}
	
	public void setPaused(bool s)
	{
		paused = s;	
	}
}
