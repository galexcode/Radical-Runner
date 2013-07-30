using UnityEngine;
using System.Collections;

public class WarningBlink : MonoBehaviour {
	private bool turnOn;
	private float timer, timerLimit;
	// Use this for initialization
	void Start () {
		turnOn = true;
		timerLimit = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > timerLimit)
		{
			flipBack();
			timer = 0.0f;
		}
		
	}
	
	void flipBack()
	{
		turnOn = !turnOn;
		gameObject.renderer.enabled = turnOn;
	}
}
