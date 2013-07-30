using UnityEngine;
using System.Collections;

public class VolumeButton : MonoBehaviour 
{
	
	private VolumeControl volume;
	public GameObject pauseMenu;
	// Use this for initialization
	void Start () {
		if(tag.Contains("SFX"))
		{
			//Debug.Log("Contains Tag" + tag);
			volume = GameObject.FindGameObjectWithTag("SFXText").GetComponent<VolumeControl>();	
		}
		else
		{
			volume = GameObject.FindGameObjectWithTag("VolumeText").GetComponent<VolumeControl>();	
		}
	}

	void OnFingerDown()
	{
		if(tag == "SFXMinus" || tag == "VolumeMinus")
		{
			volume.setVolume((volume.getVolume(tag) - 1), tag);
		}
		
		if(tag == "SFXPlus" || tag == "VolumePlus")
		{
			volume.setVolume((volume.getVolume(tag) + 1), tag);	
		}
		
		if(tag == "SFXMute" || tag == "VolumeMute")
		{
			volume.mute(tag);	
		}
		
		if(tag.Contains("SFX"))
		{
			PlayerPrefs.SetInt("volumeFX", volume.getVolume("FX"));
		}
		else
		{
			PlayerPrefs.SetInt("volume", volume.getVolume(""));	
		}
		
		PlayerPrefs.Save();
	}
}
