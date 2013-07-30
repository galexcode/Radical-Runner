using UnityEngine;
using System.Collections;

//Replaced by ClipBoardText
public class NewLevelText : MonoBehaviour {
	private SpawnObstacles spawner;
	private exSpriteFont spriteFont;
	private string statusText;

	void Start () 
	{
		if(PlayerPrefs.HasKey("volumeFX"))
		{
			if(GetComponent<AudioSource>() != null)
			{
				AudioSource audio = GetComponent<AudioSource>();
				audio.volume = (float)PlayerPrefs.GetInt("volumeFX")/10;
			}
		}
		
		spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>();
		spriteFont = GetComponent<exSpriteFont>();
		statusText = "Level " + spawner.getLevelCount()
						 +"\nGates: " + spawner.getNumGates()
						 +"\nGates Hit: " + "6"
						 +"\nGrade:"+"\n"+"\n"+"\n"
						 +"\nTap to Start"
						 +"\n";
		
		spriteFont.text = statusText;
	}
}
