using UnityEngine;

using System.Collections;


public class DragDetectorScript : MonoBehaviour
{

 

	// Values to set:
	public float comfortZone = 70.0f;
	public float minDragDist = 14.0f;
	public float maxDragTime = 0.5f;
	
	private float startTime;
	private Vector2 startPos;
	private bool couldBeDrag;
	private RunnerScript player;

    
	//directions for drag
	public enum DragDirection
	{

		None,

		Up,

		Down

	}
    
	//instantiate lastDrag
	public DragDirection lastDrag = DragDetectorScript.DragDirection.None;
	
	//time drag took to finish
	public float lastDragTime;
	
	
	void Start()
	{
		//If not title screen
		if (Application.loadedLevel != 0)
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerScript>();	
	}
	
	//Input used in Update vs FixedUpdate
	void  Update ()
	{

		if (Input.touchCount > 0) {

			Touch touch = Input.touches [0];

        

			switch (touch.phase) {
			//if touch began
			case TouchPhase.Began:
				couldBeDrag = true;
				lastDrag = DragDetectorScript.DragDirection.None;

				lastDragTime = 0;

				couldBeDrag = true;

				startPos = touch.position;

				startTime = Time.time;

				break;

                
			//if touch moved
			case TouchPhase.Moved:
				if(couldBeDrag)
				{		
					float dragDist = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
					
					//took out comfortZone
//					if (Mathf.Abs (touch.position.x - startPos.x) > comfortZone) {
//	
//						Debug.Log ("Not a swipe. Swipe strayed " + (int)Mathf.Abs (touch.position.x - startPos.x) + 
//	
//	                                  "px which is " + (int)(Mathf.Abs (touch.position.x - startPos.x) - comfortZone) + 
//	
//	                                  "px outside the comfort zone.");
//	
//						couldBeDrag = false;
//	
//					}
//					else if (dragDist > minDragDist)
					if(dragDist > minDragDist)
					{
						float dragValue = Mathf.Sign (touch.position.y - startPos.y);
						if(dragValue > 0)
						{
							if(lastDrag == DragDirection.Down || lastDrag == DragDirection.None)
							{
								lastDrag = DragDetectorScript.DragDirection.Up;
								player.setGravityUp(true);
								player.setGravityButton(true);
							}
							
						}
						else if(dragValue < 0)
						{
							if(lastDrag == DragDirection.Up || lastDrag == DragDirection.None)
							{
								lastDrag = DragDetectorScript.DragDirection.Down;
								player.setGravityUp(false);
								player.setGravityButton(true);
							}
						}
						startPos = touch.position;
					}
				}

				break;
				
			case TouchPhase.Stationary:
				if(couldBeDrag)
				{					
					startPos = touch.position;	
				}
				break;
				
			//if touch ended
			case TouchPhase.Ended:

				if (couldBeDrag) {

					float swipeTime = Time.time - startTime;

					float dragDist = (new Vector3 (0, touch.position.y, 0) - new Vector3 (0, startPos.y, 0)).magnitude;

                    

					if ((swipeTime < maxDragTime) && (dragDist > minDragDist)) {

						// It's a swiiiiiiiiiiiipe!

						//float dragValue = Mathf.Sign (touch.position.y - startPos.y);

                        

						// If the swipe direction is positive, it was an upward swipe.

						// If the swipe direction is negative, it was a downward swipe.

//						if (dragValue > 0)
//
//							lastDrag = DragDetectorScript.DragDirection.Up;
//						else if (dragValue < 0)
//
//							lastDrag = DragDetectorScript.DragDirection.Down;
//						if(dragValue > 0)
//						{
//							lastDrag = DragDetectorScript.DragDirection.Up;
//							player.setGravityUp(true);
//							player.setGravityButton(true);
//							
//						}
//						else if(dragValue < 0)
//						{
//							lastDrag = DragDetectorScript.DragDirection.Down;
//							player.setGravityUp(false);
//							player.setGravityButton(true);
//						}
                        

						// Set the time the last swipe occured, useful for other scripts to check:

						lastDragTime = Time.time;

						//Debug.Log("Found a swipe!  Direction: " + lastDrag);

					}

				}

				break;

			}

		}

	}

}