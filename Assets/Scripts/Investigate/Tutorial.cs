using UnityEngine;
using System.Collections;
using System.Xml;

public class Tutorial : MonoBehaviour {
	
	
	
	public UIPanel mPanelTutorial;
	public UILabel mLabelTutorial;

	int maxPiece;
	int curPiece;
	int ID;
	
	string[] TutorialInfo;
	
	
	
	void Awake ()
	{
		ID = 0;
		maxPiece = 0;
		curPiece = 0;
		
		TutorialInfo = null;
		
		mPanelTutorial.enabled = false;
		
	}
	
	void Start ()
	{
		ReadTutorialInfo();
		
	}
	
	void ReadTutorialInfo ()
	{
		XmlReader reader = XmlReader.Create("xmlExample.XML");
		
		while(reader.Read())
		{
			
			if (reader.IsStartElement("tutorial") && 
				reader.GetAttribute("ID") == ID.ToString() )
				
			{	
				//Debug.Log(reader.ReadString());
				//mLabelPooster.text = reader.ReadString();
				
				maxPiece = int.Parse(reader.GetAttribute("paras"));
				
				TutorialInfo = new string[maxPiece];
					
				for (curPiece = 0; curPiece < maxPiece; curPiece++)
				{
					reader.Read();
					if(reader.IsStartElement("info"))
					{
						
						TutorialInfo[curPiece] = reader.ReadString();
						
							
					}
				}
					
				curPiece = 0;

			}
		}

	}
	
	void Update ()
	{
		mLabelTutorial.text = TutorialInfo[curPiece];
		
//		Debug.Log(curPiece);
		if (Input.GetMouseButtonUp(0))
		{
			if (mPanelTutorial.enabled)
			{
				mPanelTutorial.enabled = false;
				curPiece++;
			
				
			}
			
			if (!mPanelTutorial.enabled)
			{
				
			}
			//ID++;
			
			//ReadTutorialInfo();
		}
		
	}
	
	
	
	
}
