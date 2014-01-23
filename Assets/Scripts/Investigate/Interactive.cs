using UnityEngine;
using System.Collections;
using System.Xml;

public class Interactive : MonoBehaviour {
	

	public GameObject Twitter;
	private Texture2D mouseItemTexture;
	
	// Name for the item
	public string itemName;
	

	
	public UILabel lbTwitter1;
	public UILabel lbTwitter2;
	public UIPanel pnTwitter;


	
	
	
	int maxPiece;
	int curPiece;
	
	string[] info;
	string[] speach;
	string[] type;
	string[] name;
	
	bool hasSpeach = false;
	
	public bool hasBeenInvestigate = false;
	
	int paras = 0;
	
	bool check4Once = true;

	void Awake ()
	{
		
		
		pnTwitter.gameObject.SetActive(false);
		mouseItemTexture = Resources.Load ("Investigate", typeof(Texture2D)) as Texture2D;
		
		info = null;
		type = null;
		speach = null;
		name = null;
		
		maxPiece = 0;
		curPiece = 0;
		
	}
	
	// Use this for initialization
	void Start () {
		ReadInfo();
//		GameObject.Find("Equipment").SetActive(false);
		
	}
	
	void ReadInfo ()
	{
		
		switch(itemName)
		{
		case "门":
			maxPiece = 2;
			
			info = new string[maxPiece];
			type = new string[maxPiece];
			speach = new string[maxPiece];
			name = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					info[curPiece] = "如果不是有这个因缘，我就不能参加这个没有大肆宣传的试验。";
					lbTwitter1.text = info[curPiece];
				}
				else if (curPiece == 1)
				{
					info[curPiece] = "没有想到自己也有机会进入彩虹之城。之前一直只是远远地望着它。";
					lbTwitter2.text = info[curPiece];
				}
				
				Debug.Log("miumiu!");

			}
			curPiece = 0;
			break;
		case "沉潜设备":
			maxPiece = 2;
			
			info = new string[maxPiece];
			type = new string[maxPiece];
			speach = new string[maxPiece];
			name = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					info[curPiece] = "我把这些记录存储在这里应该是安全的，唯一的问题是如何重新获得。如果我不记得了，那么工作台处应该会有所发现。";
					lbTwitter1.text = info[curPiece];
				}
				else if (curPiece == 1)
				{
					info[curPiece] = "是的，这个工作室存在秘密，而答案就在人们的思想之中。";
					lbTwitter2.text = info[curPiece];
				}
				
				Debug.Log("miumiu!");

			}
			curPiece = 0;
			break;
		case "工作台":
			maxPiece = 2;
			
			info = new string[maxPiece];
			type = new string[maxPiece];
			speach = new string[maxPiece];
			name = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					info[curPiece] = "还是要仔细确认安全条款。虽然情感研究一直是我感兴趣的课题。";
					lbTwitter1.text = info[curPiece];
				}
				else if (curPiece == 1)
				{
					info[curPiece] = "我已经没有办法做什么了。毕竟，我只是一个工作人员。服从上级是我的使命。";
					lbTwitter2.text = info[curPiece];
				}
				
				Debug.Log("miumiu!");

			}
			curPiece = 0;
			break;
			
		}
		


	}
	
	// Update is called once per frame
	void Update () {


		
		
		

		
		


		
		
		if (Input.GetMouseButtonUp(0))
		{
		//	pnTwitter.gameObject.SetActive(false);
		}
		
		
	}
	

	
	void OnOver ()
	{

		Cursor.SetCursor(mouseItemTexture, Vector2.zero, CursorMode.Auto);
	}
	
	void OnOut ()
	{

		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
	
	void OnTouch ()
	{
		if (pnTwitter.gameObject.activeSelf)
		{
			pnTwitter.gameObject.SetActive(false);
		}
		else
		{
			pnTwitter.gameObject.SetActive(true);
		}
		
		if (check4Once)
		{
			check4Once = false;
			if (itemName == "门")
			{
				StoryController.eventID = 3;
				StoryController.eventChange = true;
			}

		}
		
		if (!pnTwitter.gameObject.activeSelf)
		{
			

			hasBeenInvestigate = true;
			if (itemName == "门")
			{
				StoryController.eventID = 4;
				StoryController.eventChange = true;
			}
			if (itemName == "沉潜设备")
			{
				StoryController.eventID = 5;
				StoryController.eventChange = true;
			}
						if (itemName == "工作台")
			{
				StoryController.eventID = 6;
				StoryController.eventChange = true;
			}
		}
		else
		{
			
			if (itemName == "altar")
			{
				StoryController.eventID = 3;
				StoryController.eventChange = true;
			}
			if (itemName == "altar1")
			{
				StoryController.eventID = 4;
				StoryController.eventChange = true;

			}
			if (itemName == "altar2")
			{
				StoryController.eventID = 5;
				StoryController.eventChange = true;
				//GameObject.Find("Equipment").SetActive(true);
				
			}
			if (StoryController.eventID == 4 && itemName == "altar1")
			{
				itemName = "altar2";
			}
			

		}
		
		

		
		
		

		
	}
	

	
	
}
