using UnityEngine;
using System.Collections;

/*
 * Class controls the screen fading when player comes close to the screen edge
 * Attached to RedScreenFade game object
 */

public class RedSpriteFade : MonoBehaviour 
{
	private exSpriteAnimation sprite;
	private int atlasNum;
	private bool allowFade;
	
	void Start () 
	{
		allowFade = false;
		sprite = GetComponent<exSpriteAnimation>();
	}
	
	void FixedUpdate () 
	{
		if(allowFade)
		{
			ChangeSprite();	
		}
		
	}
	
	public void setAtlasNum(int num)
	{
		atlasNum = num;	
	}
	
	public void setEnabled(bool b)
	{
		enabled = b;	
	}
	
	private void ChangeSprite()
	{
		sprite.SetFrame("fadeAnim", atlasNum);
		setEnabled(false);
	}
	
	public void setRenderer(bool b)
	{
		if(b)
		{
			gameObject.renderer.enabled = b;
		}
		else if(!b)
		{
			gameObject.renderer.enabled = b;	
		}
	}
	
}
