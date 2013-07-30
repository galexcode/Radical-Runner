using UnityEngine;
using System.Collections;

/* Class controls players collision detection and movement based on collisions
 * Also controls actions that take place based on collision with items
 * ------------------
 * Gates = layer 9
 * Borders = layer 10
 */

public class RunnerCollision : MonoBehaviour
{
	//UI splat (public, add in inspector)
	public GameObject splatEffect1, splatEffect2, splatEffect3, splatEffect4, SplatHandEffect;
	public GameObject gameOver, tapToContinue;
	
	//Power up variables - Change to private on release
	public float slowTimeDuration, myTimeScale, slowTimeTimer, intangibleTimer, intangibleDuration, shrinkTimer, shrinkDuration;
	
	//Bonus point variables
	public bool allowScoring;
	public bool allowScoringMultiplier;
	
	//Achievement (Close Call)
	public bool closeCallStart, closeCallEnd;
	
	private RunnerScript pMovement;
	private Vector2 collisionOffset;
	private float collisionBoundOffset;
	private Transform myTransform;
	private bool intangible;

	//Respawn variables
	private bool gameRestart, respawn;
	private exSpriteAnimation buttonSprite;
	private TapDetector detector;
	private SpawnObstacles spawner;
	
	//Bonus point variables
	private RunnerScoring score;
	private RunnerScoringMulti scoreMulti;
	private GameObject bonusx2, bonusx3;
	private BonusDownBehavior myBonusBehaviorDown;
	private bool allowBonusDown;
	private int numGatesHit;
	
	//Camera control
	private GameObject myCamera;
	
	void Start ()
	{
		//Object access
		pMovement = gameObject.GetComponent<RunnerScript> ();
		myTransform = transform;
		collisionBoundOffset = pMovement.collider.bounds.size.y / 4.0f;
		score = GameObject.FindGameObjectWithTag("Score").GetComponent<RunnerScoring>();
		scoreMulti = GameObject.FindGameObjectWithTag("ScoreMulti").GetComponent<RunnerScoringMulti>();
		detector = GameObject.FindGameObjectWithTag("Recognizer").GetComponent<TapDetector>();
		spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>();
		myBonusBehaviorDown = GameObject.FindGameObjectWithTag("BonusDown").GetComponent<BonusDownBehavior>();

		//Respawn variables
		respawn = false;
		gameRestart = false;	

		//Scoring & bonus
		allowScoring = true;
		allowScoringMultiplier = true;
		allowBonusDown = true;
		numGatesHit = 0;
		
		//Camera control
		myCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		
		//Acheivement variables
		closeCallStart = false;
		closeCallEnd = false;

	}
	
	void FixedUpdate ()
	{
		if (respawn) 
		{
			ApplyResetConfig();
		} 
		
		else 
		{
			if(slowTimeDuration > 0.0f)
			{
				SlowDownTime();	
			}
			if(intangibleDuration > 0.0f)
			{
				BeIntangible();
			}
			if(shrinkDuration > 0.0f)
			{
				ShrinkSize();	
			}
			
			//close call achievement
			//#if UNITY_IPHONE && !UNITY_EDITOR
				if(closeCallStart)
				{
					if(closeCallEnd)
					{
						#if UNITY_IPHONE && !UNITY_EDITOR
							GameCenterScore.reportAchievement("CloseCall", 100.0);
						#endif
						Debug.Log("Close");
						closeCallStart = false;
						closeCallEnd = false;
					}
				}
			//#endif
			
		}	
	}
	
	#region getters/setters
	
	//added for scaling
	public void setShrinkDuration(float f)
	{
		shrinkDuration = f;	
	}
	//added for scaling
	public void setSpriteScale(float f)
	{
		myTransform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
	}
	public void setIntangibleDuration(float f)
	{
		intangibleDuration = f;	
	}
	public void setIntangible(bool r)
	{
		intangible = r;	
	}
	public void setGameRestart(bool r)
	{
		gameRestart = r;	
	}
	
	public bool getGameRestart()
	{
		return gameRestart;
	}	
	
	public void setRespawn(bool s)
	{
		respawn = s;	
	}
	
	public bool getRespawn()
	{
		return respawn;	
	}
	
	public void setSlowTimeDuration(float f)
	{
		slowTimeDuration = f;	
	}
	
	public void setTimeScale(float f)
	{
		myTimeScale = f;	
	}
	public void setAllowScoringMultiplier(bool b)
	{
		allowScoringMultiplier = b;	
	}
	
	public void setAllowScoring(bool b)
	{
		allowScoring = b;	
	}
	
	public Vector3 getCollisonLocation()
	{
		return collisionOffset;
	}
	
	public void setCloseCallStart(bool b)
	{
		closeCallStart = b;
	}
	
	public void setCloseCallEnd(bool b)
	{
		closeCallEnd = b;
	}
	
	public int getNumGatesHit()
	{
		return numGatesHit;	
	}
	
	public void setNumGatesHit(int i)
	{
		numGatesHit  = i;	
	}

	#endregion
	
	void OnTriggerEnter(Collider c)
	{
		allowBonusDown = false;
		
		//Gates
		if(c.gameObject.layer == 9 && !intangible)
		{
			allowBonusDown = true;
		}
	
	}
	
	void OnTriggerStay(Collider c)
	{
		if(c.tag == "lastObject")
		{
			if (c.transform.position.x == 0.0f)
			{
				spawner.setCrossedFinish(true);	
			}
		}
		
		//Layer 10, borders
		if(c.gameObject.layer == 10 )
		{
			VerticalCollision(c);
		}
		
		//Layer 9, gates
		if(c.gameObject.layer == 9 )
		{

			if(!intangible)
			{
				//Vertical bounds of a collision
				if ( c.collider.bounds.max.y > (pMovement.collider.bounds.min.y + collisionBoundOffset )  && c.collider.bounds.min.y < (pMovement.collider.bounds.max.y - collisionBoundOffset))
				{
					HorizontalCollision(c);
				}	
	
				//Horizontal bounds of a collision
				 else if ( c.collider.bounds.max.x > pMovement.collider.bounds.min.x + collisionBoundOffset  && c.collider.bounds.min.x < pMovement.collider.bounds.max.x - collisionBoundOffset )
				{
					VerticalCollision(c);
				}
				
				if (allowScoringMultiplier == true)
				{
					allowScoringMultiplier = false;
				}
			}
		}
		
		//End of level
		if(c.tag == "lastObject")
		{
			if(c.transform.position.x == 0.0f)
			{
				spawner.setCrossedFinish(true);
			}
		}
				
		if(c.tag == "inkBlob")
		{	
			if(!intangible)
			{
				Instantiate(splatEffect1);
				Instantiate(splatEffect2);
				Instantiate(splatEffect3);
				Instantiate(splatEffect4);
				if(GameObject.FindGameObjectWithTag("SplatHand") == null)
				{
					Instantiate(SplatHandEffect);
				}
				spawner.subtractNumObject();
				Destroy(c.gameObject);
			}
		}
		
		//Layer 13, death on collision
		if(c.gameObject.layer == 13)
		{
			if(!intangible)
			{
				setRespawn(true);
			}
		}
		
		//Pushes player backwards
		if(c.tag == "OrbLaser2")
		{
		
			if(!intangible)
			{	
				//If player is at max distance, move back before applying laser collision
				if (myTransform.position.x >= -2)
				{
					myTransform.position = new Vector3 (myTransform.position.x - 2, myTransform.position.y, myTransform.position.z);
				}
				pMovement.setHoriMoveSpeed(-500.0f);
				
			}
		}	
	}
	
	
	//Actions taken when a collision with a gate happens
	private void DisplayCollisionSprite(Vector3 collisionLocation, GameObject gate)
	{
		if (allowBonusDown && !intangible)
		{
			allowBonusDown = false;
			
			//Display collision image
			StartCoroutine (myBonusBehaviorDown.DisplayBonusDownSprite(new Vector3(collisionLocation.x, collisionLocation.y, -2.0f) ));
			
			if (gate.layer == 9 && gate.GetComponent<GateBehavior>() != null)
			{
				gate.GetComponent<GateBehavior>().onCollisionBehavior();
				
				//Record number of gates hit per level, sent to ClipBoardText.cs
				numGatesHit++;
			}
			
			//Reset Score multiplier
			scoreMulti.setGateScoreMulti(0);	

		}	
	}
	
	private void HorizontalCollision(Collider c)
	{
		//Player is on the left side of object they are colliding with
		if (c.transform.position.x > pMovement.transform.position.x)
		{	
			//Set player to the position of the collision. 
			//Using OnTriggerStay (Not OnTriggerEnter) so using 2.05f is used instead of 2.0f
			collisionOffset = new Vector2 (c.collider.bounds.min.x - (pMovement.collider.bounds.size.x /2.05f), pMovement.transform.position.y);
			DisplayCollisionSprite(c.ClosestPointOnBounds(myTransform.position), c.gameObject);
			pMovement.setMyTransformPosition (collisionOffset);
			
				
		}
		
		//Player is on the right side of object they are colliding with
		else if (c.transform.position.x < pMovement.transform.position.x)
		{
			collisionOffset = new Vector2 (c.collider.bounds.max.x + (pMovement.collider.bounds.size.x / 2.05f), pMovement.transform.position.y);
			pMovement.setMyTransformPosition (collisionOffset);
			DisplayCollisionSprite(c.ClosestPointOnBounds(myTransform.position), c.gameObject);
		}
	}
	
	private void VerticalCollision(Collider c)
	{
		//If above the object
		if (c.transform.position.y < pMovement.transform.position.y)
		{
			//Landing on object from above
			//if (pMovement.getGravity () < 0) 
			if(pMovement.getGravVal() < 0)
			{
				pMovement.setState (RunnerScript.State.onGround);
				collisionOffset = new Vector2 (pMovement.transform.position.x, c.collider.bounds.max.y + (pMovement.collider.bounds.size.y / 2.05f));
				pMovement.setMyTransformPosition (collisionOffset);	
				DisplayCollisionSprite(c.ClosestPointOnBounds(myTransform.position), c.gameObject);
			} 
			
			//Bounce-off if colliding due to inertia, this part may work better in OnTriggerEnter
			else 
			{
				pMovement.setState(RunnerScript.State.inAir);
				pMovement.setFallSpeed(0.0f);
				collisionOffset = new Vector2 (pMovement.transform.position.x, c.collider.bounds.max.y + pMovement.collider.bounds.size.y/ 1.9f);
				pMovement.setMyTransformPosition (collisionOffset);
				DisplayCollisionSprite(c.ClosestPointOnBounds(myTransform.position), c.gameObject);
			}
				
		}
		
		//If below the object
		if (c.transform.position.y > pMovement.transform.position.y)	
		{
			//Landing on object from below/upside down
			//if (pMovement.getGravity () > 0) 
			if(pMovement.getGravVal() > 0)
			{
				pMovement.setState (RunnerScript.State.onGround);
				collisionOffset = new Vector2 (pMovement.transform.position.x, c.collider.bounds.min.y - (pMovement.collider.bounds.size.y / 2.05f));
				pMovement.setMyTransformPosition (collisionOffset);
				DisplayCollisionSprite(c.ClosestPointOnBounds(myTransform.position), c.gameObject);

			} 
			
			else 
			{
				pMovement.setState(RunnerScript.State.inAir);
				pMovement.setFallSpeed(0.0f);
				collisionOffset = new Vector2 (pMovement.transform.position.x, c.collider.bounds.min.y - (pMovement.collider.bounds.size.y / 1.9f));
				pMovement.setMyTransformPosition (collisionOffset);
				DisplayCollisionSprite(c.ClosestPointOnBounds(myTransform.position), c.gameObject);

			}
		}
	}

	void OnTriggerExit (Collider c)
	{
		if((c.gameObject.layer == 9 && !intangible) || c.gameObject.layer == 10)
		{	
			pMovement.setState(RunnerScript.State.inAir);
			allowScoring = true;
			allowBonusDown = true;
		}	
		
		//Speed reset after speed changing laser
		if (c.tag == "OrbLaser2")
		{
			if (pMovement.getHoriMoveSpeed() < 0.0f)
			{
				pMovement.setHoriMoveSpeed(0.0f);
			}
		}
			
	}
	
	private void ApplyResetConfig()
	{
		Time.timeScale = 1.0f;
		gameObject.renderer.enabled = false;
		gameObject.collider.enabled = false;
		allowScoring = false;
		score.setIsDead(true);
		detector.setIsDead(true);
		spawner.setIsDead(true);
		
		if(GameObject.FindGameObjectWithTag("Respawn") == null)
		{
			Instantiate(gameOver);
		}

		if(gameRestart)
		{
			Application.LoadLevel (Application.loadedLevel);
		}
	}
	
	
	
	void SlowDownTime()
	{
		//New slow speeds
		Time.timeScale = myTimeScale;
		slowTimeTimer += Time.deltaTime;
		myCamera.audio.pitch = 0.4f;
		
		//Reset speeds
		if(slowTimeTimer >= slowTimeDuration)
		{
			myCamera.audio.pitch = 1.0f;
			slowTimeTimer = 0.0f;
			slowTimeDuration = 0.0f;
			myTimeScale = 1.0f;
			Time.timeScale = myTimeScale;
		}
		
	}
	
	//Added for scaling
	void ShrinkSize()
	{
		shrinkTimer += Time.deltaTime;
		if(shrinkTimer > shrinkDuration)
		{
			shrinkTimer = 0.0f;
			shrinkDuration = 0.0f;
			myTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}
	}
	
	void BeIntangible()
	{
		intangibleTimer += Time.deltaTime;
		if(intangibleTimer > intangibleDuration)
		{
			setIntangible(false);
			intangibleTimer = 0.0f;
			intangibleDuration = 0.0f;
		}
	}
	
}




