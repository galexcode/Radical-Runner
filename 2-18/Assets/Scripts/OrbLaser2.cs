using UnityEngine;
using System.Collections;

/*
 * Class controls behavior of the red orb and laser
 * Animation for laser orb prefab/effect
 * Working order:
 * Process: (1) Orb moves out -> (2) Orb shakes, particle effects turned on -> (3) laser shoots, particles off, orb retreats
 * Code: (1)Default/Start() (2)allowOrbParticles (3)allowOrbLaser
 * 
 * Parent: Orb
 * Child: OrbParticles, OrbLaser
 */ 

public class OrbLaser2 : MonoBehaviour {
	
	private Transform myTransform;
	private bool allowOrbParticles,allowOrbLaser = false;
	private float screenEdgeR, screenEdgeOffset;
	private float shakeAmount;
	private Vector2 orbPosition;
	private SpawnObstacles mySpawner;
	private bool allowAudio;
	
	void Start () 
	{
		//Add to audio class
		if(PlayerPrefs.HasKey("volumeFX"))
		{
			if(GetComponent<AudioSource>() != null)
			{
				AudioSource audio = GetComponent<AudioSource>();
				audio.volume = (float)PlayerPrefs.GetInt("volumeFX")/10;
			}
		}
		
		
	    myTransform = transform;
		allowAudio = true;
		//Positioning
		screenEdgeOffset = gameObject.renderer.bounds.size.x/2;
		screenEdgeR = GameObject.FindGameObjectWithTag("Top").GetComponent("Transform").collider.bounds.max.x;
		mySpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>();
		
		//Only position parent
		if (tag == "Orb2")
		{
			myTransform.position = new Vector3(screenEdgeR +  screenEdgeOffset ,Random.Range(-295,295), -10);
			shakeAmount = 3.0f;
		}
		
		
		StartCoroutine(DestroyOverTime());
	}
	
	void FixedUpdate () 
	{
		//First part of animation, moving orb
		if ( allowOrbParticles == false && allowOrbLaser == false)
		{
			//Allow orb to move out
			if (tag == "Orb2")
			{
				if (myTransform.position.x > screenEdgeR - screenEdgeOffset/1.9f)
				{
					myTransform.Translate(new Vector2(-75.0f, 0.0f)  * Time.deltaTime, Space.World);
				}
				//Set up orb for second part of animatoin
				else
				{
					orbPosition = myTransform.position;
					allowOrbParticles = true;
				}
			}
			//If not an orb, chill out
			else
			{
				StartCoroutine(firstLaserDelay());
			}	
		}
		
		//Second part of animation, shake orb, turn on particles
		if (allowOrbParticles == true && allowOrbLaser == false)
		{
			if (tag == "Orb2")
			{
				if (audio.isPlaying == false)
					audio.Play();
				
				myTransform.position = new Vector2(orbPosition.x + (Random.Range(-shakeAmount,shakeAmount) ), orbPosition.y + (Random.Range(-shakeAmount,shakeAmount)) );
			}
			
			else if (tag == "OrbParticle")
			{
				particleSystem.enableEmission = true;	
			}
			

			
			StartCoroutine(secondLaserDelay());
				
		}
	
		if (allowOrbLaser == true && allowOrbLaser == true)
		{
			
			if (tag == "OrbLaser2"  || tag == "OrbLaser3")
			{	
				myTransform.Translate(new Vector2(-1000.0f, 0.0f)  * Time.deltaTime, Space.World);
			}
			
			else if (tag == "OrbParticle2")
			{
				particleSystem.enableEmission = false;
			}
			
			else if (tag == "Orb2")
			{
				myTransform.Translate(new Vector2(50.0f, 0.0f)  * Time.deltaTime, Space.World);
			}
		}	

	}
	
	private IEnumerator firstLaserDelay()
	{
		yield return new WaitForSeconds(1.3f);
		allowOrbParticles = true;
		
	}
	
	private IEnumerator secondLaserDelay()
	{
		yield return new WaitForSeconds(2.5f);
		
		//Layer 14, specifically for OrbLaser2 audio
		if (gameObject.layer == 14)
		{	
			if (allowAudio == true)
			{
				audio.Play();
				allowAudio = false;
			}
		}
		
		allowOrbLaser = true;
	}
	
	private IEnumerator DestroyOverTime()
	{
		yield return new WaitForSeconds(8.0f);
		if (tag == "Orb2")
		{
			mySpawner.subtractNumObject();
		}
		Destroy(gameObject);
	}	
}
