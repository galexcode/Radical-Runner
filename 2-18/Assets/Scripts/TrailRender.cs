using UnityEngine;
using System.Collections;

/*
 * Class controls the particle effect behind the player
 * No longer used
 */ 
public class TrailRender : MonoBehaviour 
{
	private RunnerScript runner;
	private Vector3 p1, p2;
	private bool allowTrailRender;
	private RunnerScoring score;
	
	void Start () 
	{
		runner = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerScript> ();
		score = GameObject.FindGameObjectWithTag("Score").GetComponent<RunnerScoring> ();
		transform.position = new Vector3 (runner.transform.position.x, runner.transform.position.y, runner.transform.position.z);
		allowTrailRender = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (score.getScoreMultiplier() > 1)
		{
			allowTrailRender = true;
		}
		else
		{
			allowTrailRender = false;
		}
		

		if (allowTrailRender)
		{
			if (runner.getGravVal() < 0 && runner.getState() != RunnerScript.State.onGround && tag == "Trlup")
			{
				transform.position = new Vector3(runner.collider.bounds.center.x, runner.collider.bounds.center.y, transform.position.z);
				particleSystem.enableEmission = true;
			}
			else if (runner.getGravVal() > 0 && runner.getState() != RunnerScript.State.onGround && tag == "Trldn")
			{
				transform.position = new Vector3(runner.collider.bounds.center.x, runner.collider.bounds.center.y, transform.position.z);
				particleSystem.enableEmission = true;
			}
			else if (runner.getState() == RunnerScript.State.onGround && tag == "Trlextra")
			{
				transform.position = new Vector3(runner.collider.bounds.center.x, runner.collider.bounds.center.y, transform.position.z);
				particleSystem.enableEmission = true;
			}
			else
				particleSystem.enableEmission = false;
				
		}
		else
			particleSystem.enableEmission = false;
		
		

	}
	
	public void setAllowTrailRender(bool b)
	{
		allowTrailRender = b;	
	}
}
