using UnityEngine;
using System.Collections;

public class BuzzScript : MonoBehaviour 
{
	public GameObject warning, curObject;
	
	private Transform myTransform;
	private SphereCollider myCollider;
	private float offsetY;
	private GameObject topBorder, bottomBorder;
	private exSprite flip;
	private float speedVar, timer;
	private SpawnObstacles spawner;
	private bool doOnce;
	
	void Start () 
	{
		myTransform = transform;
		spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>();
		myCollider = gameObject.GetComponent<SphereCollider>();
		topBorder = GameObject.FindGameObjectWithTag("Top");
		bottomBorder = GameObject.FindGameObjectWithTag("Bottom"); 
		doOnce = false;
		
		//Add to volume control
		if(PlayerPrefs.HasKey("volumeFX"))
		{
			if(GetComponent<AudioSource>() != null)
			{
				AudioSource audio = GetComponent<AudioSource>();
				audio.volume = (float)PlayerPrefs.GetInt("volumeFX")/10;
			}
		}
		
		flip = GetComponent<exSprite>();
		
		//Offsets will use opposite border max/min because this object has a spehere colider, not a box collider
		if(Random.Range(0, 100) > 50)
		{
			offsetY = (topBorder.collider.bounds.max.y - gameObject.collider.bounds.size.y/1.5f);
			flip.VFlip();
			myCollider.center = -myCollider.center;
			Destroy(myTransform.FindChild("Buzzparticlebot").gameObject);
			
		}
		
		else
		{
			offsetY = (bottomBorder.collider.bounds.min.y + gameObject.collider.bounds.size.y/1.5f);
			Destroy(myTransform.FindChild("Buzzparticletop").gameObject);
		}
		
		myTransform.position = new Vector3(myTransform.position.x, offsetY, 0.0f);

	}
	
	void FixedUpdate () 
	{
		//Play audio, once
		if (myTransform.position.x < bottomBorder.collider.bounds.max.x + 50.0f && audio.isPlaying == false)
			{
				audio.Play();
			}
		
		//Set object & object warning orientation (top or bottom)
		if(gameObject.collider.bounds.min.x > 575.0f)
		{
			if(!doOnce)
			{
				float y;
				if(offsetY > 0)
				{
					y = collider.bounds.min.y;
				} 
				else
				{
					y = collider.bounds.max.y;
				}
				
				curObject = Instantiate(warning, new Vector3(510.0f, y, 0.0f), Quaternion.identity) as  GameObject;
				
				doOnce = true;
			}	
		}
		
		else
		{
			if(doOnce)
			{
				Destroy(curObject);
				doOnce = false;
			}
		}
		
		myTransform.Translate(new Vector2(speedVar * -1.3f, 0.0f)  * Time.deltaTime);
		
		
		if(gameObject.collider.bounds.max.x < -575.0f)
		{	
			spawner.subtractNumObject();
			Destroy(this.gameObject);
		}
	}
	
	public void setSpeedVar(float s)
	{
		speedVar = s;	
	}
	
	public void DestroyThis()
	{
		Destroy(this.gameObject);
		if(GameObject.FindGameObjectWithTag("Arrow") != null)
		{
			WarningBlink[] warnings = GameObject.FindObjectsOfType(typeof(WarningBlink)) as WarningBlink[];
			foreach( WarningBlink warning in warnings)
			{
				Destroy(warning.gameObject);	
			}
		}
	}
}
