using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
	
	
	public tk2dSprite UI;
	public tk2dSprite UI1;
	public tk2dSprite UI2;
	public tk2dSprite UI3;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Mouse Down");
//			UI.enabled = false;
			UI.renderer.enabled = false;
			UI1.renderer.enabled = true;
			UI2.renderer.enabled = false;
			UI3.renderer.enabled = false;
		}
		
		if (UI1.renderer.enabled)
		{
			if (Input.GetKeyDown(KeyCode.KeypadEnter))
			{
			//	Debug.Log("开始游戏");
			}
			if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				UI1.renderer.enabled = false;
				UI2.renderer.enabled = true;
				UI3.renderer.enabled = false;
			}
		}
		
		else if (UI2.renderer.enabled)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Debug.Log("About");
			}
			if (Input.GetKeyUp(KeyCode.LeftArrow))
			{
				UI1.renderer.enabled = true;
				UI2.renderer.enabled = false;
				UI3.renderer.enabled = false;
			}
			if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				UI1.renderer.enabled = false;
				UI2.renderer.enabled = false;
				UI3.renderer.enabled = true;
			}
		}
		
		else if (UI3.renderer.enabled)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Debug.Log("");
				Application.Quit();
			}
			if (Input.GetKeyUp(KeyCode.LeftArrow))
			{
				UI1.renderer.enabled = false;
				UI2.renderer.enabled = true;
				UI3.renderer.enabled = false;
			}
			if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				UI1.renderer.enabled = true;
				UI2.renderer.enabled = false;
				UI3.renderer.enabled = false;
			}
			
		}
	
	}
}
