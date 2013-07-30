using UnityEngine;
using System.Collections;

/* 
 * Class controls titlescreen elements
 */

public class TitleScreenControl : MonoBehaviour 
{
	public GameObject settingsScreen;

	private Ray ray;
	private RaycastHit rayCastHit;
	private bool onSettingsScreen;
	
	void Awake()
	{
		GameCenterScore.Authenticate();
		
		//add to audio class
		if(PlayerPrefs.HasKey("volume"))
		{
			AudioSource[] audio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];	
			foreach(AudioSource source in audio)
			{
				source.volume = (float)PlayerPrefs.GetInt("volume")/10;	
			}	
		}
		setOnSettingsScreen(false);
	}
	
	public void setOnSettingsScreen(bool f)
	{
		onSettingsScreen = f;	
	}
	
	void Update () 
	{
		if(!onSettingsScreen)
		{
			if (Input.touchCount > 0)
			{
				ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				
				if (Physics.Raycast(ray, out rayCastHit))
				{
					
					if(rayCastHit.collider.tag == "SettingsButton")
					{
						Instantiate(settingsScreen);
						setOnSettingsScreen(true);
					}
				}
			}
		}
	}
}
