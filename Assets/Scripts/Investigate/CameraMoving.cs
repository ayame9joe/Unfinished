using UnityEngine;
using System.Collections;

public class CameraMoving : MonoBehaviour {

	
	public UIPanel pnMove;
	public UISprite leftArrow;
	public UISprite rightArrow;
	
	bool check4Once = false;
	
	bool moveRight = false;
	bool moveLeft = false;
	// Use this for initialization
	void Start () {
		pnMove.enabled = false;
		
		//leftArrow.transform.localPosition= new Vector3( -300 , 0, 0 );
		//rightArrow.transform.localPosition = new Vector3( 300  , 0, 0 );
	}
	
	// Update is called once per frame
	void Update () {
		
		if (pnMove.enabled)
		{

			if (moveRight)
			{
				Debug.Log("右移");
				GameObject.Find("Bg").transform.Translate(-10f, 0, 0);
			}
			if (moveLeft)
			{
				GameObject.Find("Bg").transform.Translate(10f, 0, 0);
			}
		}
		else
		{

		}
		
		
		if (//StoryController.leisureTime && 
			(Input.mousePosition.x < 200 ||
			Input.mousePosition.x > 600))
		{
			
			pnMove.enabled = true;
		}
		else { pnMove.enabled = false; }
		
		
		

	}
	
	void OnRightArrowPress ()
	{
		
		

		moveRight = true;
		
	}
	
	void OnRightArrowRelease ()
	{
		
		moveRight = false;
		Check4Continue();
	}
	
	void OnLeftArrowPress ()
	{
		
		moveLeft = true;
		
	}
	
	void OnLeftArrowRelease ()
	{
		
		moveLeft = false;
		Check4Continue();
	}
	
	void Check4Continue ()
	{

		if (!check4Once)
		{
			check4Once = true;
			StoryController.eventID = 2;
			StoryController.eventChange = true;
			StoryController.leisureTime = false;
		}
	}
}
