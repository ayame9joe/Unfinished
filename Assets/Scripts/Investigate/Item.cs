using UnityEngine;
using System.Collections;
using System.Xml;

public class Item : MonoBehaviour {
	
	//TODO: Put the xml folder to the right place.
	// TODO: Requirements for ...
	
	
	private Texture2D mouseNPCTexture;

	// Name for the item
	public string itemName;
	
	// NGUI Elements
	public UIPanel mPanelMenu;
	public UIPanel mPanelDrama;
	public UILabel mLabelTalk;
	public UIPanel mPanelPooster;
	public UILabel mLabelPooster;
	public UILabel mLabelName;
	public GameObject mButtonListen;
	public GameObject mButtonInvestigate;
	public GameObject mButtonInteract;
	
	public UISprite mSpriteLightUp;
	
	// It's up to the level designer to choose whether or not this item has these functions.
	public bool canBeListened;
	public bool canBeInteracted;
	public bool canBeInvestigated;
	
	// For the DarmaPlaying to load to check for the continue.
	[HideInInspector] 
	public bool hasBeenListened;
	[HideInInspector] 
	public bool hasBeenInteracted;
	[HideInInspector] 
	public bool hasBeenInvestigated;
	
	// Info Data
	int maxPieceForListening;
	int curPieceForListening;
	
	int maxPieceForInvestigating;
	int curPieceForInvestigating;
	
	int maxPieceForInteract;
	int curPieceForInteract;
	
	string[] listeningInfo;
	string[] investigateInfo;
	string[] interactInfo;
	
	// Interact or Investigate
	bool isInvestigating;
	
	GameObject myCamera;
	

	
	void Awake ()
	{
		// Initialization for the variables
		maxPieceForListening = 0;
		curPieceForListening = 0;
		
		maxPieceForInvestigating = 0;
		curPieceForInvestigating = 0;
		
		maxPieceForInteract = 0;
		curPieceForInteract = 0;
		
		listeningInfo = null;
		investigateInfo = null;
		interactInfo = null;
		
		// Hide the panels before doing anything.
		mPanelMenu.enabled = false;
		mPanelPooster.enabled = false;
		
		// Show the name of the item until we have the drawing.
		mLabelName.text = itemName;
		
		// Make sure the boolen are false until player do something.
		hasBeenListened = false;
		hasBeenInvestigated = false;
		hasBeenInteracted = false;
		
		
		CheckForInteractType();
		
		myCamera = GameObject.FindGameObjectWithTag("MainCamera");
		
		mouseNPCTexture = Resources.Load ("UI_13", typeof(Texture2D)) as Texture2D;
		
	}
	
	void CheckForInteractType ()
	{
		if (canBeListened)
		{
			mButtonListen.SetActive(true);
		}
		else mButtonListen.SetActive(false);
		
		if (canBeInvestigated)
		{
			mButtonInvestigate.SetActive(true);
		}
		else mButtonInvestigate.SetActive(false);
		
		if (canBeInteracted)
		{
			mButtonInteract.SetActive(true);
		}
		else mButtonInteract.SetActive(false);
	}
	
	void Start ()
	{
		ReadItemInfo();
		//myCamera.transform.LookAt(this.transform.position);
		
		
	}
	
	void ReadItemInfo ()
	{
		XmlReader reader = XmlReader.Create("xmlExample.XML");
		
		while(reader.Read())
		{
			
			if (reader.IsStartElement("item") && 
				reader.GetAttribute("name") == itemName )
			{	
		
				maxPieceForListening = int.Parse(reader.GetAttribute("listeningParas"));
				listeningInfo = new string[maxPieceForListening];
				
				if (canBeInvestigated)
				{
					maxPieceForInvestigating = int.Parse(reader.GetAttribute("investigateParas"));
					investigateInfo = new string[maxPieceForInvestigating];
				}
				
				maxPieceForInteract = int.Parse(reader.GetAttribute("interactParas"));
				interactInfo = new string[maxPieceForInteract];
					
				for (curPieceForListening = 0; curPieceForListening < maxPieceForListening; curPieceForListening++)
				{
					reader.Read();
					if(reader.IsStartElement("listeningInfo"))
					{
						listeningInfo[curPieceForListening] = reader.ReadString();	
					}
				}
				
				for (curPieceForInvestigating = 0; curPieceForInvestigating < maxPieceForInvestigating; curPieceForInvestigating++)
				{
					reader.Read();
					if(reader.IsStartElement("investigateInfo"))
					{
						investigateInfo[curPieceForInvestigating] = reader.ReadString();	
					}
				}
				
				for (curPieceForInteract = 0; curPieceForInteract < maxPieceForInteract; curPieceForInteract++)
				{
					reader.Read();
					if(reader.IsStartElement("interactInfo"))
					{
						interactInfo[curPieceForInteract] = reader.ReadString();	
					}
				}
					
				curPieceForListening = 0;
				curPieceForInvestigating = 0;
				curPieceForInteract = 0;

			}
		}
	}
	
	void Update ()
	{
		mLabelPooster.text = listeningInfo[curPieceForListening];
		if (isInvestigating)
		{
			mLabelTalk.text = investigateInfo[curPieceForInvestigating];
		}
		else {
			mLabelTalk.text = interactInfo[curPieceForInteract];
		}
	}
	
	void OnBtnPoosterContinue ()
	{
		curPieceForListening++;
		if (curPieceForListening >= maxPieceForListening)
		{
			mPanelPooster.enabled = false;
			curPieceForListening = 0;
		}
		
	}
	
	void OnSpriteTouched ()
	{
		mPanelMenu.enabled = true;
		//mButtonListen.SetActive(false);
		
		
		Cursor.SetCursor(mouseNPCTexture, Vector2.zero, CursorMode.Auto);
		
	}
	
	void OnSpriteLeave ()
	{
		mPanelMenu.enabled = false;
		
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
	
	void OnBtnListening ()
	{
		mPanelPooster.enabled = true;
		hasBeenListened = true;
	}
	
	void OnBtnInvestigate ()
	{
		mPanelDrama.enabled = true;
		hasBeenInvestigated = true;
		isInvestigating = true;
	}
	
	void OnBtnInteract ()
	{
		mPanelDrama.enabled = true;
		hasBeenInteracted = true;
		isInvestigating = false;
	}
	
	void OnTalkContinue ()
	{
		if (isInvestigating)
		{
			curPieceForInvestigating++;
			if (curPieceForInvestigating >= maxPieceForInvestigating)
			{
				mPanelDrama.enabled = false;
				curPieceForInvestigating = 0;
			}
		}
		else
		{
			curPieceForInteract++;
			if (curPieceForInteract >= maxPieceForInteract)
			{
				mPanelDrama.enabled = false;
				curPieceForInteract = 0;
			}
		}
		
	}
}
