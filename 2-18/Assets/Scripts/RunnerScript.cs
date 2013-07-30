using UnityEngine;
using System.Collections;

/*
 * Main game class, controls the behavior of the player
 */ 
public class RunnerScript : MonoBehaviour 
{
	public enum State
	{
		respawn,
		inAir,
		onGround
	}

	public State myState;

	//X-Axis Variables - final build make private
	public float horiMoveSpeed;
	public float horiTopSpeed;
	public float horiFriction;
	public float horiAirFriction;
	public float horiGroundFriction;
	
	//Y-Axis Variables - final build make private
	public float fallSpeed;
	public float accel;
	
	//Gravity variables - final build make private
	public float gravity;
	public float gravVal;

	//Touch controls
	public bool gravityButton = false;
	public bool gravityUp = false;
	
	//Animation variables
	private bool vFlipped = false;
	private exSprite flip;
	
	//Particle system variables
	private ParticleSystem pSys;
	
	//Scoring variables
	private RunnerScoring score;
	
	//Camera effect variables
	private GameObject myCamera;
	private float shakeAmount;
	private RedSpriteFade fadeScreen;
	private bool fadedScreen, change1, change2, change3, change4, change5;
	
	//Cache & other variables
	private Transform myTransform;
	private float screenEdge;
	private RunnerCollision rCollision;
	private Vector3 startPosition;
	private TapDetector recognizer;
	
		
	//Acheivements
	private bool isCloseCall;
	
	void Awake()
	{
		#if UNITY_IPHONE && !UNITY_EDITOR
			if (iPhone.generation == iPhoneGeneration.iPhone4 || iPhone.generation == iPhoneGeneration.iPhone4S) 
				Application.targetFrameRate = 60;
			else
				Application.targetFrameRate = 30;
		#endif
	}

	void Start () 
	{
		//Object access
		pSys = GameObject.FindGameObjectWithTag("BackgroundEffect").GetComponent<ParticleSystem>();
		screenEdge = GameObject.FindGameObjectWithTag("Top").GetComponent<Collider>().bounds.min.x;
		score = GameObject.FindGameObjectWithTag("Score").GetComponent<RunnerScoring>();
		recognizer = GameObject.FindGameObjectWithTag("Recognizer").GetComponent<TapDetector>();
		rCollision = gameObject.GetComponent<RunnerCollision>();
		pSys = GameObject.FindGameObjectWithTag("BackgroundEffect").GetComponent<ParticleSystem>();
		fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen").GetComponent<RedSpriteFade>();
		myCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		myTransform = transform;
		
		//Start variables
		startPosition = myTransform.position;
		gravityButton = false;
		recognizer.setOnStart(true);
		setState (State.respawn);
		
		//Camera effects
		shakeAmount = 1.5f;
		fadeScreen.setRenderer(false);
			
			//Red UI frames
			change1 = false;
			change2 = false;
			change3 = false;
			change4 = false;
			change5 = false;
		
		//Animation variables
		flip = GetComponent<exSprite>();
	}
	
	//Key & touch-input access used in Update(), not FixedUpdate()
	void Update()
	{	
		checkButtons();	
	}
	
	void FixedUpdate()
	{
		CharacterMovement();
		ExecuteState();
		ApplyCameraShake();
		ApplyRedScreen();
	}
	
	#region getters/setters
	public float getGravVal()
	{
		return gravVal;	
	}
	public void setGravVal(float g)
	{
		gravVal = g;	
	}
	public void setGravityUp(bool u)
	{
		gravityUp = u;	
	}
	public bool getGravityUp()
	{
		return gravityUp;	
	}
	public float getHoriTopSpeed()
	{
		return horiTopSpeed;	
	}
	
	public void setHoriTopSpeed(float s)
	{
		horiTopSpeed = s;	
	}
	
	public void setGravityButton(bool s)
	{
		gravityButton = s;	
	}
	
	public float getHoriMoveSpeed()
	{
		return horiMoveSpeed;	
	}
	public void setHoriMoveSpeed(float s)
	{
		horiMoveSpeed = s;	
	}
	
	public bool getGravityButton()
	{
		return gravityButton;	
	}

	public void setState(State s)
	{
		myState = s;
	}
	
	public State getState()
	{
		return myState;
	}

	public void setFallSpeed(float s)
	{
		fallSpeed = s;
	}

	public float getGravity()
	{
		return gravity;
	}
	
	public float getFallSpeed()
	{
		return fallSpeed;
	}
	
	public void setGravity(float f)
	{
		gravity = f;	
	}

	public void setMyTransformPosition(Vector2 v)
	{
		myTransform.position = v;
	}
	#endregion
	
	void checkButtons()
	{
		//Developer/Web controls
		if(Input.GetKeyDown(KeyCode.Space))	
		{
			if (myState == State.onGround)
				setState(State.inAir);
			
			gravityUp = !gravityUp;
			
			ChangeGravity();
			
			if(gravityButton)
			gravityButton = false;
		}
		
		//iOS/Main controls
		if(gravityButton)
		{			
			if (myState == State.onGround)
				setState(State.inAir);
			
			ChangeGravity();
			
			if(gravityButton)
				gravityButton = false;
		}	
				
		if(Input.GetKey(KeyCode.R))
		{
			//setState(State.respawn);
			Application.LoadLevel(Application.loadedLevel);
		}
		
		
		//Temporary, slow down time
		if(Input.GetKey(KeyCode.N))
		{
			Time.timeScale = 0.2f;
		}
		if(Input.GetKey(KeyCode.M))
		{
			Time.timeScale = 1.0f;
		}
	}
	
	#region CharacterMovement
	void CharacterMovement()
	{
		if((int)myTransform.position.x == 0)
		{
			horiMoveSpeed = 0.0f;	
		}
		if(myTransform.position.x > 1)
		{
			horiMoveSpeed = -horiFriction * 10;
		}
		else if(myTransform.position.x < 0.0f)
		{
			if(horiMoveSpeed < horiTopSpeed)
			{
				horiMoveSpeed += horiFriction;
			}
			else
			{
				horiMoveSpeed = horiTopSpeed;	
			}
		} 
		
		//Move the player after checks
		myTransform.Translate(new Vector3(horiMoveSpeed, ApplyGravity(), 0.0f) * Time.fixedDeltaTime);
		
		//Kill you if player completely leaves screen
		if(myTransform.collider.bounds.max.x < screenEdge)
		{
			rCollision.setRespawn(true);	
		}	
	}
	
	#endregion
	
	#region gravityMethods
	private void ChangeGravity()
	{
		
		if(gravityUp)
		{
			gravVal = -gravity;
			if(!vFlipped)
			{
				vFlipped = !vFlipped;
				flip.VFlip();
			}
			
			
		}
		else
		{
			gravVal	= gravity;
			if(vFlipped)
			{
				vFlipped = !vFlipped;
				flip.VFlip();
			}
		}
		
		ChangeBackgroundParticles();
		
	}
	
	private float ApplyGravity()
	{
		//Skip falling calculations if grounded
		if (myState != State.onGround)
		{
			//Allow acceleration while falling
			if(fallSpeed > gravVal)
			{
				fallSpeed -= (gravity / -10.0f);	
			}
			else if(fallSpeed < gravVal)
			{
				fallSpeed += (gravity / -10.0f);
			}
			
			//If you are moving at max speed
			if( (int)fallSpeed == (int)(gravVal) )
			{
				fallSpeed = gravVal;
			}
			
			return fallSpeed;
		}
		
		else
		{
			return 0.0f;	
		}
	
	}
	
	#endregion
	
	#region exeState
	//Execute players behavior, based on state
	private void ExecuteState()
	{
		if (myState == State.respawn)
		{
			myTransform.position = startPosition;
			fallSpeed = 0.0f;
			gravityButton = true;
			
			//Reset animation
			if(vFlipped)
			{
				flip.VFlip();
				vFlipped = false;
			}

			setState(State.inAir);
		}

		if (myState == State.onGround)
		{
			fallSpeed = 0.0f;
			horiFriction = horiGroundFriction;
		}
		
		if (myState == State.inAir)
		{
			horiFriction = horiAirFriction;
		}
	}
	
	#endregion
	private void ChangeBackgroundParticles()
	{
		if (getGravVal() > 0.0f)	
				pSys.gravityModifier = -200.0f;
		else 
				pSys.gravityModifier = 200.0f;
	}
		
	void ApplyCameraShake()
	{
		
		if (myTransform.position.x <= screenEdge + 250.0f && myTransform.position.x > screenEdge + 150.0f)
		{
			myCamera.transform.localPosition = Random.insideUnitCircle * shakeAmount;
			myCamera.transform.localPosition = new Vector3(myCamera.transform.localPosition.x, myCamera.transform.localPosition.y, -100.0f);
		}
		
		if (myTransform.position.x <= screenEdge + 150.0f && myTransform.position.x > screenEdge)
		{
			myCamera.transform.localPosition = Random.insideUnitCircle * shakeAmount * 2.5f;
			myCamera.transform.localPosition = new Vector3(myCamera.transform.localPosition.x, myCamera.transform.localPosition.y, -100.0f);
		}
	}
	
	//Red screen effect when close to the edge of screen
	void ApplyRedScreen()
	{
		if(myTransform.position.x > screenEdge + 250.0f)
		{
			if(fadedScreen)
			{
				fadeScreen.setAtlasNum(0);
				fadeScreen.setRenderer(false);
				fadeScreen.setEnabled(false);
				fadedScreen = false;
				change1 = false;
			}
			
			//Track acheivement
			if (isCloseCall)
			{
				rCollision.setCloseCallEnd(true);
				isCloseCall = false;
			}
		}
		
		if(myTransform.position.x <= screenEdge + 250.0f && myTransform.position.x > screenEdge + 200.0f)
		{
			if(!change1)
			{
				change2 = false;
				fadedScreen = true;
				fadeScreen.setRenderer(true);
				fadeScreen.setAtlasNum(0);
				fadeScreen.setEnabled(true);
				change1 = true;
			}
		}
		
		if(myTransform.position.x <= screenEdge + 200.0f && myTransform.position.x > screenEdge + 150.0f)
		{
			if(!change2)
			{
				change1 = false;
				change3 = false;
				fadeScreen.setAtlasNum(1);
				fadeScreen.setEnabled(true);
				change2 = true;
			}
		}
		
		if(myTransform.position.x <= screenEdge + 150.0f && myTransform.position.x > screenEdge + 100.0f)
		{
			if(!change3)
			{
				change2 = false;
				change4 = false;
				fadeScreen.setAtlasNum(2);
				fadeScreen.setEnabled(true);
				change3 = true;
			}
		}
		
		if(myTransform.position.x <= screenEdge + 100.0f && myTransform.position.x > screenEdge + 50.0f)
		{
			if(!change4)
			{
				change3 = false;
				change5 = false;
				fadeScreen.setAtlasNum(3);
				fadeScreen.setEnabled(true);
				change4 = true;
				
				//Acheivement
				isCloseCall = true;
				rCollision.setCloseCallStart(true);
			}
		}
		
		if(myTransform.position.x <= screenEdge + 50.0f && myTransform.position.x > 0.0f)
		{
			if(!change5)
			{
				change4 = false;
				fadeScreen.setAtlasNum(4);
				fadeScreen.setEnabled(true);
				change5 = true;
			}
		}
	}
}
