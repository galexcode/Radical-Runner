using UnityEngine;
using System.Collections;

/* 
 * Class controls the behavior of the arrows that allow the player to choose their button layout
 */

public class ButtonPositionScreenArrow : MonoBehaviour 
{
	public GameObject[] buttons;
	private Vector3[] buttonPositions;
	private int buttonSetIndex;
	private GameObject trueButton1,trueButton2,trueButton3;
	private exSpriteFont spriteFont;
	
	void Start () 
	{
		trueButton1 = GameObject.FindGameObjectWithTag("Intangibility");
		trueButton2 = GameObject.FindGameObjectWithTag("Hrglss");
		trueButton3 = GameObject.FindGameObjectWithTag("shrink");
		spriteFont = GameObject.FindGameObjectWithTag("PositionText").GetComponent<exSpriteFont>();
		buttonSetIndex = 1;
		
		spriteFont.text = buttonSetIndex.ToString() + " (Default)";
 	}

	void OnFingerDown()
	{
		if (tag == "RightArrow")
		{
			
			ButtonPositionSet(buttonSetIndex);	
			buttonSetIndex++;
			
			if (buttonSetIndex == 1)
				spriteFont.text = buttonSetIndex.ToString() + " (Default)";
			else
			{
				spriteFont.text = buttonSetIndex.ToString();
			}
			
			//Reset to default
			if (buttonSetIndex >= 8)
			{
				buttonSetIndex = 0;
			}
			
			
		}
		
	}
	
	private void ButtonPositionSet(int setNumber)
	{
		//Default, Left Row
		if (setNumber == 0 )
		{
			setIconPosition(-275,-245,-200,-245,-125,-245,
							-445,-275,-310,-275,-175,-275);
		}
		//Corner Left
		else if (setNumber == 1 )
		{
			setIconPosition(-275,-245,-200,-245,-275,-170,
							-445,-275,-310,-275,-445,-140);
		}
		//Left Column
		else if (setNumber == 2 )
		{
			setIconPosition(-275,-245,-275,-170,-275,-95,
							-445,-275,-445,-140,-445,-5);
		}
		//Bottom Center
		else if (setNumber == 3 )
		{
			setIconPosition(-75,-245,0,-245,75,-245,
							-135,-275,0,-275,135,-275);
		}
		//Top Center
		else if (setNumber == 4 )
		{
			setIconPosition(-75,45,0,45,75,45,
							-135,275,0,275,135,275);
		}
		//Right Row
		else if (setNumber == 5 )
		{
			setIconPosition(125,-245,200,-245,275,-245,
							215,-275,350,-275,485,-275);
		}
		else if (setNumber == 6 )
		{
			setIconPosition(275,-245,200,-245,275,-170,
							485,-140,350,-275,485,-275);
		}
		else if (setNumber == 7 )
		{
			setIconPosition(275,-95,275,-245,275,-170,
							485,-5,485,-140,485,-275);
		}
	
	}
	
	private void setIconPosition(int button1x,int button1y, int button2x, int button2y, int button3x, int button3y,
								int trueButton1x,int trueButton1y, int trueButton2x, int trueButton2y,int trueButton3x, int trueButton3y)					
		
	{
			buttons[0].transform.position = new Vector3(button1x, button1y, buttons[0].transform.position.z);
			buttons[1].transform.position = new Vector3(button2x, button2y, buttons[0].transform.position.z);
			buttons[2].transform.position = new Vector3(button3x, button3y, buttons[0].transform.position.z);
			
			trueButton1.transform.position = new Vector3(trueButton1x, trueButton1y, trueButton1.transform.position.z);
			trueButton2.transform.position = new Vector3(trueButton2x, trueButton2y, trueButton1.transform.position.z);
			trueButton3.transform.position = new Vector3(trueButton3x, trueButton3y, trueButton1.transform.position.z);
	}
}
	