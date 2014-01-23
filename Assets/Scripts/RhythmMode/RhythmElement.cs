using UnityEngine;
using System.Collections;

public class RhythmElement : MonoBehaviour {
	
	//public GameObject re;
	


	
	tk2dSprite re;
	bool hasBeenRomoved = false;
	int r;
	
	
	
	// Use this for initialization
	void Start () {
		
		re = GetComponent<tk2dSprite>();
		

	}
	
	// Update is called once per frame
	void Update () {
		
		if (hasBeenRomoved)
		{
				hasBeenRomoved = false;
				switch(r)
				{
				case 1:
					// Blue;
					re.spriteId = 10;

					break;
				case 2:
					// Green;
					re.spriteId = 4;
					
					break;
				case 3:
					// Yellow;
					re.spriteId = 5;
					
					break;
				case 4:
					// Red;
					re.spriteId = 7;
					
					break;
				}
		}
	}
	
	void OnMouseUp ()
	{
		r = Random.Range(1, 6);
		Debug.Log(r);
		
		
		Debug.Log("I'm touched!");
	
		if (this.gameObject.tag == "Line" + RhythmMode.highlightenRow)
		{
			foreach (AudioSource rs in this.gameObject.GetComponents<AudioSource>())
		{
			switch(r)
			{
			case 1:
				if (rs.clip.name == "节奏3")
				{
					rs.Play();
				}
				break;
			case 2:
				if (rs.clip.name == "节奏2")
				{
					rs.Play();
				}
				break;
			case 3:
				if (rs.clip.name == "节奏6")
				{
					rs.Play();
				}
				break;
			case 4:
				if (rs.clip.name == "节奏4")
				{
					rs.Play();
				}
				break;
			case 5:
				if (rs.clip.name == "节奏5")
				{
					rs.Play();
				}
				break;
			}
		}
			if (RhythmMode.evaluation % 0.08f == 0)
			{
				Debug.Log("Perfect");
			}
			if (re.spriteId == 5)
			{
				RhythmMode.score += 100;
			}
			else
			{
				RhythmMode.score -= 50;
			}
			
			if (re.spriteId == 6)
			{
				RhythmMode.color = 1;
			}
			else if (re.spriteId == 4)
			{
				RhythmMode.color = 2;
			}
			else if (re.spriteId == 3)
			{
				RhythmMode.color = 3;
			}
			else if (re.spriteId == 5)
			{
				RhythmMode.color = 4;
			}
			
			
			
			hasBeenRomoved = true;
				
		}
		
		
	}
	

	

	
	
	

	
	
}
