using UnityEngine;
using System.Collections;

public class VolumeControl : MonoBehaviour {

	private exSpriteFont spriteFont;
	private int volume, volumeFX;
	// Use this for initialization
	void Start () {
		
		spriteFont = GetComponent<exSpriteFont>();
		if(PlayerPrefs.HasKey("volume"))
		{
			if(tag == "VolumeText")
			spriteFont.text = (PlayerPrefs.GetInt("volume") * 10).ToString();
			volume = PlayerPrefs.GetInt("volume");
		}
		if(PlayerPrefs.HasKey("volumeFX"))
		{
			if(tag == "SFXText")
			spriteFont.text = (PlayerPrefs.GetInt("volumeFX")*10).ToString();
			volumeFX = PlayerPrefs.GetInt("volumeFX");
		}
	}
	
	public void setVolume(int f, string tag)
	{
		if(tag.Contains("SFX"))
		{
			if(f > -1 && f < 11)
			{
				volumeFX = f;
//				Debug.Log("SFXVolume: " + volumeFX);
				AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];	
				foreach(AudioSource source in audios)
				{
					if(source.gameObject.tag != "MainCamera")
					source.volume = (float)volumeFX/10;	
				}
				string volumeText = (volumeFX * 10).ToString();
				spriteFont.text = volumeText;
				audio.Play();
				PlayerPrefs.SetInt("volumeFX", volumeFX);
				PlayerPrefs.Save();
			}
		}
		else
		{
			if(f > -1 && f < 11)
			{
				volume = f;
//				Debug.Log("Music Volume: " + volume);
				AudioSource[] audio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];	
				foreach(AudioSource source in audio)
				{
					if(source.gameObject.tag == "MainCamera")
					source.volume = (float)volume/10;	
				}
				string volumeText = (volume * 10).ToString();
				spriteFont.text = volumeText;
				PlayerPrefs.SetInt("volume", volume);
				PlayerPrefs.Save();
			}
		}
	}
	
	public void mute(string tag)
	{
		setVolume(0, tag);
	}
	
	public int getVolume(string tag)
	{
		if(tag.Contains("SFX"))
		{
			return volumeFX;	
		}
		else
		{
			return volume;
		}
	}
	
	
	
}
