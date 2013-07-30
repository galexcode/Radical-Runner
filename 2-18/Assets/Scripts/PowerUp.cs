using UnityEngine;
using System.Collections;

/*
 * Class controls the behavior of the powerups that are used by the player
 */

public class PowerUp : MonoBehaviour 
{
	public string type;
	public bool typeIsSet, onStart, canUsePowerUp;
	public float buttonTimer, buttonTimerLimit;
	
	private RunnerCollision rCollision;
	private RunnerScript runner;
	private PauseButton pause;
	private float blinkTime;
	
	//Hour glass effect
	public GameObject HourGlassEffect0, HourGlassEffect1, HourGlassEffect2, HourGlassEffect3;

	void Start () 
	{
		blinkTime = 0.0f;
		buttonTimer = 20.0f;
		buttonTimerLimit = 20.0f;
		canUsePowerUp = true;
		rCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerCollision>();
		runner = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerScript>();
		pause = GameObject.FindGameObjectWithTag("PauseButton").GetComponent<PauseButton>();
		typeIsSet = false;
		onStart = true;
		type = tag;
		collider.enabled = false;
		renderer.enabled = false;
	}
	
	void Update () 
	{
		if(!onStart)
		{
			buttonTimer += 1.0f * Time.deltaTime;
			if(!canUsePowerUp)
			{
				if(buttonTimer > buttonTimerLimit)
				{
					
					canUsePowerUp = true;
					typeIsSet = false;
				}
			}
			if(!typeIsSet)
			{
				//moved to end
				if(canUsePowerUp)
				{
					typeIsSet = true;
					collider.enabled = true;
					renderer.enabled = true;
				}
			}
			else
			{
				if(rCollision.getRespawn() == true)
				{
					//Destroy (this.gameObject);
				}
			}
		}
		else
		{
			collider.enabled = false;
			renderer.enabled = false;
		}
		
	}
	public void setOnStart(bool s)
	{
		onStart = s;	
	}
	
	public void setType(string s)
	{
		type = s;	
	}
	
	public void setTypeIsSet(bool s)
	{
		typeIsSet = s;	
	}
	
	//Trigger power up behavior
	void OnFingerDown()
	{
		if(pause.getPaused() == false)
		{
			if(canUsePowerUp)
			{
				if(type == "Hrglss")
				{
					//Create hour glass effect
					Instantiate(HourGlassEffect0);
					Instantiate(HourGlassEffect1);
					Instantiate(HourGlassEffect2);
					Instantiate(HourGlassEffect3);
					
					//Slow down time
					rCollision.setTimeScale(0.5f);
					rCollision.setSlowTimeDuration(3.0f);
				}	
				
				if(type == "Intangibility")
				{
					StartCoroutine(fadeTimer());
					rCollision.setIntangible(true);
					rCollision.setIntangibleDuration(4.5f);
					
				}
				
				//added for scaling
				if(type == "shrink")
				{
					
					if (runner.getState() == RunnerScript.State.onGround)
					{
					rCollision.setAllowScoringMultiplier(false);
					rCollision.setSpriteScale(0.25f);
					rCollision.setShrinkDuration(4.0f);	
					}
					
					else 
					{
						rCollision.setSpriteScale(0.25f);
						rCollision.setShrinkDuration(4.0f);	
					}
					
				}
				
				collider.enabled = false;
				renderer.enabled = false;
				buttonTimer = 0.0f;
				canUsePowerUp = false;
			}
		}
	}
	
	//Used for Intangible effect
	public IEnumerator fadeTimer()
	{
		while (blinkTime <= 2.75)
		{
			if (runner.renderer.enabled == true)
			{
				runner.renderer.enabled = false;
			}
			else if (runner.renderer.enabled == false)
			{
				runner.renderer.enabled = true;
			}
			
			blinkTime += 0.25f;
			
			yield return new WaitForSeconds(0.25f);
		}
		
		while (blinkTime <= 3.95)
		{
			if (runner.renderer.enabled == true)
			{
				runner.renderer.enabled = false;
			}
			else if (runner.renderer.enabled == false)
			{
				runner.renderer.enabled = true;
			}
			
			blinkTime += 0.1f;
			
			yield return new WaitForSeconds(0.1f);
		}
		
		blinkTime = 0.0f;
	}
}
