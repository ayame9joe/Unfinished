using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour {
	
	tk2dSprite es;
	public static bool chosen = false;
	// Use this for initialization
	void Start () {
		es = GetComponent<tk2dSprite>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseUp ()
	{
		Debug.Log("chosen");
		//es.transform.localPosition()
	}
}
