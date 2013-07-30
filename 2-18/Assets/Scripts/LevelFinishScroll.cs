using UnityEngine;
using System.Collections;

public class LevelFinishScroll : MonoBehaviour 
{
	
	private Transform myTransform;
	private bool allowFirstMove, allowSecondMove, clearScreen;
	private float screenEdgeLeft;
	private Vector3 resetPosition;
	private float speed;
	private SpawnObstacles spawner;
	

	void Start () 
	{
		renderer.enabled = false;
		myTransform = transform;
		allowFirstMove = false;
		allowSecondMove = false;
		clearScreen = false;
		screenEdgeLeft = GameObject.FindGameObjectWithTag("Top").GetComponent<Transform>().collider.bounds.min.x;	
		resetPosition = new Vector3(1205.0f, 0.0f, 2.0f);
		spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>();
		speed = spawner.getSpeedVar();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//Move to the center of screen, triggered by coroutine in spawnObstacles
		if (allowFirstMove)
		{
			renderer.enabled = true;
			if (myTransform.position.x >= 0.0f)
			{
				myTransform.Translate(Time.deltaTime * -speed, 0.0f , 0.0f);
				if(!clearScreen)
				{
					if(myTransform.position.x < 400.0f)
					{
						spawner.ClearGameScreen();
						clearScreen = true;
					}
				}
			}	
			else 
			{
				spawner.setCrossedFinish(true);
				allowFirstMove = false;
			}
		}
		
		//Move from center of screen off-screen. Triggered by SwipeDetector
		if (allowSecondMove)
		{
			if (myTransform.collider.bounds.max.x >= screenEdgeLeft)
			{
				myTransform.Translate(Time.deltaTime * -speed * 2.0f, 0.0f, 0.0f);
			}	
			else 
			{
				Reset();
			}
		}
	}
	
	private void Reset()
	{
		allowFirstMove = false;
		allowSecondMove = false;
		renderer.enabled = false;
		clearScreen = false;
		spawner.setCrossedFinish(false);
		spawner.setSpawnedFinish(false);
		myTransform.position = resetPosition;// new Vector3(1205.0f, 0.0f, 2.0f);
	}
	
	public void FirstTransitionMove()
	{
		renderer.enabled = true;
		allowFirstMove = true;
	}
	
	public void SecondTransitionMove()
	{
		allowSecondMove = true;
		speed = spawner.getSpeedVar();
	}
	
	
	
}
