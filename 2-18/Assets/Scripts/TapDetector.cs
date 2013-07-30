using UnityEngine;
using System.Collections;


public class TapDetector : MonoBehaviour 
{
	public bool isDead;
	public bool onStart;
	public bool nextLevel;
	public bool onSettingsScreen;
	
	private Transform myTransform;
	private RunnerCollision rCollision;
	private SpawnObstacles spawner;
	private RunnerScoring score2;
	private LevelFinishScroll lObjectSign;
	private Camera cam;
	
	//Passing speedVar
	private GameObject[] frontBackgroundObjects;
	private GameObject[] backBackgroundObjects;
	
	// Use this for initialization
	void Start () 
	{
		//If not title screen
		if(Application.loadedLevel != 0 )
		{
			myTransform = transform;
			myTransform.localScale = new Vector3(Camera.main.orthographicSize * 3, Camera.main.orthographicSize * 2, 1.0f);
			spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>();
			rCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerCollision>();
			cam = GameObject.FindGameObjectWithTag("MainCamera").camera;
			score2 = GameObject.FindGameObjectWithTag("Score").GetComponent<RunnerScoring>();
			lObjectSign = GameObject.FindGameObjectWithTag("lastObject").GetComponent<LevelFinishScroll>();
	
			
			//Populate background pillars
			backBackgroundObjects = GameObject.FindGameObjectsWithTag("BackBackground");
			frontBackgroundObjects = GameObject.FindGameObjectsWithTag("FrontBackground");
		}
	}

	void OnFingerDown()
	{
		if(!onSettingsScreen)
		{
			if(onStart)
			{
				score2.setHasStarted(true);
				score2.setScoreMultiplier(1);
				spawner.setIsDead(false);
				
				PowerUp[] powerUps = GameObject.FindObjectsOfType(typeof(PowerUp)) as PowerUp[];
				foreach(PowerUp button in powerUps)
				{
					button.setOnStart(false);	
				}
				//***************************************
				spawner.setLolJk(true);
				//***************************************
				Destroy(GameObject.FindGameObjectWithTag("Respawn"));
				onStart = false;
			}
			
			if(isDead)
			{
				rCollision.setGameRestart(true);
			}
			
			if(nextLevel)
			{
				//Change Color Scheme
				cam.camera.backgroundColor = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), 1);
				
				foreach (GameObject element in backBackgroundObjects )
				{
					//element.SendMessage("setSpeedFromSpawner", speedVar);
					element.SendMessage("ChangeBackgroundColor");
				}
						
				foreach (GameObject element in frontBackgroundObjects )
				{
					//element.SendMessage("setSpeedFromSpawner", speedVar);
					element.SendMessage("ChangeBackgroundColor");
				}
				
				score2.setHasStarted(true);
				
				//Current onscreen items destroyed
				Destroy(GameObject.FindGameObjectWithTag("nextLevel"));
				lObjectSign.SecondTransitionMove();
				Destroy(GameObject.FindGameObjectWithTag("ClipBoard"));
				Destroy(GameObject.FindGameObjectWithTag("ClipBoardGrade"));
				spawner.setLevelDelay(false);
				spawner.setSpawnedSign(false);
				rCollision.setAllowScoring(true);
				
				//Change background pillar colors
				
				//Power up buttons between levels
				PowerUp[] powerUps = GameObject.FindObjectsOfType(typeof(PowerUp)) as PowerUp[];
				foreach( PowerUp button in powerUps)
				{
					button.setOnStart(false);	
				}
				nextLevel = false;
				
			}
		}
	}	
	
	#region getters/setters
	public void setOnStart(bool b)
	{
		onStart = b;	
	}
	
	public void setIsDead(bool b)
	{
		isDead = b;	
	}
	
	public void setNextLevel(bool b)
	{
		nextLevel = b;	
	}
	
	public void setOnSettingsScreen(bool b)
	{
		onSettingsScreen = b;	
	}
	#endregion
}
