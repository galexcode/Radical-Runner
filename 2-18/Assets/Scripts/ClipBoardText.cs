using UnityEngine;
using System.Collections;

/*
 * Class is responsible for the behavior of the clipBoard Text that apears at the end of levels
 * Attached to the object ClipBoardText/Nextlevel
 */ 

public class ClipBoardText : MonoBehaviour 
{
	private int speed;
	private Transform myTransform;
	private Vector2 endPosition;
	private SpawnObstacles spawner;
	private exSpriteFont spriteFont;
	private string statusText;
	private RunnerCollision rCollision;
	
	//Display Grade
	private float fPercent;
	private string grade;
	
	
	void Start () 
	{
		//Object Access
		myTransform = transform;
		spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>();
		rCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerCollision>();
		spriteFont = GetComponent<exSpriteFont>();
		
		speed = 1200;
		
		fPercent = 100 - (100 * ( (float)rCollision.getNumGatesHit() / (float)spawner.getNumGatesInLevel() ));
		DisplayGradeLetter();
		//Text to display
		statusText = "Level " + spawner.getLevelCount()
						 +"\nGates: " + spawner.getNumGatesInLevel()
						 +"\nGates Hit: " + rCollision.getNumGatesHit()
						 +"\nGrade:"+"\n"+"\n"+"\n"
						 +"\nTap to Start"
						 +"\n";
		
		spriteFont.text = statusText;
		
		//Add to volume control
		if(PlayerPrefs.HasKey("volumeFX"))
		{
			if(GetComponent<AudioSource>() != null)
			{
				AudioSource audio = GetComponent<AudioSource>();
				audio.volume = (float)PlayerPrefs.GetInt("volumeFX")/10;
			}
		}
	}
	
	private void FixedUpdate () 
	{
		if (myTransform.position.y >= -25.0f)
		{
			myTransform.Translate(0, Time.deltaTime * -speed, 0);
		}
	}
	
	public string getGrade()
	{
		return grade;
	}
	
	private void DisplayGradeLetter () 
	{
		if (rCollision.getNumGatesHit() == 0)
			grade = "A+";
		else if (fPercent >= 95)
			grade = "A";
		else if (fPercent >= 90)
			grade = "A-";
		else if (fPercent >= 85)
			grade = "B+";
		else if (fPercent >= 80)
			grade = "B";
		else if (fPercent >= 75)
			grade = "B-";
		else if (fPercent >= 70)
			grade = "C+";
		else if (fPercent >= 65)
			grade = "C";
		else if (fPercent >= 60)
			grade = "C-";
		else if (fPercent >= 55)
			grade = "D+";
		else if (fPercent >= 50)
			grade = "D";
		else if (fPercent >= 45)
			grade = "D-";
		else
			grade = "F";
						
	}
	

}
