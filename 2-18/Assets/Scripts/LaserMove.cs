using UnityEngine;
using System.Collections;

/*
 * Class controls the behavior or the lasers on the edge of the screen
 */

public class LaserMove : MonoBehaviour 
{
	private Transform myTransform;

	void Start () 
	{
	    myTransform = transform;
	}
	
	void FixedUpdate () 
	{
		myTransform.Translate(new Vector2(0.0f, -145.0f)  * Time.deltaTime);
		
		if (myTransform.position.y < -724.0f)
		{
			myTransform.position = new Vector2(myTransform.position.x, 731.0f);
		}
	
	}
}
