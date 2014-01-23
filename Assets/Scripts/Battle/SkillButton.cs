using UnityEngine;
using System.Collections;

public class SkillButton : MonoBehaviour {
	
	public static string skillName;

	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnPress () 
	{
		BattleSystem.onUseSkill = true;
		Debug.Log(this.GetComponentInChildren<UILabel>().text);
		skillName = this.GetComponentInChildren<UILabel>().text;
		switch(this.GetComponentInChildren<UILabel>().text)
		{
		case "Attack1":
			break;
		case "Attack2":
			break;
			
		}
	}
}
