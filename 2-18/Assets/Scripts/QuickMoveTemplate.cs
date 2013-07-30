using UnityEngine;
using System.Collections;

/*
 * Template class for moving an object
 */

public class QuickMoveTemplate : MonoBehaviour 
{
	private Transform myTransform;
	
	void Start () 
	{
	    myTransform = transform;
	}
	
	void FixedUpdate () 
	{
		myTransform.Translate(new Vector2(-19.0f, 0.0f)  * Time.deltaTime);
	}
}
