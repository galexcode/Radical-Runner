using UnityEngine;
using System.Collections;

/*
 * Class controls the text indicating what level the player is on
 */ 

public class LevelDisplay : MonoBehaviour 
{
	private string theLevelText;
	private exSpriteFont theSpriteFont;
	private bool textIsSet;
	
	void Start () 
	{
		theSpriteFont = GetComponent<exSpriteFont>();
	}
	
	void Update () 
	{
		if(!textIsSet)
		{
			
			theSpriteFont.text = theLevelText;
			textIsSet = true;
		}
	}
	
	public void setTheLevelText(int s)
	{
	
		textIsSet = false;
		if(s == 0)
		{
			theLevelText = "";
		}
		else
		{
			theLevelText = "Level: " + s.ToString();
		}
	}
	
}
