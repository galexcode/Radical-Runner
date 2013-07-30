using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

/*
 * Main Screen touch interaction
 */

public class SpawnOnTouch : MonoBehaviour 
{
	public GameObject spawnerObject, curObject;
	private bool pressedButton;
	private PauseButton pause;
	
	private Transform myTransform;
	
	void Start()
	{
		pause = GameObject.FindGameObjectWithTag("PauseButton").GetComponent<PauseButton>();	
	}
	
	void OnFingerDown()
	{
		if(pause.getPaused() == false)
		{
			myTransform = transform;
			curObject = Instantiate(spawnerObject) as GameObject;
			curObject.transform.position = new Vector3(myTransform.position.x, myTransform.position.y, myTransform.position.z - 1.0f);
			
			pressedButton = true;
		}
	}
	
	void OnFingerUp()
	{
		if(curObject != null)
		{
			Destroy(curObject);	
		}
		
		if(pause.getPaused() == false)
		{
		
			if(tag == "gameCenter" && pressedButton)
			{
				if(GameCenterScore.getAuthenticated() == true)
				{
					#if UNITY_IPHONE && !UNITY_EDITOR
						Social.ShowLeaderboardUI();
					#endif
				}	
			}
			
			if(tag == "gameCenterAchieve" && pressedButton)
			{
				#if UNITY_IPHONE && !UNITY_EDITOR
					Social.ShowAchievementsUI();		
				#endif
			}
			
			if(tag == "playButton"&& pressedButton)
			{
				Application.LoadLevel(1);	
			}
			
			if(tag == "credits"&& pressedButton)
			{
				Application.LoadLevel(2);	
			}
			
			if(tag == "gameCenterReset"&& pressedButton)
			{
				#if UNITY_IPHONE && !UNITY_EDITOR
					GameCenterScore.ResetAllAchievements();
				#endif
			}
			
			pressedButton = false;
		}
	}
	
}
