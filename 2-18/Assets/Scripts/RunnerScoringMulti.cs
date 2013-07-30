using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.GameCenter;

/*
 * Class controls amount of each score
 */ 
public class RunnerScoringMulti : MonoBehaviour 
{
	//Scoring number
	private int gateScoreMulti;
 
	private exSpriteFont spriteFont;
	private RunnerScript runner;
	private Renderer myRenderer;

	
	void Start ()    
	{
		spriteFont = GetComponent<exSpriteFont>();
		gateScoreMulti = 0;
		myRenderer = renderer;
		
		myRenderer.enabled = false;	
	}
	
	void FixedUpdate () 
	{	
		
		if (gateScoreMulti >= 1)
		{
			if (spriteFont.botColor != Color.green && spriteFont.topColor != Color.green)
			{
				spriteFont.topColor = Color.green;
				spriteFont.botColor = Color.green;		
			}
			spriteFont.text = " +" + gateScoreMulti.ToString();
		}
		else if (gateScoreMulti < 1)
		{
			spriteFont.text = " X";
			spriteFont.topColor = Color.red;
			spriteFont.botColor = Color.red;
		}
		

	}
	
	public IEnumerator Animate()
	{		
		myRenderer.enabled = true;
		yield return new WaitForSeconds(0.0f);
	}
	
	public int getGateScoreMulti()
	{
		return gateScoreMulti;
	}
	
	public void setGateScoreMulti(int i)
	{
		gateScoreMulti = i;
	}
}
