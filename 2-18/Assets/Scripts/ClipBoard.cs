using UnityEngine;
using System.Collections;

/*
 * Class is responsible for the behavior of the clipBoard object that apears at the end of levels
 * Attached to the object ClipBoard
 */ 

public class ClipBoard : MonoBehaviour 
{
	private Transform myTransform;
	private Vector2 endPosition;
	
	void Start () 
	{
		myTransform = transform;
		endPosition = new Vector2(290.0f,myTransform.position.y);
		
		//Add to volume control
		if(PlayerPrefs.HasKey("volumeFX"))
		{
			if(GetComponent<AudioSource>() != null)
			{
				AudioSource audio = GetComponent<AudioSource>();
				audio.volume = (float)PlayerPrefs.GetInt("volumeFX")/10;
			}
		}
		
		//Begin Behavior
		StartCoroutine(MoveTo(endPosition, 0.5f));
	}
	
	void FixedUpdate () 
	{
	}
	
	//Clipboard movement behavior
	IEnumerator MoveTo(Vector2 endPosition, float time)
	{
	    Vector3 start = myTransform.position;
	    Vector3 end = endPosition;
	    float t = 0;

	    while(t < 1)
	    {
	        yield return null;
	        t += Time.deltaTime / time;
	        transform.position = Vector2.Lerp(start, end, t);
	    }
		
    	myTransform.position = end;
	}
}
