using UnityEngine;
using System.Collections;
using System.Xml;

public class DramaPlay : MonoBehaviour {
	
	
	bool check;
	public UIPanel TalkPanel;
	public UILabel lbTalk;
	public UILabel lbName;
	public UILabel lbTutorial;
	public UISprite ChangeStatus;
	
	
	//--- Modes Panels---
	public UIPanel RhythmMode;
	public UIPanel InvestigateMode;
	public UIPanel BattleSystem;
	
	public GameObject PreBattleMode;
	
	public static bool checkForContinue = false;
	
	public static bool leisureTime = false;
	
	bool speedUpDialogue = false;
	
	
	// Talker Data
	string[] Name;
	string[] Expression;
	string[] Type;
	
	// Talk Data
	public static int maxPiece;
	public static int curPiece;
	string[] TalkInfo;
	
	// Number to show the sequence of event
	public static int eventID;
	public static bool eventChange;
	
	// Variables use for TypeWriter
	int mOffset = 0;
	float mNextChar = 0f;
	
	public int charsPerSecond = 40;
	private WWW _statusFile;

            
	void Awake () 
	{
		Screen.SetResolution (1280, 720, true, 60);
//		GameObject.Find("BattleSystemController").GetComponent<BattleDemo>().enabled = false;
		RhythmMode.gameObject.SetActive(false);
		// Initialization for the variables
		maxPiece = 0;
		curPiece = 0;
		
		TalkInfo = null;
		Name = null;
		Expression = null;
		Type = null;
		
		eventID = 0;
		
		// Hide the Panel for the beginning
		//TalkPanel.enabled = false;
		
		ClearExps();

		
		
		
		
	}
	
	void ClearExps ()
	{
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("诺诺"))
		{
			go.GetComponent<UISprite>().enabled = false;
		}
		
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("K"))
		{
			go.GetComponent<UISprite>().enabled = false;
		}
	}

	// Use this for initialization
	void Start () {

		LoadLevelData();
		StartCoroutine(ReadIn());
	}
	
	
	IEnumerator ReadIn ()
	{
		WWW www = new WWW("https://docs.google.com/file/d/0B9OW0rcDNPTbOEI4XzQ3YkRJc2c/edit?usp=sharing");
        while (!www.isDone)
        {
            Debug.Log("Getting web time");
            yield return www;
            Debug.Log("Finish getting web time and whole xml is :   " + www.text);
            ParseXml(www);
        }
	}
	
	void ParseXml(WWW www)
	{
		XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(www.text);
        XmlElement root = xmlDoc.DocumentElement;
		Debug.Log(www.text);
		XmlNodeList nodeList = root.SelectNodes("/events/event");
        foreach (XmlElement xe in nodeList)
        {
            foreach (XmlElement x1 in xe.ChildNodes)
            {
                if (x1.Name == "speach")
                    Debug.Log("Current year:      " + x1.InnerText);
				lbTalk.text = x1.InnerText;
                if (x1.Name == "exp")
                    Debug.Log("Current month:      " + x1.InnerText);
                if (x1.Name == "type")
                    Debug.Log("Current day:      " + x1.InnerText);
                if (x1.Name == "hour")
                    Debug.Log("Current hour:      " + x1.InnerText);
                if (x1.Name == "minite")
                    Debug.Log("Current minite:      " + x1.InnerText);
                if (x1.Name == "second")
                    Debug.Log("Current second:      " + x1.InnerText);
            }
        }
	}
	
	void LoadLevelData ()
	{
		_statusFile = new WWW(Application.dataPath + "\\xmlExample.XML");
		XmlReader reader = XmlReader.Create(Application.dataPath + "\\xmlExample.XML");
		while(reader.Read())
		{
			
			if (reader.IsStartElement("event") && 
				reader.GetAttribute("eventID") == eventID.ToString())
			{	
				
				maxPiece = int.Parse(reader.GetAttribute("entries"));
				
				TalkInfo = new string[maxPiece];
				Name = new string[maxPiece];
				Expression = new string[maxPiece];
				Type = new string[maxPiece];
				
				
					
				for (curPiece = 0; curPiece < maxPiece; curPiece++)
				{
					reader.Read();
					//speedUpDialogue = false;
					if(reader.IsStartElement("speach"))
					{
						
						Name[curPiece] = reader.GetAttribute("name");
						Expression[curPiece] = reader.GetAttribute("exp");
						Type[curPiece] = reader.GetAttribute("type");
						TalkInfo[curPiece] = reader.ReadString();
					}
				}
				
				curPiece = 0;
				//eventID++;
				
			}			
		}
		
	}
	
	//void CheckForContinue ()
	//{
		
	//	if (GameObject.Find("Item").GetComponent<Item>().hasBeenListened)
	//	{
	//		eventID++;
	//		LoadLevelData();
	//	}
	//}
	
	// Update is called once per frame
	void Update () {
//		print(_statusFile.text);
		if(InvestigateMode.gameObject.activeSelf)
		{
			if (!leisureTime)
			{
				ShowingText();
				//ChangeStatus.enabled = false;
			}
			else
			{
				ChangeStatus.enabled = true;
			}
			//CheckForContinue();
			
			// Disable items interaction when drama playing
			DisableItemsInteraction();
			
			if (checkForContinue)
			{
				checkForContinue = false;
				leisureTime = false;
				eventID++;
				LoadLevelData();
				TalkPanel.enabled = true;
			}
			if (eventChange)
			{
				eventChange = false;
				leisureTime = false;
				LoadLevelData();
				TalkPanel.enabled = true;
				
				OnBattleEnd();
				
				
				//pnBattleSystem.gameObject.SetActive(false);
			}
			
			if (eventID == 7 && leisureTime)
			{
				OnBattleBegin();
			}
			if (eventID == 8 && leisureTime)
			{
				OnBattleBegin();
				
			}
			
			
		}
		
		
		
		
	
	}
	
	void ShowingText ()
	{
		
		if (Type[curPiece] == "speach")
		{
			lbTutorial.enabled = false;
			if (TalkPanel.enabled)
			{
				// 逐字显示对话
				//if (!speedUpDialogue)
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
							//lbTalk.text = TalkInfo[curPiece].Substring(0, ++mOffset);
								
						}
					}
				}
				
				// 显示姓名
				lbName.text = Name[curPiece];
				
				// 显示表情
				if(Name[curPiece] == "K")
				{			
					foreach (GameObject go in GameObject.FindGameObjectsWithTag("诺诺"))
					{
						go.GetComponent<UISprite>().enabled = false;
					}

					if (Expression[curPiece] == "think")
					{
						GameObject.Find("k吃惊").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k愤怒").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k得意").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k倾听").GetComponent<UISprite>().enabled = true;
					}
					else if (Expression[curPiece] == "angry")
					{
						
						GameObject.Find("k倾听").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k得意").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k吃惊").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k愤怒").GetComponent<UISprite>().enabled = true;
					}
					else if (Expression[curPiece] == "happy")
					{
						GameObject.Find("k倾听").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k吃惊").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k愤怒").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k得意").GetComponent<UISprite>().enabled = true;
					}
					else if (Expression[curPiece] == "surprise")
					{
						GameObject.Find("k倾听").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k愤怒").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k得意").GetComponent<UISprite>().enabled = false;
						GameObject.Find("k吃惊").GetComponent<UISprite>().enabled = true;
					}
				}
				else if(Name[curPiece] == "诺诺")
				{			
					foreach (GameObject go in GameObject.FindGameObjectsWithTag("K"))
					{
						go.GetComponent<UISprite>().enabled = false;
					}
					//Debug.Log(Expression[curPiece]);
					if (Expression[curPiece] == "think")
					{
						GameObject.Find("诺诺吃惊").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺愤怒").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺得意").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺倾听").GetComponent<UISprite>().enabled = true;
					}
					else if (Expression[curPiece] == "angry")
					{
						GameObject.Find("诺诺倾听").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺得意").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺吃惊").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺愤怒").GetComponent<UISprite>().enabled = true;
					}
					else if (Expression[curPiece] == "happy")
					{
						GameObject.Find("诺诺倾听").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺吃惊").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺愤怒").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺得意").GetComponent<UISprite>().enabled = true;
					}
					else if (Expression[curPiece] == "surprise")
					{
						GameObject.Find("诺诺倾听").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺愤怒").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺得意").GetComponent<UISprite>().enabled = false;
						GameObject.Find("诺诺吃惊").GetComponent<UISprite>().enabled = true;
					}
				}
				
				
				}
		}
		else if (Type[curPiece] == "tutorial")
		{
			TalkPanel.enabled = false;
			lbTutorial.enabled = true;
			lbTutorial.text = TalkInfo[curPiece];
			
		}
		if (Input.GetMouseButtonUp(0))
		{

			if (Type[curPiece] == "speach")
			{
				if (mOffset >= TalkInfo[curPiece].Length)
				{
					mOffset = 0;
					curPiece++;
					//speedUpDialogue = false;
				}
				else
				{
					//speedUpDialogue = true;
					//charsPerSecond = 50;
				}
			}
			else if (Type[curPiece] == "tutorial")
			{
				mOffset = 0;
				curPiece++;
				lbTutorial.enabled = false;

			}
				
			if (curPiece >= maxPiece)
			{
				leisureTime = true;
					
				TalkPanel.enabled = false;
				curPiece = 0;
				lbTalk.text = null;
				ClearExps();
				
			}

			
			
		}
		
		
	}
	
	void DisableItemsInteraction ()
	{
		// TODO: Learning how to get a child object.
		GameObject[] items;
		items =	GameObject.FindGameObjectsWithTag("Item");
		foreach (GameObject item in items)
		if (TalkPanel.enabled)
		{
			item.GetComponent<UIButtonMessage>().enabled = false;
		}
		else { item.GetComponent<UIButtonMessage>().enabled = true;}

	}
	
	
	void OnChangeStatus ()
	{
		
		
		if (RhythmMode.gameObject.activeSelf)
		{
			
			RhythmMode.gameObject.SetActive(false);
			GameObject.Find("RhythmModeController").GetComponent<RhythmMode>().enabled = false;
			for (int i = 0; i < 12; i++)
			{
				foreach (GameObject go in 
					GameObject.FindGameObjectsWithTag("Line" + i))
				{
					go.renderer.enabled = false;
				}
			}
			InvestigateMode.gameObject.SetActive(true);
		}
		else {
			RhythmMode.gameObject.SetActive(true);
			GameObject.Find("RhythmModeController").GetComponent<RhythmMode>().enabled = true;
			for (int i = 0; i < 12; i++)
			{
				foreach (GameObject go in 
					GameObject.FindGameObjectsWithTag("Line" + i))
				{
					go.renderer.enabled = true;
				}
			}
			InvestigateMode.gameObject.SetActive(false);//.enabled = false;
		}
		
	}
	
	void OnBattleBegin ()
	{
		Debug.Log("Battle Begin");
		//GameObject battleMode = (GameObject)Instantiate(PreBattleMode, new Vector3(0, 0, 0), transform.rotation);
		BattleDemo.battleTimes++;
		BattleDemo.battleHasStarted = true;
		BattleSystem.gameObject.SetActive(true);
		InvestigateMode.gameObject.SetActive(false);
		//GameObject.Find("BattleSystemController").GetComponent<BattleDemo>().enabled = true;
	}
	
	void OnBattleEnd ()
	{
		BattleDemo.battleHasStarted = false;
//		GameObject.Find("BattleSystemController").GetComponent<BattleDemo>().enabled = false;
		BattleSystem.gameObject.SetActive(false);
		InvestigateMode.gameObject.SetActive(true);
		
	}
	
	
	
}

