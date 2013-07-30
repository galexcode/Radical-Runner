using UnityEngine;
using System.Collections;

public class HourGlassEffect : MonoBehaviour {
	
	private Transform myTransform;
	private GameObject runner;
	private int speed;
	private float scale;
	private bool fadeOut;
	
	// Use this for initialization
	void Start () 
	{
		runner = GameObject.FindGameObjectWithTag("Player");
		myTransform = transform;
		
		//Off set for effect
		if (runner.GetComponent<RunnerScript>().getFallSpeed() < 0)
			myTransform.position = new Vector3(runner.collider.bounds.max.x, runner.collider.bounds.min.y, runner.transform.position.z + 1);
		else if (runner.GetComponent<RunnerScript>().getFallSpeed() > 0)
			myTransform.position = new Vector3(runner.collider.bounds.max.x, runner.collider.bounds.max.y, runner.transform.position.z + 1);
		else
			myTransform.position = new Vector3(runner.collider.bounds.max.x, runner.transform.position.y, runner.transform.position.z + 1);
		
		speed = 250;
		scale = myTransform.localScale.x;
		fadeOut = true;
		
		StartCoroutine(FadeTimer());
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//On spawn move away from player
		if (fadeOut)
		{
			if (tag == "Glass1")
			{
				myTransform.Translate( (new Vector3(-1, 1, 0) * Time.deltaTime * speed), Space.World);
				audio.Play();	
			}
			if (tag == "Glass2")
				myTransform.Translate( (new Vector3(-1, -1, 0) * Time.deltaTime * speed), Space.World);
			if (tag == "Glass3")
				myTransform.Translate( (new Vector3(1, 1, 0) * Time.deltaTime * speed), Space.World);
			if (tag == "Glass0")
				myTransform.Translate( (new Vector3(1, -1, 0) * Time.deltaTime * speed), Space.World);
		
		
			myTransform.Rotate( (new Vector3(0,0,360) * Time.deltaTime));	
		
			if (myTransform.localScale.x <= 2)
			{
				myTransform.localScale = new Vector3(scale, scale, myTransform.localScale.z);
				scale += 0.01f;
			}
		
		}
	
		//After time, move towards player
		else
		{
			myTransform.position = Vector3.MoveTowards(myTransform.position, runner.transform.position, speed * 10.0f * Time.deltaTime);
			myTransform.Rotate( (new Vector3(0,0,-360) * Time.deltaTime));	
		
			if (myTransform.localScale.x > 0.2f)
			{
				myTransform.localScale = new Vector3(scale, scale, myTransform.localScale.z);
				scale -= 0.05f;
			}
		}
	}
	
	public IEnumerator FadeTimer()
	{
		
		yield return new WaitForSeconds(2.5f);
		
		//Reset positions
		if (tag == "Glass1")
			myTransform.position = new Vector3(runner.transform.position.x -700, runner.transform.position.y + 700, runner.transform.position.z + 1);
		if (tag == "Glass2")
			myTransform.position = new Vector3(runner.transform.position.x - 700, runner.transform.position.y - 700, runner.transform.position.z + 1);
		if (tag == "Glass3")
			myTransform.position = new Vector3(runner.transform.position.x + 700, runner.transform.position.y + 700, runner.transform.position.z + 1);
		if (tag == "Glass0")
			myTransform.position = new Vector3(runner.transform.position.x + 700, runner.transform.position.y - 700, runner.transform.position.z + 1);
		
		//Change direction
		fadeOut = false;
		
		//Destroy after effect finishes
		yield return new WaitForSeconds(0.4f);
		Destroy(gameObject);
	}
}
