using UnityEngine;
using UnityEngine.SocialPlatforms;

/* 
 * Class controls game center functions
 */
public class GameCenter : MonoBehaviour 
{	

//  to call show gamecenter
//	#if UNITY_IPHONE && !UNITY_EDITOR
//		GameCenterScoreboard.ReportScore(scoreNum);
//		Social.ShowLeaderboardUI();
//	#endif
	
	
	//change to private
	public bool authenticated;
	
	private RunnerScoring scoreScript;
	
    void Start () 
	{
		authenticated = false;
        // Authenticate and register a ProcessAuthentication callback
        // This call needs to be made before we can proceed to other calls in the Social API
        Social.localUser.Authenticate (ProcessAuthentication);
    }

    // This function gets called when Authenticate completes
    // Note that if the operation is successful, Social.localUser will contain data from the server. 
    void ProcessAuthentication (bool success) 
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
	
	public void ReportScore(long score)
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
//
//    // This function gets called when the LoadAchievement call completes
//    void ProcessLoadedAchievements (IAchievement[] achievements) {
//        if (achievements.Length == 0)
//            Debug.Log ("Error: no achievements found");
//        else
//            Debug.Log ("Got " + achievements.Length + " achievements");
//
//        // You can also call into the functions like this
//        Social.ReportProgress ("Achievement01", 100.0, result => {
//            if (result)
//                Debug.Log ("Successfully reported achievement progress");
//            else
//                Debug.Log ("Failed to report achievement");
//        });
//    }
}
