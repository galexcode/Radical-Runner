using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

/* 
 * Class holds the game center score values
 */ 
public static class GameCenterScore
{
	private static bool authenticated;
	
    public static void Authenticate () 
	{
		authenticated = false;
        // Authenticate and register a ProcessAuthentication callback
        // This call needs to be made before we can proceed to other calls in the Social API
        Social.localUser.Authenticate (ProcessAuthentication);
    }

    // This function gets called when Authenticate completes
    // Note that if the operation is successful, Social.localUser will contain data from the server. 
    static void ProcessAuthentication (bool success) 
	{
        if (success) 
		{
            Debug.Log ("Authenticated");
			authenticated = true;
            // Request loaded achievements, and register a callback for processing them
            //Social.LoadAchievements (ProcessLoadedAchievements);
        }
        else
		{
			authenticated = false;
            Debug.Log ("Failed to authenticate");
		}
    }
	
	public static void ReportScore(long score)
	{
		if(authenticated)
		{
			string leaderboardID = "Test_Leaderboard1";
			Social.ReportScore (score, leaderboardID, success => 
			{
        		Debug.Log(success ? "Reported score successfully" : "Failed to report score");
   			});
		}
		else
		{
			//Social.localUser.Authenticate(ProcessAuthentication);	
		}	
	}
	
	public static bool getAuthenticated()
	{
		return authenticated;
	}
	
	public static string getUsername()
	{
		if(getAuthenticated() == true)
		{
			return Social.localUser.userName;	
		}
		else
		{
			return "Player"; 	
		}
	}
	
	public static void ResetAllAchievements()
	{
		GameCenterPlatform.ResetAllAchievements(HandleAchievementsReset);
	}
			
	private static void HandleAchievementsReset(bool status)
	{
		if(status)
		{
			Social.LoadAchievements (achievements => {
				if(achievements.Length > 0)
				{
					Debug.Log ("Achievements Loaded");
			    }
			    else
				{
			        Debug.Log ("No achievements returned");
				}
			});	
		}
	}

	//***********************************************************************************************
	public static void reportAchievement(string id, double percentage)
	{
		IAchievement achievement;
		achievement = Social.CreateAchievement();
		achievement.id = id;
		achievement.percentCompleted = percentage;
		achievement.ReportProgress( result => {
		    if (result)
			{
		        Debug.Log ("Successfully reported progress");
			}
		    else
		        Debug.Log ("Failed to report progress");
		});
		
		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
	}
	//***********************************************************************************************
}
