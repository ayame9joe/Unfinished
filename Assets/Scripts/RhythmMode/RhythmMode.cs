using UnityEngine;
using System.Collections;

public class RhythmMode : MonoBehaviour {
	
	
	public GameObject RhythmElement1Prefab;
	GameObject Instance4RhythmElement1;
	
	public GameObject RhythmElement2Prefab;
	GameObject Instance4RhythmElement2;
	
	public GameObject RhythmElement3Prefab;
	GameObject Instance4RhythmElement3;
	
	public GameObject RhythmElement4Prefab;
	GameObject Instance4RhythmElement4;
	
	
	public UIPanel RhythmModePanel;
	public UIPanel InvestigatePanel;
	

	
	public AudioSource RhythmModeMusic;
	
	//----
	
	public static int evaluation = 0;
	int numRow = 9;
	int numCol = 13;
	public static int highlightenRow = -1;
	
	float width4RhythmElement = 0.175f;
	float height4RhythmElement = 0.15f;
	
	Vector3 hitPos = new Vector3(-1.1f, 0.70f, 0);
	//Vector3 hitPos = new Vector3(-1f, 1f, 0);
	//---
	public static int combo = 0;
	public static int color;
	public static int totalCombo = 0;
	public static int score = 0;
	int curScore;
	public static bool check4Combo;
	int comboTime = 3;
	
	//---
	public UILabel lbComboNumber;
	public UISprite spCurColor;
	public UILabel lbScore;
	public UISprite comboSprite;
	public UISprite curColor;
	
	bool upward = false;
	
	bool rhythmModeHasStarted = true;
	
	
	
	// Use this for initialization
	void Start () {
		//WaitForGameStart();
		
		Debug.Log("Combo Start");
		RhythmModeMusic.Play();
		for (int i = 0; i< numRow; i++){
		for (int j = 0; j < numCol; j++){
				int r = Random.Range(1, 5);
				switch(r)
				{
				case 1:
					Instance4RhythmElement1 = GameObject.Instantiate(RhythmElement1Prefab, new Vector3(0, 0, 1), transform.rotation) as GameObject;
					Instance4RhythmElement1.transform.parent = GameObject.Find("RhythmElements").transform;
					if (i % 2 == 0)
					{
						Instance4RhythmElement1.transform.localPosition = new Vector3(j * width4RhythmElement + width4RhythmElement / 2,
	                    -i * height4RhythmElement - height4RhythmElement / 2 , -1) + hitPos;
						
					}
					else
					{
						Instance4RhythmElement1.transform.localPosition = new Vector3(j * width4RhythmElement,
	                    -i * height4RhythmElement - height4RhythmElement / 2 , -1) + hitPos;
					}
					
					Instance4RhythmElement1.transform.localScale = new Vector3(1, 1, 1); 
					Instance4RhythmElement1.tag = "Line" + i.ToString();
					break;
				case 2:
					Instance4RhythmElement2 = GameObject.Instantiate(RhythmElement2Prefab, new Vector3(0, 0, 1), transform.rotation) as GameObject;
					Instance4RhythmElement2.transform.parent = GameObject.Find("RhythmElements").transform;
					if (i % 2 == 0)
					{
						Instance4RhythmElement2.transform.localPosition = new Vector3(j * width4RhythmElement + width4RhythmElement / 2,
	                    -i * height4RhythmElement - height4RhythmElement / 2 , -1) + hitPos;
						
					}
					else
					{
						Instance4RhythmElement2.transform.localPosition = new Vector3(j * width4RhythmElement,
	                    -i * height4RhythmElement - height4RhythmElement / 2 , -1) + hitPos;
					}
					
					Instance4RhythmElement2.transform.localScale = new Vector3(1, 1, 1); 
					Instance4RhythmElement2.tag = "Line" + i.ToString();
					break;
				case 3:
					Instance4RhythmElement3 = GameObject.Instantiate(RhythmElement3Prefab, new Vector3(0, 0, 1), transform.rotation) as GameObject;
					Instance4RhythmElement3.transform.parent = GameObject.Find("RhythmElements").transform;
					if (i % 2 == 0)
					{
						Instance4RhythmElement3.transform.localPosition = new Vector3(j * width4RhythmElement + width4RhythmElement / 2,
	                    -i * height4RhythmElement - height4RhythmElement / 2 , -1) + hitPos;
						
					}
					else
					{
						Instance4RhythmElement3.transform.localPosition = new Vector3(j * width4RhythmElement,
	                    -i * height4RhythmElement - height4RhythmElement / 2 , -1) + hitPos;
					}
					
					Instance4RhythmElement3.transform.localScale = new Vector3(1, 1, 1); 
					Instance4RhythmElement3.tag = "Line" + i.ToString();
					break;
				case 4:
					Instance4RhythmElement4 = GameObject.Instantiate(RhythmElement4Prefab, new Vector3(0, 0, 1), transform.rotation) as GameObject;
					Instance4RhythmElement4.transform.parent = GameObject.Find("RhythmElements").transform;
					if (i % 2 == 0)
					{
						Instance4RhythmElement4.transform.localPosition = new Vector3(j * width4RhythmElement + width4RhythmElement / 2,
	                    -i * height4RhythmElement - height4RhythmElement / 2 , -1) + hitPos;
						
					}
					else
					{
						Instance4RhythmElement4.transform.localPosition = new Vector3(j * width4RhythmElement,
	                    -i * height4RhythmElement - height4RhythmElement / 2 , -1) + hitPos;
					}
					
					Instance4RhythmElement4.transform.localScale = new Vector3(1, 1, 1); 
					Instance4RhythmElement4.tag = "Line" + i.ToString();
					break;
				}
				
				
				
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
//		GameObject.Find("Sprite4Evaluation").transform.localScale = new Vector3(score / 100, 0.2f, 1);
		switch(color)
		{
		case 1:
			curColor.GetComponent<UISprite>().spriteName = "节奏游戏UI-2a";
			curColor.transform.localScale = new Vector3(163, 45, 1);
			break;
		case 2:
			curColor.GetComponent<UISprite>().spriteName = "节奏游戏UI-2c";
			curColor.transform.localScale = new Vector3(163, 45, 1);
			break;
		case 3:
			curColor.GetComponent<UISprite>().spriteName = "节奏游戏UI-2d";
			curColor.transform.localScale = new Vector3(163, 45, 1);
			break;
		case 4:
			curColor.GetComponent<UISprite>().spriteName = "节奏游戏UI-2b";
			curColor.transform.localScale = new Vector3(163, 45, 1);
			break;
		}
		
//		lbComboNumber.text = combo.ToString();
//		lbScore.text = score.ToString();
		//if (highlightenRow == numRow){ highlightenRow = 0;}
	}
	
	void FixedUpdate ()
	{
		StartCoroutine("WaitForGameStart");
		//evaluation += height4RhythmElement/2;
		
		
		
		if (rhythmModeHasStarted)
		{
					

		//if (highlightenRow <= numRow && highlightenRow)
		{
		//	highlightenRow++;
		}
		if (highlightenRow <= 0){ 
			//向下
			upward = false;
		}
		else if (highlightenRow >= numRow -1)
		{
			//向上
			upward = true;
		}
			
		if (upward)
		{
			highlightenRow--;
		}
		else {highlightenRow++;}
		
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Line" + highlightenRow.ToString()))
		{
			if (go.GetComponent<tk2dSprite>().spriteId == 0)
			{
				go.GetComponent<tk2dSprite>().spriteId = 6;
			}
			else if (go.GetComponent<tk2dSprite>().spriteId == 2)
			{
				go.GetComponent<tk2dSprite>().spriteId = 4;
			} 
			else if (go.GetComponent<tk2dSprite>().spriteId == 7)
			{
				go.GetComponent<tk2dSprite>().spriteId = 3;
			}
			else if (go.GetComponent<tk2dSprite>().spriteId == 1)
			{
				go.GetComponent<tk2dSprite>().spriteId = 5;
			}
		}	
		if (upward)
		{
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Line" + (highlightenRow+1).ToString()))
			{
				if (go.GetComponent<tk2dSprite>().spriteId == 6)
				{
					go.GetComponent<tk2dSprite>().spriteId = 0;
				}
				else if (go.GetComponent<tk2dSprite>().spriteId == 4)
				{
					go.GetComponent<tk2dSprite>().spriteId = 2;
				} 
				else if (go.GetComponent<tk2dSprite>().spriteId == 3)
				{
					go.GetComponent<tk2dSprite>().spriteId = 7;
				}
				else if (go.GetComponent<tk2dSprite>().spriteId == 5)
				{
					go.GetComponent<tk2dSprite>().spriteId = 1;
				} 
			}
		}
		else
		{
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Line" + (highlightenRow-1).ToString()))
			{
				if (go.GetComponent<tk2dSprite>().spriteId == 6)
				{
					go.GetComponent<tk2dSprite>().spriteId = 0;
				}
				else if (go.GetComponent<tk2dSprite>().spriteId == 4)
				{
					go.GetComponent<tk2dSprite>().spriteId = 2;
				} 
				else if (go.GetComponent<tk2dSprite>().spriteId == 3)
				{
					go.GetComponent<tk2dSprite>().spriteId = 7;
				}
				else if (go.GetComponent<tk2dSprite>().spriteId == 5)
				{
					go.GetComponent<tk2dSprite>().spriteId = 1;
				} 
			}	
		}
		if (check4Combo)
		{
			comboTime--;
		}
		
		
		if ( (score - curScore) <= 0)
		{
			//score -= 20;
			curScore = score;
		}
		else { curScore = score;}
		//Debug.Log(curScore);
		Debug.Log(score);
			
		//comboSprite.transform.localScale = new Vector3(score, 67, 1);
		if (score > 500)
		{
			Debug.Log("完成");
			StoryController.eventID = 7;
			StoryController.eventChange = true;
			//StoryController.OnChangeStatus();
			RhythmModePanel.gameObject.SetActive(false);
			InvestigatePanel.gameObject.SetActive(true);
			DestroyElements();
			GameObject.Find("RhythmModeController").GetComponent<RhythmMode>().enabled = false;
		}
			

		}
		
		
	}
	
	IEnumerator WaitForGameStart ()
	{
		Debug.Log("Wait for Rhythm Game Start!");
		if (!rhythmModeHasStarted)
		{
			Debug.Log("Wait for Rhythm Game Start!");
			yield return new WaitForSeconds(3f);
			Debug.Log("Rhythm Game has Started!");
			
			rhythmModeHasStarted = true;
		}
	}
	
	public void DestroyElements ()
	{
		Destroy(GameObject.Find("RhythmElements"));
	}
	

}
