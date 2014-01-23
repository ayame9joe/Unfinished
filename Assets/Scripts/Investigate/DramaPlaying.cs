using UnityEngine;
using System.Collections;
using System.Xml;


public class DramaPlaying : MonoBehaviour {
	
	// TODO: Add the move part
	// TODO: Other part of the menu
	// TODO: You can use the button to speed up showing the talk, but you can't use it to skip the talk.
	// TODO: Add the player postion.
	
	// NGUI Elements
	public UIPanel mPanelDrama;
	public UILabel mLabelName;
	public UILabel mLabelTalk;
	public UIPanel mPanelLowerMenu;
	public UIPanel mPanelItem;
	
	public UIPanel mPanelLeftandRight;
	
	// Talker Data
	string[] Name;
	
	// Talk Data
	int maxPiece;
	int curPiece;
	string[] TalkInfo;
	
	// Number to show the sequence of event
	int eventID;
	
	// Variables use for TypeWriter
	int mOffset = 0;
	float mNextChar = 0f;
	
	public int charsPerSecond = 40;
	
	void Awake () 
	{
		// Initialization for the variables
		maxPiece = 0;
		curPiece = 0;
		
		Name = null;
		TalkInfo = null;
		
		eventID = 0;
		
		// Hide the Panel for the beginning
		mPanelDrama.enabled = false;
		
		mPanelLeftandRight.enabled = false;
		
		
	}

	// Use this for initialization
	void Start () {

		LoadLevelData();
		
	}
	
	void LoadLevelData ()
	{
		XmlReader reader = XmlReader.Create("xmlExample.XML");
		
		while(reader.Read())
		{
			
			if (reader.IsStartElement("event") && 
				reader.GetAttribute("eventID") == eventID.ToString())
			{	
				
				maxPiece = int.Parse(reader.GetAttribute("entries"));
				
				TalkInfo = new string[maxPiece];
				Name = new string[maxPiece];
					
				for (curPiece = 0; curPiece < maxPiece; curPiece++)
				{
					reader.Read();
					if(reader.IsStartElement("speach"))
					{
						
						Name[curPiece] = reader.GetAttribute("name");
						TalkInfo[curPiece] = reader.ReadString();
					}
				}
				
				curPiece = 0;
				
			}			
		}
		
	}
	
	void CheckForContinue ()
	{
		
		if (GameObject.Find("Item").GetComponent<Item>().hasBeenListened)
		{
			eventID++;
			LoadLevelData();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		ShowingText();
		
		CheckForContinue();
		
		// Disable items interaction when drama playing
		DisableItemsInteraction();
		
		if (Input.mousePosition.x <= 30)
		{
			Debug.Log("Here we go!");
			mPanelLeftandRight.enabled = true;
		}
		else if (Input.mousePosition.x >= Screen.width - 30)
		{
			Debug.Log("Here we go again!");
			mPanelLeftandRight.enabled = true;
		}		
		else
		{
			mPanelLeftandRight.enabled = false;
		}
	
	}
	
	void ShowingText ()
	{
		
		if (mPanelDrama.enabled)
		{
			
			if (mOffset < TalkInfo[curPiece].Length)
			{
				if (mNextChar <= Time.time)
				{
					charsPerSecond = Mathf.Max(1, charsPerSecond);
	
					// Periods and end-of-line characters should pause for a longer time.
					float delay = 1f / charsPerSecond;
					char c = TalkInfo[curPiece][mOffset];
					if (c == '.' || c == '\n' || c == '!' || c == '?') delay *= 4f;
	
					mNextChar = Time.time + delay;
					mLabelTalk.text = TalkInfo[curPiece].Substring(0, ++mOffset);
					
				}
			}
			
			mLabelName.text = Name[curPiece];
		}
		
	}
	
	void DisableItemsInteraction ()
	{
		// TODO: Learning how to get a child object.
		GameObject[] items;
		items =	GameObject.FindGameObjectsWithTag("Item");
		foreach (GameObject item in items)
			if (mPanelDrama.enabled)
			{
				item.GetComponent<UIButtonMessage>().enabled = false;
			}
			else { item.GetComponent<UIButtonMessage>().enabled = true;}

	}
	
	void OnBtnContinue () {
		

		Debug.Log("Button Clicked!");
		mOffset = 0;
		curPiece++;
		
		if (curPiece >= maxPiece)
		{
			
			mPanelDrama.enabled = false;
			curPiece = 0;
		}
	}
	

	
	void OnBtnOption ()
	{
		if (mPanelLowerMenu.enabled)
		{
			mPanelLowerMenu.enabled = false;
		}
		else
		{
			mPanelLowerMenu.enabled = true;
		}
	}
	
	void OnBtnShowDialogue ()
	{
		
		mPanelDrama.enabled = true;
		
	}
	

	
	void OnBtnSpeak ()
	{
		mPanelDrama.enabled = true;
		curPiece = maxPiece - 1;
	}
	
}
