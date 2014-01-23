using UnityEngine;
using System.Collections;

public class RhythmModeOld : MonoBehaviour {
	
	
	public GameObject RhythmElement1Prefab;
	GameObject Instance4RhythmElement1;
	
	public GameObject RhythmElement2Prefab;
	GameObject Instance4RhythmElement2;
	
	public GameObject RhythmElement3Prefab;
	GameObject Instance4RhythmElement3;
	
	public GameObject RhythmElement4Prefab;
	GameObject Instance4RhythmElement4;
	
	
	
	
	//----
	int numRow = 9;
	int numCol = 9;
	public static int highlightenRow = 0;
	
	float width4RhythmElement = 0.2f;
	float height4RhythmElement = 0.2f;
	
	Vector3 hitPos = new Vector3(-1, 1, 0);
	
	//---
	public static int combo = 0;
	public static int color;
	public static int totalCombo = 0;
	int score = 0;
	public static bool check4Combo;
	int comboTime = 3;
	
	//---
	public UILabel lbComboNumber;
	public UISprite spCurColor;
	public UILabel lbScore;
	
	// Use this for initialization
	void Start () {
		
		for (int i = 0; i< numRow; i++){
		for (int j = 0; j < numCol; j++){
				int r = Random.Range(1, 5);
				switch(r)
				{
				case 1:
					Instance4RhythmElement1 = GameObject.Instantiate(RhythmElement1Prefab, new Vector3(0, 0, 1), transform.rotation) as GameObject;
					//Instance4RhythmElement1.transform.parent = GameObject.Find("RhythmElements").transform;
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
					//Instance4RhythmElement2.transform.parent = GameObject.Find("RhythmElements").transform;
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
					//Instance4RhythmElement3.transform.parent = GameObject.Find("RhythmElements").transform;
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
					//Instance4RhythmElement4.transform.parent = GameObject.Find("RhythmElements").transform;
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
		
		switch(color)
		{
		case 0:
			spCurColor.color = Color.blue;
			break;
		case 1:
			spCurColor.color = Color.green;
			break;
		case 2:
			spCurColor.color = Color.red;
			break;
		case 3:
			spCurColor.color = Color.yellow;
			break;
		}
		
		lbComboNumber.text = combo.ToString();
		lbScore.text = (totalCombo * 20).ToString();
	}
	
	void FixedUpdate ()
	{
		if (highlightenRow < numRow)
		{
			highlightenRow++;
		}
		else { highlightenRow = 0;}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Line" + highlightenRow.ToString()))
		{
			go.GetComponent<tk2dSprite>().color = Color.gray;
		}
		if (highlightenRow > 0)
		{
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Line" + (highlightenRow-1).ToString()))
			{
				go.GetComponent<tk2dSprite>().color = Color.white;
			}
		}
		
		if (check4Combo)
		{
			comboTime--;
		}
		
	}
	

}
