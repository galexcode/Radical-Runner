using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {
	
	private exSpriteFont spriteFont;
	
	private int volume, volumeFX;
	// Use this for initialization
	void Start () {
		
		spriteFont = GetComponent<exSpriteFont>();
		if(tag == "VolumeText")
		{
			if(PlayerPrefs.HasKey("volume"))
			{
				if(PlayerPrefs.GetInt("volume") > 0)
				{
					spriteFont.text = "On";
					volume = 5;
					PlayerPrefs.SetInt("volume", 5);
				}
				else
				{
					spriteFont.text = "Off";
					volume = 0;
					PlayerPrefs.SetInt("volume", 0);
				}
			}
			else
			{
				spriteFont.text = "On";
				volume = 5;
				PlayerPrefs.SetInt("volume", 5);
			}
		}
		
		if(tag == "SFXText")
		{
			if(PlayerPrefs.HasKey("volumeFX"))
			{
				if(PlayerPrefs.GetInt("volumeFX") > 0)
				{
					spriteFont.text = "On";
					volumeFX = 10;
					PlayerPrefs.SetInt("volumeFX", 10);
				}
				else
				{
					spriteFont.text = "Off";
					volumeFX = 0;
					PlayerPrefs.SetInt("volumeFX", 0);
				}
			}
			else
			{
				spriteFont.text = "On";
				volumeFX = 10;
				PlayerPrefs.SetInt("volumeFX", 10);
			}
		}
		
		PlayerPrefs.Save();
	}
	
	void OnFingerDown()
	{
		if(tag == "VolumeText")
		{
			if(PlayerPrefs.HasKey("volume"))
			{
				if(PlayerPrefs.GetInt("volume") > 0)
				{
					spriteFont.text = "Off";
					volume = 0;
					PlayerPrefs.SetInt("volume", 0);
				}
				else
				{
					spriteFont.text = "On";
					volume = 10;
					PlayerPrefs.SetInt("volume", 10);
				}
			}
			else
			{
				spriteFont.text = "On";
				volume = 10;
				PlayerPrefs.SetInt("volume", 10);
			}
			
			AudioSource[] audio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];	
			foreach(AudioSource source in audio)
			{
				if(source.gameObject.tag == "MainCamera")
				source.volume = (float)volume/10;	
			}
		}
		
		if(tag == "SFXText")
		{
			if(PlayerPrefs.HasKey("volumeFX"))
			{
				if(PlayerPrefs.GetInt("volumeFX") > 0)
				{
					spriteFont.text = "Off";
					volume = 0;
					PlayerPrefs.SetInt("volumeFX", 0);
					
				}
				else
				{
					spriteFont.text = "On";
					volume = 10;
					PlayerPrefs.SetInt("volumeFX", 10);
				}
			}
			else
			{
				spriteFont.text = "On";
				volume = 10;
				PlayerPrefs.SetInt("volumeFX", 10);
			}
			
			AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];	
			foreach(AudioSource source in audios)
			{
				if(source.gameObject.tag != "MainCamera")
				source.volume = (float)volumeFX/10;	
			}
			
		}
		
		PlayerPrefs.Save();
	}
}
