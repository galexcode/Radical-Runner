using UnityEngine;
using System.Collections;

/* Class enables return to main screen
 * Attached to credit Screen
 */ 

public class CreditsGoBack : MonoBehaviour 
{
	void OnFingerUp()
	{
		Application.LoadLevel(0);	
	}
}
