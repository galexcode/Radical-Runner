using UnityEngine;

using System.Collections;

 /* 
  * Class controls the high-scores board
  */

public static class Highscores 
{
    public static int lastLevel=-1;
    public static int lastIndex=-1;

    private static bool initialized=Initialize();

    static bool Initialize() 
	{      
        return true;
    }

    public static bool IsHighscore(int level, int score) 
	{
        if(score == 0) return false;

        for(int i=0;i<10;i++) 
		{
            if(PlayerPrefs.HasKey("highscore_"+level+"_"+i) && PlayerPrefs.HasKey("name_"+level+"_"+i)) 
			{
                if(score>PlayerPrefs.GetInt("highscore_"+level+"_"+i)) 
				{
                    return true;
                }

            } 
			else 
			{
                return true;
            }
        }
        return false;
    }

    
    public static void Store(int level, string name, int score) 
	{
        for(int i=0;i<10;i++) 
		{
            if(PlayerPrefs.HasKey("highscore_"+level+"_"+i) && PlayerPrefs.HasKey("name_"+level+"_"+i)) 
			{
                if(score>PlayerPrefs.GetInt("highscore_"+level+"_"+i)) 
				{
                    StoreAt(level,i,name,score);
                    return;
                }
            } 
			else 
			{
                StoreAt(level,i,name,score);
                return;
            }
        }
    }

    private static void StoreAt(int level, int index,string name,int score) 
	{
        lastLevel=level;
        lastIndex=index;

        string tmpName;
        int tmpScore;

        for(int i=8;i>=index;i--) 
		{
            tmpName=PlayerPrefs.GetString("name_"+level+"_"+i);
            tmpScore=PlayerPrefs.GetInt("highscore_"+level+"_"+i);
            PlayerPrefs.SetString("name_"+level+"_"+(i+1),tmpName);
            PlayerPrefs.SetInt("highscore_"+level+"_"+(i+1),tmpScore);
        }

        PlayerPrefs.SetString("name_"+level+"_"+index,name);
        PlayerPrefs.SetInt("highscore_"+level+"_"+index,score);
    }

    public static string GetNames(int level) 
	{
        string ret="";

        for(int i=0;i<10;i++) 
		{
            ret+=GetName(level,i)+"\n";
        }
        return ret;
    }

    public static string GetName(int level, int index) 
	{
        if(PlayerPrefs.HasKey("name_"+level+"_"+index) && PlayerPrefs.GetString("name_"+level+"_"+index)!="") 
		{
            return PlayerPrefs.GetString("name_"+level+"_"+index);
        } 
		else 
		{
            return "-";
        }
    }

    public static string GetScores(int level) 
	{
        string ret="";

        for(int i=0;i<10;i++) 
		{
            ret+=GetScore(level,i)+"\n";
        }

        return ret;
    }

    public static string GetScore(int level, int index) 
	{
        if(PlayerPrefs.HasKey("highscore_"+level+"_"+index) && PlayerPrefs.GetString("name_"+level+"_"+index)!="") 
		{
            return ""+PlayerPrefs.GetInt("highscore_"+level+"_"+index);
        } 
		else 
		{
            return "-";
        }
    }

    public static int GetScoreNum(int level, int index) 
	{
        if(PlayerPrefs.HasKey("highscore_"+level+"_"+index) && PlayerPrefs.GetString("name_"+level+"_"+index)!="") 
		{
            return PlayerPrefs.GetInt("highscore_"+level+"_"+index);
        } 
		else 
		{
            return 0;
        }

    }

    public static string GetIndexes() 
	{
        string ret="";

        for(int i=0;i<10;i++) 
		{
            ret+=(i+1)+"\n";
        }
		
        return ret;
    }

    public static string GetIndicator(int level, string sign) 
	{
        if(level!=lastLevel) return "";
        if(lastIndex==-1) return "";
        string ret="";

        for(int i=0;i<lastIndex;i++) 
		{

            ret+="\n";
        }

        ret+=sign;
        return ret;
    }


    public static void Clear(int level) 
	{
        for(int i=0;i<10;i++) 
		{
            PlayerPrefs.DeleteKey("highscore_"+level+"_"+i);
            PlayerPrefs.DeleteKey("name_"+level+"_"+i);
        }
    }
}