using UnityEngine;
using System.Collections;

public class GameBegin : MonoBehaviour {
	public UISprite chosen1;
	public UISprite chosen2;
	// Use this for initialization
	void Start () {
			chosen1.enabled = false;
		chosen2.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick ()
	{
		Application.LoadLevel(3);
	}
	
	void OnHover ()
	{
		chosen1.enabled = true;
		chosen2.enabled = false;
	}
}
