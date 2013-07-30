using UnityEngine;
using System.Collections;

/*
 * Class controls the behavior of the black border at the end of each level
 */ 
public class LastObject : MonoBehaviour 
{
	private Transform myTransform;
	
	void Start () 
	{
		myTransform = GetComponent<Transform>();
	}
	
	public IEnumerator Transition(Vector3 destination, float timeToFinish)
	{
    	float t = 0.0f;
    	Vector3 startingPos;
		if(destination.x >= 0.0f)
		{
			startingPos = new Vector3(1205.0f, 0.0f, 1.0f);
		}
		else
		{
			startingPos = new Vector3(0.0f, 0.0f, 1.0f);	
		}
   	 	while (t < 1.0f)
	    {
	        t += Time.deltaTime * (Time.timeScale/timeToFinish);
	        transform.position = Vector3.Lerp(startingPos, destination, t);
			yield return 0;
	    }		
	}
	
}
