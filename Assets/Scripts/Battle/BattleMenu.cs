using UnityEngine;
using System.Collections;

public class BattleMenu : MonoBehaviour {
	
	
	public UISprite chosen1;
	public UISprite chosen2;
	public UISprite chosen3;
	// Use this for initialization
	void Start () {
		chosen1.enabled = false;
		chosen2.enabled = false;
		chosen3.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnHover ()
	{
		
	}
	
	void OnPress ()
	{
		
		chosen1.enabled = true;
		chosen2.enabled = false;
		chosen3.enabled = false;
		
	}
}
