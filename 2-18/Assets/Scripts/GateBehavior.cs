using UnityEngine;
using System.Collections;


/* This class handles the behavior of gates as they spawn
 * Behavior includes position, speed sync, and scoring for gate passing
 */
public class GateBehavior : MonoBehaviour
{
	private GameObject runner;
	private RunnerScoring runnerScore;
	private RunnerScoringMulti runnerScoreMulti, runnerScoreMulti2;
	private Transform myTransform;
	private Collider myCollider;
	private float sizeX, sizeY;
	private SpawnObstacles spawner;
	private GameObject topBorder, bottomBorder;
	private float speedVar;
	
	//Alternate moving (verticle) speed variables
	private float speedVarBottomTop, speedVarMovingGate;
	
	//Scoring variables
	private bool allowScoreOnPass;
	private int scoreCount;
	
	void Start ()
	{
		//Cache
		myTransform = transform;
		myCollider = collider;
		runner = GameObject.FindGameObjectWithTag ("Player");
		spawner = GameObject.FindGameObjectWithTag ("Spawner").GetComponent<SpawnObstacles> ();
		topBorder = GameObject.FindGameObjectWithTag("Top");
		bottomBorder = GameObject.FindGameObjectWithTag("Bottom");
		runnerScore = GameObject.FindGameObjectWithTag ("Score").GetComponent<RunnerScoring>();
		runnerScoreMulti = GameObject.FindGameObjectWithTag ("ScoreMulti").GetComponent<RunnerScoringMulti>();
		
		//Positioning initialization
		if(gameObject.CompareTag("BottomGate"))
		{
			speedVarBottomTop = speedVar * Random.Range(0.2f, 0.4f);
			myTransform.position = new Vector3 (590.0f, bottomBorder.collider.bounds.max.y, 0.0f);	
		}
		
		if(gameObject.CompareTag("TopGate"))
		{
			speedVarBottomTop = speedVar * Random.Range(0.2f, 0.4f);
			myTransform.position = new Vector3 (590.0f, topBorder.collider.bounds.min.y, 0.0f);	
		}
		
		if(gameObject.CompareTag("CenterGate"))
		{
			myTransform.position = new Vector3 (590.0f, 0.0f, 0.0f);	
		}
		
		if(gameObject.CompareTag("regGate"))
		{
			myTransform.position = new Vector3 (590.0f, topBorder.collider.bounds.min.y - myCollider.bounds.extents.y, 0.0f);	
		}
		
		if(gameObject.CompareTag("MovingGate"))
		{
			myTransform.position = new Vector3 (590.0f, myTransform.position.y, 0.0f);	
			myTransform.localScale = new Vector3(myTransform.localScale.x, Random.Range(100, 250), myTransform.localScale.z);
			speedVarMovingGate = speedVar * Random.Range(0.5f, 0.7f);
			if (Random.Range (0, 2) == 1 )
			{
				speedVarMovingGate *= -1;
			}
		}	
		
		//Scoring initialization
		allowScoreOnPass = true;
	}
	
	void FixedUpdate ()
	{
		
		//Positioning
		if (gameObject.tag == "regGate" || gameObject.tag == "CenterGate" )
		{
			normalMoveLeft();
		}
		
		else if (gameObject.tag == "MovingGate" )
		{
			myTransform.Translate(new Vector3(-speedVar, speedVarMovingGate, 0.0f) * Time.fixedDeltaTime, Space.World);
			if (gameObject.collider.bounds.max.y >= topBorder.collider.bounds.min.y) 
			{	
				speedVarMovingGate = -speedVarMovingGate;
			}
			
			if (gameObject.collider.bounds.min.y <= bottomBorder.collider.bounds.max.y) 
			{	
				speedVarMovingGate = -speedVarMovingGate;
			}
		}
		
		else if (gameObject.tag == "BottomGate" )
		{
			if (myTransform.collider.bounds.min.y < bottomBorder.collider.bounds.max.y) 
			{
				myTransform.Translate(new Vector3(-speedVar, speedVarBottomTop, 0.0f) * Time.fixedDeltaTime, Space.World);
			}
			
			else
			{
				normalMoveLeft();
			}	
		}
		
		else 
		{
			if (myTransform.collider.bounds.max.y > topBorder.collider.bounds.min.y) 
			{
				myTransform.Translate(new Vector3(-speedVar, -(speedVarBottomTop), 0.0f) * Time.fixedDeltaTime, Space.World);	
			}
			
			else
			{
				normalMoveLeft();
			}	
		}
		
		
		
		/* 
		 *  Scoring and color
		 *  Is set to false at end of loop (one cycle) Score is reset on a collision(runnerCollision.cs)
		 *  Color changing turned off
		 */
		
		if(allowScoreOnPass && (myCollider.bounds.max.x < runner.collider.bounds.min.x) )
		{		
			//Increase BonusAmount (+1)
			runnerScoreMulti.setGateScoreMulti(runnerScoreMulti.getGateScoreMulti() + 1);
			//Display icon
			StartCoroutine(runnerScoreMulti.Animate());
			//Increase overall score
			runnerScore.IncreaseGateScore(runnerScoreMulti.getGateScoreMulti());
			
			//renderer.material.color = Color.gray;
			
			allowScoreOnPass = false;
		}
		
		
		
		//Destroy off the screen
		if (myCollider.bounds.max.x < topBorder.collider.bounds.min.x) 
		{	
			spawner.subtractNumObject();
			DestroyThis();
		}
	}
	
	public void onCollisionBehavior()
	{
		allowScoreOnPass = false;
		//renderer.material.color = Color.red;		
	}
	
	#region Getters&Setters
	/* 
	 * Getters & setters
	 */ 
	
	private void normalMoveLeft()
	{
		myTransform.Translate(new Vector3(-speedVar, 0.0f, 0.0f) * Time.fixedDeltaTime, Space.World);
	}
	
	public void setSpeedVar(float s)
	{
		speedVar = s;	
	}
	
	public void DestroyThis()
	{
		Destroy(this.gameObject);
	}
	

	
	

	#endregion
}
