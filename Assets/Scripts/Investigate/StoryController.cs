using UnityEngine;
using System.Collections;


public class StoryController : MonoBehaviour {
	
	
	bool check;
	
	
	public UIPanel TalkPanel;
	public UILabel lbTalk;
	public UILabel lbName;
	public UIPanel TutorialPanel;
	public UILabel lbTutorial;
	public UISprite ChangeStatus;
	
	
	//--- Modes Panels---
	public UIPanel RhythmMode;
	public UIPanel InvestigateMode;
	public UIPanel BattleMode1;
	public UIPanel BattleMode2;
	
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
//		GameObject.Find("Controller").GetComponent<BattleDemo>().enabled = false;

		// Initialization for the variables
		maxPiece = 0;
		curPiece = 0;
		
		TalkInfo = null;
		Name = null;
		Expression = null;
		Type = null;
		
		eventID = 1;
		
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
		RhythmMode.gameObject.SetActive(false);
		GameObject.Find("RhythmModeController").GetComponent<RhythmMode>().enabled = false;
		BattleMode1.gameObject.SetActive(false);
		GameObject.Find("BattleSystem1Controller").GetComponent<BattleDemo>().enabled = false;
		BattleMode2.gameObject.SetActive(false);
		GameObject.Find("BattleSystem2Controller").GetComponent<BattleDemo2>().enabled = false;
		LoadLevelData();
		
	}

	
	void LoadLevelData ()
	{
		


		switch(eventID)
		{
		case 1:
			maxPiece = 8;
			
			TalkInfo = new string[maxPiece];
			Name = new string[maxPiece];
			Expression = new string[maxPiece];
			Type = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					TalkInfo[curPiece] = "这是什么地方？";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 1)
				{
					TalkInfo[curPiece] = "为什么我会出现在这里？";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 2)
				{
					TalkInfo[curPiece] = "我睡着了吗，或者……";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 3)
				{
					TalkInfo[curPiece] = "天呐，我什么都不记得了。";
					Name[curPiece] = "K";
					Expression[curPiece] = "surprise";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 4)
				{
					TalkInfo[curPiece] = "有人吗？";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 5)
				{
					TalkInfo[curPiece] = "或者有办法可以出去吗？";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 6)
				{
					TalkInfo[curPiece] = "或许我应该四处调查一番，看看能够发现什么。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 7)
				{
					TalkInfo[curPiece] = "教学1：将鼠标移动至屏幕两侧以显示箭头。长按箭头向相应的方向移动。";
					Name[curPiece] = null;
					Expression[curPiece] = null;
					Type[curPiece] = "tutorial";
				}
			}
				
			curPiece = 0;
			break;
		case 2:
			maxPiece = 9;
			
			TalkInfo = new string[maxPiece];
			Name = new string[maxPiece];
			Expression = new string[maxPiece];
			Type = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					TalkInfo[curPiece] = "口袋电脑？……";
					Name[curPiece] = "K";
					Expression[curPiece] = "surprise";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 1)
				{
					TalkInfo[curPiece] = "嗯，找到了。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 2)
				{
					TalkInfo[curPiece] = "不知从什么时候起，和同时代人一样，变得对于这个物件须臾不离。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 3)
				{
					TalkInfo[curPiece] = "似乎已经成为身体的衍伸与常识一般的存在。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 4)
				{
					TalkInfo[curPiece] = "即使什么都记不得的现在，恢复意识的第一反应还是寻找口袋电脑。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 5)
				{
					TalkInfo[curPiece] = "在这个场所口袋电脑还能够使用吗？";
					Name[curPiece] = "K";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 6)
				{
					TalkInfo[curPiece] = "嗯，没有问题，但是看上去有些古怪。那是什么？";
					Name[curPiece] = "K";
					Expression[curPiece] = "surprise";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 7)
				{
					TalkInfo[curPiece] = "教学2.1：点击物品获得情感信息。";
										Name[curPiece] = null;
					Expression[curPiece] = null;
					Type[curPiece] = "tutorial";
				}
			}
				
			curPiece = 0;
			break;
			case 3:
			maxPiece = 1;
			
			TalkInfo = new string[maxPiece];
			Name = new string[maxPiece];
			Expression = new string[maxPiece];
			Type = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					TalkInfo[curPiece] = "教学2.2：再次点击退出情感信息系统。";
										Name[curPiece] = null;
					Expression[curPiece] = null;
					Type[curPiece] = "tutorial";
				}
				
			}
				
			curPiece = 0;
			break;
			
			case 4:
			maxPiece = 7;
			
			TalkInfo = new string[maxPiece];
			Name = new string[maxPiece];
			Expression = new string[maxPiece];
			Type = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					TalkInfo[curPiece] = "他们在说什么？完全听不懂。";
					Name[curPiece] = "K";
					Expression[curPiece] = "surprise";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 1)
				{
					TalkInfo[curPiece] = "对我来说，更为有趣的是这些信息。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 2)
				{
					TalkInfo[curPiece] = "它们来自哪里，又是如何为我所接收到的？看上去与语音推特像极了。但又有所不同。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 3)
				{
					TalkInfo[curPiece] = "似乎都与门有关？";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 4)
				{
					TalkInfo[curPiece] = "最为神奇的是，我感到它们并不真的象推特。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 5)
				{
					TalkInfo[curPiece] = "我的意思是，这并不是那些公共领域发送的信息，而更像是只是某个升起的念头而已。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 6)
				{
					TalkInfo[curPiece] = "还有什么有趣的吗？";
					Name[curPiece] = "K";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
			}
				
			curPiece = 0;
			break;
			case 5:
			maxPiece = 3;
			
			TalkInfo = new string[maxPiece];
			Name = new string[maxPiece];
			Expression = new string[maxPiece];
			Type = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					TalkInfo[curPiece] = "我的声音？可是我对此毫无记忆！";
					Name[curPiece] = "K";
					Expression[curPiece] = "surprise";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 1)
				{
					TalkInfo[curPiece] = "难道我在没有意识的时候曾经来过这里？甚至从未离开？";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 2)
				{
					TalkInfo[curPiece] = "工作台？他——我是说，我——指的是前面那个东西吗？";
					Name[curPiece] = "K";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
			}
				
			curPiece = 0;
			break;
		case 6:
			maxPiece = 17;
			
			TalkInfo = new string[maxPiece];
			Name = new string[maxPiece];
			Expression = new string[maxPiece];
			Type = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					TalkInfo[curPiece] = "所以我并没有失业？现在还有人来参观展馆。";
					Name[curPiece] = "诺诺";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 1)
				{
					TalkInfo[curPiece] = "这里到底是什么地方？你为什么会在这里？";
					Name[curPiece] = "K";
					Expression[curPiece] = "surprise";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 2)
				{
					TalkInfo[curPiece] = "作为导航人工智能，我在这里不是再正常不过的事情吗？";
					Name[curPiece] = "诺诺";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 3)
				{
					TalkInfo[curPiece] = "一个不肯坚守岗位的人工智能？";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 4)
				{
					TalkInfo[curPiece] = "那么，你对此一定知道些什么咯？关于这一切。";
					Name[curPiece] = "K";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 5)
				{
					TalkInfo[curPiece] = "关于这里，答案有很多，关键在于你是什么身份。——而你，好像正好不知道这一点吧。";
					Name[curPiece] = "诺诺";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 6)
				{
					TalkInfo[curPiece] = "可恶！这些试验是怎么回事？我想我不是试验者。但是，我能够听到那些情感信息……";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 7)
				{
					TalkInfo[curPiece] = "这里是彩虹之城，如果你还记得这座城市的标志景观的话；也是IIRO的实验场所。";
					Name[curPiece] = "诺诺";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 8)
				{
					TalkInfo[curPiece] = "如果你不是试验者的话，莫不是参赛者？";
					Name[curPiece] = "诺诺";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
				else if (curPiece == 9)
				{
					TalkInfo[curPiece] = "参赛者？那是什么……";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
					else if (curPiece == 10)
				{
					TalkInfo[curPiece] = "如果你是的话，那你的目标就应该是到达塔顶，但不要问我这是什么意思。";
					Name[curPiece] = "诺诺";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
									else if (curPiece == 11)
				{
					TalkInfo[curPiece] = "等等！我在那些信息之中听到了我的声音。你总该知道些什么吧。";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
													else if (curPiece == 12)
				{
					TalkInfo[curPiece] = "或许是这样没错，不过，我目前可没有办法提取。";
					Name[curPiece] = "诺诺";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
																	else if (curPiece == 13)
				{
					TalkInfo[curPiece] = "别着急嘛。如果你坚持的话，来看看这些也不错。";
					Name[curPiece] = "诺诺";
					Expression[curPiece] = "happy";
					Type[curPiece] = "speach";
				}
																					else if (curPiece == 14)
				{
					TalkInfo[curPiece] = "教学3.1 点击切换按钮切换至节奏模式。";
										Name[curPiece] = null;
					Expression[curPiece] = null;
					Type[curPiece] = "tutorial";
				}
							if (curPiece == 15)
				{
					TalkInfo[curPiece] = "教学3.2:高亮时点击元件积攒槽值。当前点击蓝色元件。";
										Name[curPiece] = null;
					Expression[curPiece] = null;
					Type[curPiece] = "tutorial";
				}
								if (curPiece == 16)
				{
					TalkInfo[curPiece] = "教学3.3:击中错误元件将扣除一定槽值。";
										Name[curPiece] = null;
					Expression[curPiece] = null;
					Type[curPiece] = "tutorial";
				}
			}
			
			curPiece = 0;
			break;
			case 7:
			maxPiece = 2;
			
			TalkInfo = new string[maxPiece];
			Name = new string[maxPiece];
			Expression = new string[maxPiece];
			Type = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					TalkInfo[curPiece] = "哦，不……";
					Name[curPiece] = "诺诺";
					Expression[curPiece] = "surprise";
					Type[curPiece] = "speach";
				}
				if (curPiece == 1)
				{
					TalkInfo[curPiece] = "那是什么？！";
					Name[curPiece] = "K";
					Expression[curPiece] = "supriese";
					Type[curPiece] = "speach";
				}
				
			}
				OnBattle1Begin();
			curPiece = 0;
			break;
			case 8:
			maxPiece = 3;
			
			TalkInfo = new string[maxPiece];
			Name = new string[maxPiece];
			Expression = new string[maxPiece];
			Type = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					TalkInfo[curPiece] = "不，它还在继续……";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
				if (curPiece == 1)
				{
					TalkInfo[curPiece] = "我不能够就这样退缩。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				if (curPiece == 2)
				{
					TalkInfo[curPiece] = "这场战斗让我有一种莫名的熟悉感。就用它来试试吧。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
					OnBattle2Begin();
				}
				
			}
				OnBattle1Begin();
			curPiece = 0;
			break;
						case 9:
			maxPiece = 3;
			
			TalkInfo = new string[maxPiece];
			Name = new string[maxPiece];
			Expression = new string[maxPiece];
			Type = new string[maxPiece];
			
			for (curPiece = 0; curPiece < maxPiece; curPiece++)
			{
				if (curPiece == 0)
				{
					TalkInfo[curPiece] = "刚才那个女孩？……";
					Name[curPiece] = "K";
					Expression[curPiece] = "angry";
					Type[curPiece] = "speach";
				}
				if (curPiece == 1)
				{
					TalkInfo[curPiece] = "向那个方向去了……";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
				}
				if (curPiece == 2)
				{
					TalkInfo[curPiece] = "好，我也跟上去吧。";
					Name[curPiece] = "K";
					Expression[curPiece] = "think";
					Type[curPiece] = "speach";
					Application.LoadLevel(2);
				}
				
			}
				OnBattle1Begin();
			curPiece = 0;
			break;
		}
		
		
				

				
				
					

				//eventID++;
				
				

		
	}
	

	
	// Update is called once per frame
	void Update () {
//		Debug.Log(GameObject.FindGameObjectWithTag("Item").GetComponent<Interactive>().enabled);
		if(InvestigateMode.gameObject.activeSelf)
		{
			RhythmMode.gameObject.SetActive(false);
		GameObject.Find("RhythmModeController").GetComponent<RhythmMode>().enabled = false;
		BattleMode1.gameObject.SetActive(false);
		GameObject.Find("BattleSystem1Controller").GetComponent<BattleDemo>().enabled = false;
		BattleMode2.gameObject.SetActive(false);
		GameObject.Find("BattleSystem2Controller").GetComponent<BattleDemo2>().enabled = false;
			if (!leisureTime)
			{
				ShowingText();
				//DisableItemsInteraction();
				ChangeStatus.enabled = false;
			}
			else
			{
				
				ChangeStatus.enabled = true;
			}
			//CheckForContinue();
			
			// Disable items interaction when drama playing
			
			
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
				
				OnBattle1End();
				OnBattle2End();
				
				
				//pnBattleMode.gameObject.SetActive(false);
			}
			
			if (eventID == 7 && leisureTime)
			{
				OnBattle1Begin();
			}
			if (eventID == 8 && leisureTime)
			{
				OnBattle2Begin();
				
			}
			
			
		}
		
		
		
		
	
	}
	
	void ShowingText ()
	{
		
		if (Type[curPiece] == "speach")
		{
			lbTutorial.enabled = false;
			TutorialPanel.enabled = false;
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
							lbTalk.text = TalkInfo[curPiece].Substring(0, ++mOffset);
								
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
				else
				{
					ClearExps();
				}
				
				
				}
		}
		else if (Type[curPiece] == "tutorial")
		{
			ClearExps();
			TalkPanel.enabled = false;
			lbTutorial.enabled = true;
			TutorialPanel.enabled = true;
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
				TutorialPanel.enabled = false;

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
	
		Debug.Log("Leisure Time:" + leisureTime);
		
		
	}
	
	void DisableItemsInteraction ()
	{
		// TODO: Learning how to get a child object.
		GameObject[] items;
		items =	GameObject.FindGameObjectsWithTag("Item");
		foreach (GameObject item in items)
		if (!StoryController.leisureTime)
		{
			item.GetComponentInChildren<UIButtonMessage>().enabled = false;
		}
		else { item.GetComponentInChildren<UIButtonMessage>().enabled = true;}

	}
	
	
	void OnChangeStatus ()
	{
		
		Debug.Log(RhythmMode.gameObject.activeSelf);
		
		 {
			
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
	
	void OnBattle1Begin ()
	{
		Debug.Log("Battle Begin");
		//GameObject battleMode = (GameObject)Instantiate(PreBattleMode, new Vector3(0, 0, 0), transform.rotation);
		BattleDemo.battleTimes++;
		BattleDemo.battleHasStarted = true;
		BattleMode1.gameObject.SetActive(true);
		InvestigateMode.gameObject.SetActive(false);
		GameObject.Find("BattleSystem1Controller").GetComponent<BattleDemo>().enabled = true;
	}
	
	void OnBattle1End ()
	{
		BattleDemo.battleHasStarted = false;
		GameObject.Find("BattleSystem1Controller").GetComponent<BattleDemo>().enabled = false;
		BattleMode1.gameObject.SetActive(false);
		InvestigateMode.gameObject.SetActive(true);
		
	}
	void OnBattle2Begin ()
	{
		Debug.Log("Battle Begin");
		//GameObject battleMode = (GameObject)Instantiate(PreBattleMode, new Vector3(0, 0, 0), transform.rotation);
		BattleDemo2.battleTimes++;
		BattleDemo.battleHasStarted = true;
		BattleMode2.gameObject.SetActive(true);
		InvestigateMode.gameObject.SetActive(false);
		GameObject.Find("BattleSystem2Controller").GetComponent<BattleDemo>().enabled = true;
	}
	
	void OnBattle2End ()
	{
		BattleDemo2.battleHasStarted = false;
		GameObject.Find("BattleSystem2Controller").GetComponent<BattleDemo2>().enabled = false;
		BattleMode2.gameObject.SetActive(false);
		InvestigateMode.gameObject.SetActive(true);
		
	

	}
}