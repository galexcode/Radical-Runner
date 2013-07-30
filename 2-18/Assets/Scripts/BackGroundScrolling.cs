using UnityEngine;
using System.Collections;

/*
 * Class controlls the behavior of the background
 */

public class BackGroundScrolling : MonoBehaviour 
{
	public Material[] matArrayFront, matArrayBack;
	
	private int matIndexFront, matIndexBack;
	private int matIndex;
	private Transform myTransform;
	private float speedFromSpawner;

	
	void Start () 
	{
	    myTransform = transform;
		
		//Set scroll speed
		speedFromSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>().getSpeedVar();
		if (tag == "BackBackground")
		{
			matArrayFront = null;
			speedFromSpawner /= 8.0f;
		}
		else
		{
			matArrayBack = null;
			speedFromSpawner /= 4.0f;
		}
				
		matIndexBack = 1;
		matIndexFront = 1;
		matIndex = 1;
		
	}
	
	void FixedUpdate () 
	{
		myTransform.Translate(new Vector2(-speedFromSpawner, 0.0f)  * Time.deltaTime);
		
		if (myTransform.position.x < -625.0f)
		{
			myTransform.position = new Vector3(	615.0f, myTransform.position.y, 26.0f);
		}
	
		//Scroll through the colors, unused in iOS
		if (Input.GetKeyDown(KeyCode.U))
		{
			
		
			if (tag == "BackBackground")
			{
				
				if (matIndex >= matArrayBack.Length)
				{
					matIndex = 0;
					
				}
				
			renderer.material =  matArrayBack[matIndex];
			}
			
			
			else if (tag == "FrontBackground")
			{
				
				if (matIndex >= matArrayFront.Length)
				{
					matIndex = 0;
				}
				
			renderer.material = matArrayFront[matIndex];
				
			}
			
			matIndex++;
			matIndexFront++;
			matIndexBack++;
		}
	}
	
	public void setSpeedFromSpawner(float speed)
	{
		if (tag == "BackBackground")
		{
			speedFromSpawner = speed/8.0f;
		}
		else
		{
			speedFromSpawner = speed/4.0f;
		}
				
			
	}
	
	public void ChangeBackgroundColor()
	{
		if (tag == "BackBackground")
		{
			
			if (matIndex >= matArrayBack.Length)
			{
				matIndex = 0;
				
			}
			
		renderer.material =  matArrayBack[matIndex];
		}
		
		
		else if (tag == "FrontBackground")
		{
			
			if (matIndex >= matArrayFront.Length)
			{
				matIndex = 0;
			}
			
		renderer.material = matArrayFront[matIndex];
			
		}
		
		matIndex++;
	}
}
