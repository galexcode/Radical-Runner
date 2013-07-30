using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.GameCenter;

/* 
 * Class controls the game score system
 */ 
public class RunnerScoring : MonoBehaviour 
{
	//Inspector variables - Change to private 
	public bool isDead, isScoring, scored, hasStarted, increaseMulti, achieve1,  achieve2;
	
	//Score objects
	public GameObject hScores, highScoreSign;
	
	private exSpriteFont spriteFont;
	private float timer, delay;
	private RunnerScript runner;
	private RunnerCollision runnerCol;
	private int scoreMultiplier;
	
	//Scoring
	private int gateScore, gateScoreMulti;
	
	//Acheivement
	private int AcheiveScore;
 
	void Start ()    
	{
		//Object access		
		runner = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerScript>();
		runnerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerCollision>();
		spriteFont = GetComponent<exSpriteFont>();
		
		//Initiated by swipeDetector.cs
		hasStarted = false;

		//Set in runnerCollision
		scoreMultiplier = 1;
		
		//acheivs
		achieve1 = false;
		achieve2 = false;
		delay = 1.0f;
		
		//Gate Scoring
		gateScore = 0;
		gateScoreMulti = 1;
		
	}
	
	void FixedUpdate () 
	{
		//Score Display String
		spriteFont.text = gateScore.ToString();
	
		//Acheivement - Possibly Remove
		if(hasStarted && runnerCol.allowScoring && (runner.getState() != RunnerScript.State.respawn) )
		{
			timer += Time.deltaTime;
			if(timer > delay)
			{
				AcheiveScore += 1;
				timer = 0.0f;
				
			#if UNITY_IPHONE && !UNITY_EDITOR
				
				//1000 pts achievement
				if(!achieve1)
				{
					if(AcheiveScore > 1000.0f)
					{
						if(GameCenterScore.getAuthenticated() == true)
						{
							GameCenterScore.reportAchievement("500pts", 100.0);
							achieve1 = true;
						}
					}
				}
				//5x bonus achievement
				if(!achieve2)
				{
					if(scoreMultiplier == 5)
					{
						if(GameCenterScore.getAuthenticated() == true)
						{
							GameCenterScore.reportAchievement("5xBonus", 100.0);
							achieve2 = true;
						}
					}
				}
					
				
			#endif
			}
			
		}
		// Record Scores
		if(isDead)
		{
			if(Highscores.IsHighscore(1, gateScore) && !scored)
			{
				#if UNITY_IPHONE && !UNITY_EDITOR
					Highscores.Store(1, GameCenterScore.getUsername() , gateScore);
					Instantiate(highScoreSign);
					Instantiate(hScores);
					GameCenterScore.ReportScore(gateScore);
				
				#elif UNITY_EDITOR
					Highscores.Store(1, "Player", gateScore);
					Debug.Log("scored high score");
					Instantiate(highScoreSign);
					Instantiate(hScores);
				
				#endif
				scored = true;
			}
			else if(!scored)
			{
				Instantiate(hScores);
				scored = true;
				#if UNITY_IPHONE && !UNITY_EDITOR
					GameCenterScore.ReportScore(gateScore);
					//Social.ShowLeaderboardUI();
				#endif
			}	
		}
	}
	

	public void IncreaseGateScore(int i)
	{
		gateScore += i;	
	}
	#region Get/Set
	public void setIsScoring(bool s)
	{
		isScoring = s;	
	}
	
	public void setHasStarted(bool s)
	{
		hasStarted = s;	
	}
	
	public int getScoreMultiplier()
	{
		return scoreMultiplier;	
	}
	
	public void setScoreMultiplier(int i)
	{
		if(increaseMulti)
		{
			scoreMultiplier = i;
		}
	}
	
	public bool getIncreaseMulti()
	{
		return increaseMulti;	
	}
	public void setIncreaseMulti(bool b)
	{
		increaseMulti = b;	
	}
	public void setIsDead(bool s)
	{
		isDead = s;	
	}
	public int getGateScore()
	{
		return gateScore;
	}
	public void setGateScore(int i)
	{
		gateScore = i;
	}
	public int getGateScoreMulti()
	{
		return gateScoreMulti;
	}
	public void setGateScoreMulti(int i)
	{
		gateScoreMulti = i;
	}
	#endregion
}
