using UnityEngine;
using System.Collections;

public class ClipBoardGrade : MonoBehaviour {
	
	private exSpriteFont spriteFont;
	private ClipBoardText clipBoardText;
	void Start () 
	{
		
		clipBoardText = GameObject.FindGameObjectWithTag("nextLevel").GetComponent<ClipBoardText>();
		spriteFont = GetComponent<exSpriteFont>();
		
		spriteFont.text = clipBoardText.getGrade();
		
	}
	
	
}
