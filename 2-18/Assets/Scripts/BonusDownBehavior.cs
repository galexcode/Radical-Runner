using UnityEngine;
using System.Collections;

/* 
 * Class controls the "X" sprite that is shown when player collides with a gate
 * Applied to the "BonusDown" game object, never destroyed or instanciated.
 */ 

public class BonusDownBehavior : MonoBehaviour 
{
	private Transform myTransform;
	private Vector3 bonusStartPosition;
	private RunnerScript runner;
	private bool allowMove;
	private float speedFromSpawner;

	void Start () 
	{
		runner = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerScript> ();
		speedFromSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>().getSpeedVar();
		myTransform = transform;
		bonusStartPosition = new Vector3 (runner.collider.bounds.min.x, runner.transform.position.y, runner.transform.position.z);
		myTransform.position = bonusStartPosition;
		allowMove = true;
		
		//Hide on start
		renderer.enabled = false;
	}
	
	void FixedUpdate () 
	{
		if (allowMove)
			myTransform.Translate(new Vector3(-(speedFromSpawner), 0.0f, 0.0f)  * Time.deltaTime);
	}
	
	//Receive location to appear from runnerCollision.cs
	public IEnumerator DisplayBonusDownSprite(Vector3 collisionLocation)
	{
		myTransform.position = collisionLocation;
		
		//Sprite on
		renderer.enabled = true;
		allowMove = true;
		
		yield return new WaitForSeconds(0.5f);
		
		//Sprite off
		renderer.enabled = false;
		allowMove = false;
	}
			
	public void setSpeedFromSpawner(float speed)
	{
		speedFromSpawner = speed;	
	}	
	
}
