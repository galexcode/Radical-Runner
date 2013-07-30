using UnityEngine;
using System.Collections;

/*
 * This class provides instructions for the arrows that scroll between instruction scenes 
 */

public class InstructionSceneArrows : MonoBehaviour 
{
	private GameObject InstructionSceneMainObject;
	
	// Use this for initialization
	void Start () 
	{
		InstructionSceneMainObject = GameObject.FindGameObjectWithTag("InstructionsSceneMain");
	}
	
	void OnFingerDown()
	{
		//Shift menu right on right arrow press
		if (tag == "RightArrow")
			{
				InstructionSceneMainObject.transform.position = 
				new Vector3(InstructionSceneMainObject.transform.position.x - 1000,
							InstructionSceneMainObject.transform.position.y,
							InstructionSceneMainObject.transform.position.z);
			}
		
		//Shift left on left arrow press
		else if (tag == "LeftArrow")
			{
				InstructionSceneMainObject.transform.position = 
				new Vector3(InstructionSceneMainObject.transform.position.x + 1000,
							InstructionSceneMainObject.transform.position.y,
							InstructionSceneMainObject.transform.position.z);
			}
		
		
						
		
	}
}
