using UnityEngine;
using System.Collections;

/*
 * Class holds the high score
 */ 
public class DisplayHighScores : MonoBehaviour 
{
	private string score;
	private exSpriteFont spriteFont;
	
	void Start () 
	{
		spriteFont = GetComponent<exSpriteFont>();

		for(int i = 1; i < 11; i++)
		{
			score += string.Format("{0,2:00} - {1,-20:00000000000000000000} - {2,5}", i, Highscores.GetName(1, i -1), Highscores.GetScore(1, i-1)) + "\n";
		}
		
		spriteFont.text = score;
	}
}
