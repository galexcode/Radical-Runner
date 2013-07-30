using UnityEngine;
using System.Collections;

/*
 * Class controls the behavior of the Ink Blob power down
 */

public class InkBlob : MonoBehaviour 
{
	public float speed, turn;
	
	private float distance;
	private Transform myTransform;
	private Bounds b1, b2;
	private SpawnObstacles spawner;
	private float timer;

	void Start () 
	{
		myTransform = transform;
		spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnObstacles>();
		
		//Change y pos
		myTransform.position = new Vector3(1205.0f, Random.Range(-300.0f, 300.0f), myTransform.position.z);
	}
	
	void Update () 
	{
		myTransform.Translate(new Vector2(-450.0f, 0.0f)  * Time.deltaTime, Space.World);
        myTransform.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * 100);
			
		if(gameObject.collider.bounds.max.x < -575.0f)
		{	
			spawner.subtractNumObject();
			DestroyThis();
		}
	}
	
	public void DestroyThis()
	{
		Destroy(this.gameObject);
	}
	

}
