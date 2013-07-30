using UnityEngine;
using System.Collections;

/*
 * Class controls all the objects spawned in the game
 */ 
public class SpawnObstacles : MonoBehaviour
{
	//Assigned in inspector
	public GameObject[] m_prefabChoices;
	public GameObject[] obstacles;
	
	//Viewable in inspettor - Change to private 
	public float minX = 400.0f;
	public float numObjects, maxObjects;
	
	public float speedVar,
				 timer, timerLimit, 
				 counter, counterLimit, 
				 srslyCounter, srslyCounterLimit,
				 counterLimitMin, counterLimitMax;
	public int numGates, numGatesInLevel, numSplats, numSaws, numLasers, numPushLasers;			
	public bool loljk, laserIsSpawned;
	public bool isDead;
	public string lastTag;
	public int levelCount;
	public float levelTimer, newLevelCount;
	public GameObject levelSign, clipBoard, clipBoardGrade;
	public bool spawnedSign, levelDelay, startMarker, achieve1, crossedFinish, spawnedFinish;
	public LevelDisplay levelText;
	
	private Transform myTransform;
	private RunnerCollision rCollision;
	private GameObject curObject, cube1, cube2, cube3, curPowerUp, curObstacle;
	private RunnerScript rPlayer;
	private TapDetector sDetector;
	private RunnerScoring score;
	private MoveProgressBar marker;
	private Vector3 markerStart, markerEnd;
	private LevelFinishScroll finish;

	//Passing speedVar
	private GameObject[] frontBackgroundObjects;
	private GameObject[] backBackgroundObjects;
	
	private GameObject bonusDown;
	
	void Awake()
	{
		if(PlayerPrefs.HasKey("volume"))
		{
			AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];	
			foreach(AudioSource source in audios)
			{
				if(source.tag == "MainCamera")
				source.volume = (float)PlayerPrefs.GetInt("volume")/10;	
			}	
		}
		
		if(PlayerPrefs.HasKey("volumeFX"))
		{
			AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
			foreach(AudioSource source in audios)
			{
				if(source.tag != "MainCamera")
				source.volume = (float)PlayerPrefs.GetInt("volumeFX")/10;
			}
		}
	}
	// Use this for initialization
	void Start ()
	{
		levelCount = 0;
		newLevelCount = 30.0f;
		timerLimit = 1.0f;
		
		//Assign object spawn variables
		counterLimitMin = 1.0f;
		counterLimitMax = counterLimitMin * 5.0f;
		counterLimit = Random.Range(counterLimitMin, counterLimitMax);
		
		minX = 100.0f;
		myTransform = transform;
		isDead = true;
		startMarker = false;
		score = GameObject.FindGameObjectWithTag("Score").GetComponent<RunnerScoring>();
		sDetector = GameObject.FindGameObjectWithTag("Recognizer").GetComponent<TapDetector>();
		rPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerScript>();
		rCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerCollision>();

		marker = GameObject.FindGameObjectWithTag("marker").GetComponent<MoveProgressBar>();
		markerStart = new Vector3(-500.0f, 355.0f, -80.0f);
		markerEnd = new Vector3(500.0f, 355.0f, -80.0f);
		
		numGates = 0;
		numGatesInLevel = 0;
		
		spawnedFinish = false;
		crossedFinish = false;
		finish = GameObject.FindGameObjectWithTag("lastObject").GetComponent<LevelFinishScroll>();
		levelText = GameObject.FindGameObjectWithTag("LevelText").GetComponent<LevelDisplay>();
		levelText.setTheLevelText(levelCount);
		
		//Pass speedVar objects
		bonusDown = GameObject.FindGameObjectWithTag("BonusDown");
		backBackgroundObjects = GameObject.FindGameObjectsWithTag("BackBackground");
		frontBackgroundObjects = GameObject.FindGameObjectsWithTag("FrontBackground");
		
		if (m_prefabChoices == null || m_prefabChoices.Length == 0) 
		{
			return;	
		}
		
		//test
		rPlayer.setGravity((rPlayer.getGravity() + speedVar * -0.10f));
		
		srslyCounterLimit = 10.0f; 
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(loljk)
		{
			if(srslyCounter < srslyCounterLimit)
			{
				if(isDead)
				{
					#if UNITY_IPHONE && !UNITY_EDITOR
						GameCenterScore.reportAchievement("srsly", 100.0);
					#endif
					loljk = false;
					Debug.Log("srsly?");
				}
			}
			else
			{
				loljk = false;	
			}
			if(!isDead)
			{
				srslyCounter += 1.0f * Time.deltaTime;	
			}
		}
		//if no curObject or if position is below minX
		if (!isDead)
		{
			//doesn't start the level until set to true
			if(!levelDelay)
			{
				//only happens at the start of the game
				if(!startMarker)
				{
					marker.setIsDead(false);
					rCollision.setNumGatesHit(0);
					numGatesInLevel = 0;
					levelCount += 1;
					StartCoroutine(marker.MoveFromTo(markerStart, markerEnd, newLevelCount));
					startMarker = true;
					score.setIncreaseMulti(true);
					
					
					//sets the text for the level
					levelText.setTheLevelText(levelCount);

					foreach (GameObject element in backBackgroundObjects )
					{
						element.SendMessage("setSpeedFromSpawner", speedVar);
						//element.SendMessage("ChangeBackgroundColor");
					}
					
					foreach (GameObject element in frontBackgroundObjects )
					{
						element.SendMessage("setSpeedFromSpawner", speedVar);
						//element.SendMessage("ChangeBackgroundColor");
					}
					
					bonusDown.SendMessage("setSpeedFromSpawner", speedVar);
					
					PowerUp[] powerUpButtons = GameObject.FindObjectsOfType(typeof(PowerUp)) as PowerUp[];
					foreach(PowerUp button in powerUpButtons)
					{
						button.setTypeIsSet(false);	
					}
				}
				
				levelTimer += Time.deltaTime;
				counter += Time.deltaTime;
				if (curObject == null || (numObjects < maxObjects && curObject.collider.bounds.max.x < (minX + Random.Range (-20, 20))))
				{	
					//ChooseGate();
					curObject = Instantiate (ChooseGate(), myTransform.position, Quaternion.identity) as GameObject;
					numGates++;
					numGatesInLevel++;
					curObject.SendMessage ("setSpeedVar", speedVar);
					numObjects++;
					lastTag = cube1.tag;
					
				}
				
				if(numObjects < maxObjects && counter > counterLimit)
				{
					SpawnObstacle();	
				}
			}
			else
			{
				if(!crossedFinish)
				{
					if(!spawnedFinish)
					{
						finish.FirstTransitionMove();
						spawnedFinish = true;
					}
				}
				else if(!spawnedSign)
				{
					if(rPlayer.getHoriMoveSpeed() < 0.0f)
					{
						rPlayer.setHoriMoveSpeed(0.0f);	
					}
					if(numObjects == 0)
					{
						StartCoroutine(spawnLevelSign());
						spawnedSign = true;
					}
					
				}
			}
			
			if(levelTimer > newLevelCount)
			{
				setLevelDelay(true);
				startMarker = false;
				
				
				speedVar = speedVar + (speedVar * 0.075f);
				minX = minX + (minX * 0.075f);
				
				//Decrease the min amount of time it takes to spawn an obstacle by 5%
				counterLimitMin = counterLimitMin - (counterLimitMin * 0.05f);
				counterLimitMax = counterLimitMin * 5.0f;
				rPlayer.setGravity((rPlayer.getGravity() + speedVar * -0.075f));
				
				score.setHasStarted(false);
				score.setIncreaseMulti(false);
				levelTimer = 0.0f;
				
				newLevelCount = newLevelCount + (newLevelCount * 0.1f);
				//Allow more obstcals to spawn per level
				if (maxObjects < 10)
					maxObjects++;

				
				//level 11 achievement
				if(!achieve1 && GameCenterScore.getAuthenticated() == true && levelCount == 11)
				{
					#if UNITY_IPHONE && !UNITY_EDITOR
						GameCenterScore.reportAchievement("level11", 100.0);
					#endif
					achieve1 = true;
				}				
			}		
		}
		else
		{
			marker.setIsDead(true);	
		}
		
	}
	
	private GameObject ChooseGate()
	{
		int choice = Random.Range (0, m_prefabChoices.Length);
		cube1 = m_prefabChoices [choice];
		if (cube1.tag == lastTag)
		{
			return ChooseGate();
		}
		else
		{
			return cube1;	
		}			
	}
	
	private GameObject ChooseObstacle()
	{
		int choice = Random.Range(0, obstacles.Length);
		cube3 = obstacles[choice];
		if(cube3.tag == "Orb" && getLaserIsSpawned() == true)
		{
			return ChooseObstacle();
		}
		else 
		{
			return cube3;
		}
	}
	
	private void SpawnObstacle()
	{
		counter = 0.0f;
		counterLimit = Random.Range(counterLimitMin, counterLimitMax);
				
		ChooseObstacle();
		
		curObstacle = Instantiate(cube3, myTransform.position, Quaternion.identity) as GameObject;
		
		if (curObstacle.tag != "Orb" && curObstacle.tag != "Orb2" && curObstacle.tag != "inkBlob")
		{
			curObstacle.SendMessage("setSpeedVar", speedVar);
		}
		
		if(curObstacle.tag == "inkBlob")
		{
			numSplats++;	
		}
		if(curObstacle.tag == "Orb")
		{
			setLaserIsSpawned(true);
			numLasers++;	
		}
		if(curObstacle.tag == "Orb2")
		{
			numPushLasers++;
		}
		if(curObstacle.tag == "CollisionDeath")
		{
			numSaws++;	
		}
		numObjects++;
		
	}
	
	public void ClearGameScreen()
	{
		if(GameObject.FindObjectsOfType(typeof(GateBehavior)) != null)
		{
			GateBehavior[] theGates = GameObject.FindObjectsOfType(typeof(GateBehavior)) as GateBehavior[];
			foreach(GateBehavior obstacle in theGates)
			{
				obstacle.DestroyThis();
				numObjects--;
			}
		}
		if(GameObject.FindObjectsOfType(typeof(BuzzScript)) != null)
		{
			BuzzScript[] buzzes = GameObject.FindObjectsOfType(typeof(BuzzScript)) as BuzzScript[];
			foreach(BuzzScript buzz in buzzes)
			{
				buzz.DestroyThis();	
				numObjects--;
			}
		}
		if(GameObject.FindObjectsOfType(typeof(InkBlob)) != null)
		{
			InkBlob[] splats = GameObject.FindObjectsOfType(typeof(InkBlob)) as InkBlob[];
			foreach(InkBlob goo in splats)
			{
				goo.DestroyThis();
				numObjects--;
			}
		}
		
		
		if(GameObject.FindObjectsOfType(typeof(OrbLaser)) != null)
		{
			OrbLaser[] orbs = GameObject.FindObjectsOfType(typeof(OrbLaser)) as OrbLaser[];
			foreach(OrbLaser orb in orbs)
			{
				if(orb.tag == "Orb")
				{
					numObjects--;	
				}
				Destroy(orb.gameObject);
			}
		}
		
		if(GameObject.FindObjectsOfType(typeof(OrbLaser2)) != null)
		{
			OrbLaser2[] orbs2 = GameObject.FindObjectsOfType(typeof(OrbLaser2)) as OrbLaser2[];
			foreach(OrbLaser2 orb in orbs2)
			{
				if(orb.tag == "Orb2")
				{
					numObjects--;	
				}
				Destroy(orb.gameObject);
			}
		}
	}
	
	private IEnumerator spawnLevelSign()
	{
		yield return new WaitForSeconds(0.5f);
		Instantiate(clipBoard);
		yield return new WaitForSeconds(1.0f);
		Instantiate(levelSign);
		yield return new WaitForSeconds(2.2f);
		setLaserIsSpawned(false);
		Instantiate(clipBoardGrade);
		sDetector.setNextLevel(true);
	}
	
	#region getter/setters
	//srsly achievement
	public void setLolJk(bool b)
	{
		loljk = b;	
	}
	
	public int getLevelCount()
	{
		return levelCount;	
	}
	public bool getLevelDelay()
	{
		return levelDelay;	
	}
	public void setLevelDelay(bool f)
	{
		levelDelay = f;	
	}
	public void setIsDead (bool b)
	{
		isDead = b;	
	}

	public void subtractNumObject ()
	{
		numObjects--;	
	}
	
	public float getSpeedVar ()
	{
		return speedVar;	
	}
	
	public void setSpawnedSign(bool b)
	{
		spawnedSign = b;	
	}

	public void setCrossedFinish(bool b)
	{
		crossedFinish = b;	
	}
	
	public void setSpawnedFinish(bool b)
	{
		spawnedFinish = b;	
	}
	
	public int getNumGates()
	{
		return numGates;	
	}
	
	public int getNumGatesInLevel()
	{
		return numGatesInLevel;	
	}
	
	public int getNumSplats()
	{
		return numSplats;	
	}
	
	public int getNumSaws()
	{
		return numSaws;
	}
	
	public int getNumLasers()
	{
		return numLasers;	
	}
	
	public int getNumPushLasers()
	{
		return numPushLasers;
	}
	
	public void setLaserIsSpawned(bool b)
	{
		laserIsSpawned = b;	
	}
	
	public bool getLaserIsSpawned()
	{
		return laserIsSpawned;	
	}
	
	#endregion
}