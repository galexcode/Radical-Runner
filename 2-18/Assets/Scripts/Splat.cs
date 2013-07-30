using UnityEngine;
using System.Collections;

/*
 * Class controls the visual effects of the ink splat & hand on the screen
 */

public class Splat: MonoBehaviour 
{
	private Transform myTransform;
	private int handSpeedV, handSpeedH, handPingPong;
	private bool allowMoveLeft, allowMoveRight;
	private GameObject topBorder;
	
	void Start () 
	{
		
		if(PlayerPrefs.HasKey("volumeFX"))
		{
			if(GetComponent<AudioSource>() != null)
			{
				AudioSource audio = GetComponent<AudioSource>();
				audio.volume = (float)PlayerPrefs.GetInt("volumeFX")/10;
			}
		}
		
		myTransform = transform;
		
		//Set splats
		if (tag != "SplatHand")
		{
			renderer.enabled = false;
			myTransform.position = new Vector3(Random.Range(-350,350), Random.Range(-150,150),-80);
			myTransform.localRotation = Quaternion.Euler(new Vector3(0,0,Random.Range(-360, 360)));
		}
		
		//Set splat hand
		else
		{
			myTransform.position = new Vector3(0, -700,-81);
			renderer.enabled = false;
			handSpeedV = 1000;
			handSpeedH = 0;
			handPingPong = 0;
			collider.enabled = false;
		}
			
		//Option to make splats appear on screen at different times , currently unused. 
		//Turns out if you do this, it actually just looks like the game is chopy, all set to 0 second delay.
		if (tag == "SE1")
			StartCoroutine(SplatEffect(0.0f));
		else if (tag == "SE2")
			StartCoroutine(SplatEffect(0.0f));
		else if (tag == "SE3")
			StartCoroutine(SplatEffect(0.0f));
		else if (tag == "SE4")
			StartCoroutine(SplatEffect(0.0f));
		else if (tag == "SplatHand")
			StartCoroutine(SplatEffect(2.0f));
			
		allowMoveLeft = true;
		allowMoveRight = false;
		topBorder = GameObject.FindGameObjectWithTag("Top");
		
	}
	
	void FixedUpdate () 
	{
		//Only apply to the hand, not the splat images
		if (tag == "SplatHand" && renderer.enabled == true)
		{
			//Final animation, send off the screen and destroy
			if (handPingPong >= 3)
			{
				myTransform.Translate(new Vector2(-handSpeedH, 0.0f) * Time.deltaTime, Space.World);
				
				if(collider.bounds.max.x < topBorder.collider.bounds.min.x)
				{
					Destroy(gameObject);
					for(int i = 1; i <= 4; i++)
					{
						string tag1 = "SE";
						tag1 = tag1 + i.ToString();
						if(GameObject.FindGameObjectsWithTag(tag1) != null)
						{
							GameObject[] splats = GameObject.FindGameObjectsWithTag(tag1);
							foreach(GameObject splat in splats)
							{
								Destroy(splat);	
							}
						}
					}
				}
			}
			
			//First movement, move up
			if (myTransform.position.y < 0)
			{
				myTransform.Translate(new Vector2(0.0f, handSpeedV)  * Time.deltaTime, Space.World);
				if (audio.isPlaying == false)
					audio.Play();
			}
			
			else 
			{
				//Set variables for second movement
				handSpeedH = 1300;
				collider.enabled = true;
			}
			
			//Second movement, left - right pingpong
			if (allowMoveLeft)
			{
				myTransform.Translate(new Vector2(-handSpeedH, 0.0f) * Time.deltaTime, Space.World);	
			}
		
			if (allowMoveRight)
			{
				myTransform.Translate(new Vector2(handSpeedH, 0.0f) * Time.deltaTime, Space.World);	
			}
			
			//Ping Pong transition
			if (myTransform.position.x < -140.0f && handPingPong < 3)
			{
				myTransform.position = new Vector3(-140.0f, 0,-81);
				allowMoveLeft = false;
				allowMoveRight = true;
			
			}
		
			else if (myTransform.position.x > 240.0f && handPingPong < 3)
			{
				myTransform.position = new Vector3(240.0f, 0,-81);
				allowMoveLeft = true;
				allowMoveRight = false;
				handPingPong ++;
			}
		}			
	}
	
	void OnTriggerEnter(Collider c)
	{
		//Splats
		if(c.gameObject.layer == 8 && gameObject.tag == "SplatHand")
		{	
			StartCoroutine(SplatEffectRemove(Random.Range(0.2f,1.5f), c.gameObject));
		}	
	}
	
	
	public IEnumerator SplatEffect(float SplatEffectNumber)
	{
		yield return new WaitForSeconds(SplatEffectNumber);
		renderer.enabled = true;
			
	}
	
	public IEnumerator SplatEffectRemove(float destroyTime, GameObject destroyObject)
	{
		yield return new WaitForSeconds(destroyTime);
		Destroy(destroyObject);
		
			
	}

}
