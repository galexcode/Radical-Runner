using UnityEngine;
using System.Collections;

/* 
 * Class controls the small character at the top of the screen, representing the level progress
 */

public class MoveProgressBar : MonoBehaviour 
{
	public bool moving = false;
	public bool isDead, notSet;
	
	private Transform myTransform;
	
	void Start () 
	{
		myTransform = transform;
		isDead = false;
		notSet = false;
	}
	
	void Update()
	{
		if(isDead)
		{
			gameObject.renderer.enabled = false;
		}
		else if(notSet == false)
		{
			gameObject.renderer.enabled = true;
			notSet = true;
		}
	}
	

	public IEnumerator MoveFromTo(Vector3 pointA, Vector3 pointB, float time){
		if (!moving)
		{ 
			//Do nothing if already moving
			moving = true; 
			float t = 0f;
			while (t < 1f)
			{
				//Sweeps from 0 to 1 in time seconds
				t += Time.deltaTime / time; 
				//Set position proportional to t
				myTransform.position = Vector3.Lerp(pointA, pointB, t); 
				//Leave the routine and return here in the next frame
				yield return 0; 
			}
			//Finished moving
			moving = false; 
		}
	}
	
	public void setIsDead(bool b)
	{
		isDead = b;	
	}
}
