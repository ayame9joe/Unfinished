using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleDemo : MonoBehaviour {

	//--
	//--Battle System for Unfinished.
	//--Version 1.0
	//--Author: Qiao Xiaomeng
	//--2013.6.13

	bool check;
	bool hasInit = false;
	bool check1 = true;
	// TODO: 原型测试与讨论修改

	public GameObject PrePlayerAttackComic;
	public GameObject PrePlayerDefendComic;
	public GameObject PrePlayerBreakComic;
	public GameObject PrePlayerBeatenComic;
	public GameObject PreEnemyAttackComic;
	public GameObject PreEnemyBeatenComic;
	public GameObject PreEnemyStatus;
	
	public UIPanel InvestigatePanel;
	public UIPanel BattleSystemPanel;
	
	public UIButton Attacker;
	
	public UIButton Defender;
	public UIButton Over;
	
	public UILabel AP;
	public UILabel BP;
	
	public UISprite HP;
	public UISprite EnemyHP;
	
	public UISprite BattleStart;
	
	bool check4EnemysTurn = false;
	bool check4Once = true;
	
	public static int battleTimes = 0;
	
	
	GameObject playerInstance;
	GameObject enemyInstance;
	GameObject enemyStatusInstance;
	
	GameObject[] comicInstances;
	List<GameObject> enemyStatusInstances;
	
	bool newComicComing = false;

	
	bool stopForASecond = false;
	
	bool everyTurn = true;
	bool isEnemysTurn;
	bool battleStart = false;
	int i = 1;
	
	bool onStatus = false;
	bool onAttacker = false;
	bool onDefender = false;
	bool onBack = false;
	bool onBrave = false;
	bool onDefault = false;
	bool onOver = false;
	
	enum ComicPlay 
	{
		playerAttack,
		playerBeaten,
		playerBreak,
		playerRetreat,
		enemyAttack,
		enemyBeaten,
		nothing
	}
	
	ComicPlay comicPlay = ComicPlay.nothing;
	//------Variables Used in Battle System------
	int turnCount;					// 回合数
	int availableActionTimes;		// 可行动数
	int actualActionTimes;			// 实际行动累积数
	int braveTimes;					// 勇气数
	int BPValue;					// BP值
	
	int dropsDownJudge;
	
	int defendTimes = 0;
	
	//------Boolens Used in Battle System------
	public static bool battleHasStarted = false;		
	bool playersTurn;					// 玩家回合
	bool checkForOnce = true;
	bool battleWin = false;
	bool hasLevelUp = true;
	bool changeStatus = false;
	bool checkForAvailableSkills = false;
	bool battleEvaluation = false;
	bool continuoursFighting = false;
	bool chooseEnemy = false;
	
	
	//------Skills------
	Skill[] skillWeHaveNow;
	List<Skill> skillCanBeUsed;

	Skill attack1;
	Skill attack2;
	Skill defend;
	Skill heal;
	
	//------Player------
	Player player;
	
	//------Enemies------
	Enemy[] enemies;
	Enemy choosedOne = new Enemy();
	
	struct Player
	{
		//----Base Attributes for the Player----
		public float baseHealth;
		public float curHealth;
		public int level;
		public int exp;
		public int ATK;
		public int DEF;

		
		//----Exp for a certain Level Caculation----
		public int ExpForNextLevel(int lv)
		{
			// TODO: 研究经验值与等级计算公式
			return lv * lv * lv+ 3 * lv * lv + 9 * lv + 9;
		}
		
		//----All Statuses for the Player----
		public enum STATUS 
		{
			ATTACKER,
			BLASTER,
			DEFENDER,
			HEALER,
			ENHANGCE,
			JAMMER
		}
		
		//----Current Status for the Player----
		public STATUS status;
		
		//----Initialization for the Player----
		public void Init ()
		{
			this.baseHealth = 500;
			this.curHealth = this.baseHealth;
			this.status = STATUS.ATTACKER;
			this.ATK = 300;
			this.DEF = 20;
		}
		
		//----Initialization for Level and Exp----
		public void InitLvExp ()
		{
			this.level = 1;
			this.exp = 0;
		}
	}
	
	struct Enemy
	{
		//----Base Attributes for this Enemy----
		public string name;
		public float baseHealth;
		public float curHealth;
		public int ATK;
		public int DEF;
		
		
		//----Variables relates to Drops
		public int EXP;
		public string[] drops;
		public float[] dropsRate;
		
		public void Init ()
		{
			this.name = "Enemy";
			this.baseHealth = 1000;
			this.curHealth = this.baseHealth;
			this.ATK = 60;
			this.DEF = 20;
			this.EXP = 10;
			this.drops = new string[2]{"Item1", "Item2"};
			// TODO: You didn't use dropsRate
			this.dropsRate = new float[2]{0.2f, 0.8f};
		}
	}
	
	struct Item
	{
		// TODO: Should We at least have a name for that? Or, think about what should an item have in its struct.
		public string name;
	}
	
	struct Skill {
		
		// TODO: Level up the skill.
		// TODO: For further use, you should division the skill struct into more specific ones. Like RPG Maker does.
		
		//----Base Attributes for the Skill----
		public string name;
		public bool available;
		public int curLevel;
		public int maxLevel;
		public int proficiency;
		public int damage;
		
		//----The target for Skill Using----
		public Enemy targetEnemy;
		public Player myPlayer;
		
		//----Caculation for the Damage.
		public void DamageCaculation (Enemy enemy)
		{
			// TODO: We'll check this out. Or try to make it better.
			enemy.curHealth -= 800;
			//Debug.Log(enemy.curHealth);
		}
	}
	
	public void InitSkill()
	{
	
		//----Initialization for All Skills----
		attack1.name = "攻   击";
		attack1.myPlayer = player;
		// TODO: Damage Caculation
		attack1.damage = (attack1.myPlayer.ATK - attack1.targetEnemy.DEF) + Random.Range(-20, 20);
		attack2.name = "攻   击";
		defend.name = "防   御";
		heal.name = "Heal";
		
		//----Put them into different categraphies according to availability
		skillWeHaveNow = new Skill[4]{attack1, attack2, defend, heal};
		
		skillCanBeUsed = new List<Skill>();
		foreach (Skill skill in skillWeHaveNow)
		{
			if (skill.available)
			{
				skillCanBeUsed.Add(skill);
			}
		}
			

	}
	
	public void CheckStatusSkill ()
	{
		// TODO:这里可以优化一下，直接用技能类别进行判断
		switch (player.status)
		{
		case Player.STATUS.ATTACKER:			
			attack1.available = true;
			attack2.available = true;
			defend.available = false;
			heal.available = false;				
			break;
		case Player.STATUS.DEFENDER:		
			defend.available = true;
			attack1.available = false;
			attack2.available = false;
			heal.available = false;
			//skillCanBeUsed.Clear();						
			break;
		case Player.STATUS.HEALER:
			heal.available = true;
			defend.available = false;
			attack1.available = false;
			attack2.available = false;		
			break;
		}
	}
	

	
	public void InitEnemy ()
	{
		int numberOfEnemy = Random.Range(1,4);
		enemies = new Enemy[1];
		
		for (int i = 0; i < enemies.Length; i++)
		{
			//Debug.Log(i);
			
			enemies[i].name = "Enemy" + i.ToString();
			enemies[i].Init();
		}
	}
	
	public void InitBaseVars ()
	{
		turnCount = 1;					// 回合数
		availableActionTimes = 0;		// 可行动数
		actualActionTimes = 0;			// 实际行动累积数
		braveTimes = 0;					// 勇气数
		BPValue = 0;					// BP值
		battleEvaluation = false;
		battleWin = false;
	}
	
	// Use this for initialization
	void Start () {
		
		enemyStatusInstances = new List<GameObject>();
//		StatusCommand.enabled = false;
		BattleStart.enabled = false;
		StartCoroutine("WaitForGameStart");
		Attacker.gameObject.SetActive(false);
		Defender.gameObject.SetActive(false);
		Over.gameObject.SetActive(false);
		if (!hasInit)
		{
			InitAll();
			
		}
	}
	
	void InitAll()
	{
		//checkForOnce = true;
		Debug.Log("Init all!");
		hasInit = true;
		//----Initialization for the Player
		player.Init();
		player.InitLvExp();
		InitEnemy();
		InitBaseVars();
		




	

	}
	
	// TODO: Check out if we can combine InitAll() and ResetAll() and ContinousFighting()
	void ResetAll()
	{
		player.Init();
		InitEnemy();
		InitBaseVars();
		
	}
	
	void ResetContinuousFighting()
	{
		InitEnemy();
		InitBaseVars();

	}

	// Update is called once per frame
	void Update () {
		
		//Debug.Log(battleHasStarted);
		Debug.Log(stopForASecond);
		AP.text = (availableActionTimes - actualActionTimes).ToString();
		BP.text = BPValue.ToString();
		HP.transform.localScale = new Vector3(player.curHealth / player.baseHealth * 1.3f, 0.125f, 1);
		EnemyHP.transform.localScale = new Vector3(enemies[0].curHealth / enemies[0].baseHealth * 1.3f, 0.125f, 1);
		//HP.transform.localPosition 
		Debug.Log(comicPlay);
		if (battleHasStarted)
		{
			//----Count for the Turn and Base Variables----
			if (turnCount % 2 == 1)
			{
				playersTurn = true;
				availableActionTimes = braveTimes + 1;
			}
			else 
			{
				playersTurn = false;
			}
			
			
			
			float allEnemyCurHealth = 0;
			//----Check for the Health for the Enemy----
			for (int i = 0; i < enemies.Length; i++)
			{
				if (enemies[i].curHealth <= 0)
				{
					enemies[i].curHealth = 0;
				}
				allEnemyCurHealth += enemies[i].curHealth;
	
			}
			
			//----Battle Lose & Show the Evaluation----
			if ( allEnemyCurHealth <= 0)
			{
				
				battleWin = true;
				battleEvaluation = true;

			}
			//----Battle Win & Show the Evaluation----
			else if (player.curHealth <= 0)
			{
				
				player.curHealth = 0;
				battleWin = false;
				battleEvaluation = true;
				
			}
		}
		else 
		{
			
			if (continuoursFighting)
			{
				ResetContinuousFighting();
			}
			else
			{
				ResetAll();
			}
			
		}
		
		Debug.Log(newComicComing);
		comicInstances = GameObject.FindGameObjectsWithTag("Comic");
		if (comicInstances.Length > 0)
		{
			Debug.Log("漫画数量" + comicInstances.Length);
			if (!stopForASecond)
			{
				foreach (GameObject go in comicInstances)
				{
					go.transform.Translate(Vector3.up * 10f);
					
					if (go.transform.localPosition.y > 500)
						Destroy(go);	
				}
				
				if (comicInstances[comicInstances.Length - 1])
				{
				if (comicInstances[comicInstances.Length - 1].transform.localPosition.y > 220)
				{
						Debug.Log("漫画+1");
					newComicComing = true;

					everyTurn = true;
					
					i++;
					// TODO: You can change the number according to how many enemies encountered
					if (i%2 == 0)
					{
						isEnemysTurn = true;
					}
					else { isEnemysTurn = false; }
					
				}
				}
			}
				
		}
		
		if (newComicComing)
		{
			
			

			newComicComing = false;

		}
		
		if (Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if (!stopForASecond)
			{
				stopForASecond = true;
			}
			else { stopForASecond = false; }
		}
		
		if (everyTurn)
		{
			stopForASecond = true;
		}



	}
	
	void OnGUI()
	{
		if (!battleHasStarted)
		{
			//battleWin = false;
			//if (GUI.Button(new Rect(0, 0, 100, 25), "Battle Begin"))
			{
				//battleHasStarted = true;	
				// TODO: 将它移到更合适的地方
				hasLevelUp = false;
				dropsDownJudge = Random.Range(1, 10);
			}
		}
		
		
		if (battleHasStarted)
		{

			//Command.enabled = true;
			//StatusCommand.enabled = false;
			
			InitSkill();
			//GUI.Label(new Rect(0, 375, 500, 50),"You encountered " + enemies.Length + " enemies.");
			
			
			if (!battleEvaluation)
			{
				
				continuoursFighting = false;
				if (playersTurn)
				{
					
					//GUI.Label(new Rect(0, 350, 500, 50),"This is player's turn");

					
					if (checkForOnce)
					{	
						//InitAll();
						Debug.Log("Another Battle Begin!");
						Over.gameObject.SetActive(false);
						onStatus = false;
						actualActionTimes = 0;
						braveTimes = 0;
						stopForASecond = true;
						everyTurn = true;

						checkForOnce = false;
						if (turnCount > 1)
						{
							comicPlay = ComicPlay.enemyAttack;
							ComicBorn();

						}
						check1 = true;
						//ComicBorn();
					}
		
					
					//----Option Brave----
					//if (GUI.Button(new Rect(0, 0, 75, 25), "Brave"))
					if (onBrave)
					{
						
						onBrave = false;
						if (BPValue > -3)
						{
							Debug.Log("Brave" + BPValue);
							BPValue--;
							braveTimes++;
							
							comicPlay = ComicPlay.playerBreak;
							ComicBorn();
						}
						
						
					
					}
					
					//----Option Default----
					//else if (GUI.Button(new Rect(0, 25, 75, 25), "Default"))
					else if (onDefault)
					{
						onDefault = false;
						if ( BPValue < 3 )
						{
							BPValue++;
							turnCount++;
							
						}
						comicPlay = ComicPlay.playerRetreat;
						ComicBorn();
					}
					
					if (!onStatus)
					{
						Attacker.gameObject.SetActive(false);
						Defender.gameObject.SetActive(false);
					//	Command.enabled = true;
					//	StatusCommand.enabled = false;
					}
					
					//----Still Have Chance to Act----
					if (actualActionTimes < availableActionTimes)
					{
						//----Option Status----
						//if (GUI.Button(new Rect(0, 50, 75, 25), "Status"))
						//if (onStatus)
						/*{
							//onStatus

							if (changeStatus)
							{
								changeStatus = false;
							}
							else 
							{
								changeStatus = true;
							}
							
						}*/
						
						if (onStatus)
						{
							Attacker.gameObject.SetActive(true);
							Defender.gameObject.SetActive(true);
							//Command.enabled = false;
							//StatusCommand.enabled = true;
							//if (GUI.Button(new Rect(75, 50, 75, 25), "Attacker"))
							if (onAttacker)
							{
								//newComicComing = true;
								chooseEnemy = true;
								
								//Debug.Log("dot");
								//player.status = Player.STATUS.ATTACKER;
								Attacker.gameObject.SetActive(false);
								Defender.gameObject.SetActive(false);
								onAttacker = false;
								CheckStatusSkill();
								actualActionTimes++;
								checkForAvailableSkills = true;
								comicPlay = ComicPlay.playerAttack;
								ComicBorn();


								
								
								
								if (actualActionTimes == availableActionTimes)
								{

										Debug.Log("hi there");
										turnCount++;
				
								}
							}
							//if (GUI.Button(new Rect(150, 50, 75, 25), "Defender"))
							if (onDefender)
							{
								
								Attacker.gameObject.SetActive(false);
								Defender.gameObject.SetActive(false);
								defendTimes++;
								actualActionTimes++;
								Debug.Log("防御次数" + defendTimes);
								//turnCount++;
								if (actualActionTimes == availableActionTimes)
								{
										Debug.Log("hi there");
										turnCount++;
				
								}

									
									comicPlay = ComicPlay.playerRetreat;
									ComicBorn();
								//if (battleTimes == 1)
								{
									if (defendTimes >= 3)
									{
										Debug.Log("下一次战斗");
										//InitAll();
										//DestroyComics();
																
										StoryController.eventID = 8;
										StoryController.eventChange = true;
										InvestigatePanel.gameObject.SetActive(true);
										GameObject.Find("StoryController").GetComponent<StoryController>().enabled = true;
										BattleSystemPanel.gameObject.SetActive(false);
										GameObject.Find("BattleSystem1Controller").GetComponent<BattleDemo>().enabled = false;

										
										//Destroy(GameObject.Find("BattleMode"));
										
										
										
										
										
	
										
									}
								}
								onDefender = false;
								player.status = Player.STATUS.DEFENDER;
								
								

								CheckStatusSkill();
								
								checkForAvailableSkills = true;
							}
							//if (GUI.Button(new Rect(225, 50, 75, 25), "Healer"))
							//{
		//
							//	player.status = Player.STATUS.HEALER;
							//
							//	CheckStatusSkill();
							//}
						}
						
						//----Option Skill----
						/*if (GUI.Button(new Rect(0, 75, 75, 25),"Skill"))
						{

							if (checkForAvailableSkills)
							{
								checkForAvailableSkills = false;
							}
							else { checkForAvailableSkills = true; }

						}*/
						
						
						//----Check & Choose Skills----
						if (checkForAvailableSkills)
						{
							
							Debug.Log("Check for available skills");
							for (int i = 0; i < skillCanBeUsed.Count; i++)
							{
								// TODO: 修改
								GameObject.Find("SkillList").GetComponent<UILabel>().text = skillCanBeUsed[i].name;
							//if (GUI.Button(new Rect(75 + i * 75, 75, 75, 25), skillCanBeUsed[i].name))
							{
									
								if (stopForASecond)
								{
								//actualActionTimes++;
								//enemy.curHealth -= 500;
								switch (skillCanBeUsed[i].name)
								{
								case "攻   击":
									
									Debug.Log("攻击");
									
									attack1.proficiency++;
									//chooseEnemy = true;
									//if (choosedOne)
									{
										choosedOne.curHealth -= attack1.damage;
									}
									//GUI.Button(new Rect(75, 100, 75, 25), "Enemy");
										
									// 伤害公式
									//int temp = (player.ATK - enemy.DEF) + Random.Range(-20, 20);
									//enemy.curHealth -= attack1.damage;
									//checkForAvailableSkills = false;
										
									
									//GUI.Label(new Rect(0, 175, 300, 25),"Player Attack!" + attack1.damage);
									break;
								case "Attack2":
											chooseEnemy = true;
										attack2.proficiency++;
									break;		
								case "防   御":
										defend.proficiency++;
									Debug.Log("Defend");
									//GUI.Label(new Rect(0, 175, 300, 25),"Player Defend!");

												
									
									
									break;
								case "Heal":
										heal.proficiency++;
									Debug.Log("Heal");
									//GUI.Label(new Rect(0, 175, 300, 25),"Player Heal 30");
									actualActionTimes++;
									player.curHealth += 50;
									if (player.curHealth >= player.baseHealth)
									{
										player.curHealth = player.baseHealth;
									}
									break;
								}
										
								}
							}
							}
								
						}
						
						
						//----Choose Enemy----					
						if (chooseEnemy)
						{
							//for (int j = 0; j < enemies.Length; j++)
							{
								chooseEnemy = false;
								//if (GUI.Button(new Rect(75 + j * 75, 100, 75, 25), enemies[j].name))
								{
									//Debug.Log(actualActionTimes);
									//actualActionTimes++;
									//attack1.targetEnemy = enemies[j];
									//attack1.DamageCaculation(enemies[j]);
									enemies[0].curHealth -= Random.Range(200, 300);
									
									
									//Over.gameObject.SetActive(true);
									
											
								}
							}
						}
						
						//----Option Auto----
					
						

					


						
					}
					

					
					//check4EnemysTurn = false;
					
				}
				else 
				{
					//----Enemy's Turn----
					// TODO: enemy's turn caculation
					if (isEnemysTurn)
					{
						
						
						
				
					}
					if (turnCount > 1)
					{
						Over.gameObject.SetActive(true);
					}
					//Over.gameObject.SetActive(true);
					//if (GUI.Button(new Rect(0, 75, 75, 25),"End Turn"))
				//	if (!check4EnemysTurn)
					if (onOver)
					{
	
						//check4EnemysTurn = true;
						onOver = false;
						// 我方伤害公式
						
						for(int i = 0; i < enemies.Length; i++)
						{
						int temp = enemies[i].ATK - player.DEF + Random.Range(-5, 5);
						//GUI.Label(new Rect(0, 175, 300, 25),"Enemy Attack:" + temp);
						player.curHealth -= temp;
						}

						turnCount++;
						checkForOnce = true;
							// 判断下一回合玩家可否行动
						if (BPValue + 1 <= 0)
						{
							comicPlay = ComicPlay.enemyAttack;
							ComicBorn();
													comicPlay = ComicPlay.playerBeaten;
							ComicBorn();
								// TODO:暂停并显示标志
								
								//GUI.Label(new Rect(0, 175, 300, 25),"Player passed this turn!");
								turnCount++;
								BPValue++;
								actualActionTimes = 0;
								braveTimes = 0;
						}
							
						
					}
					//GUI.Label(new Rect(0, 350, 500, 50),"This is enemy's turn");
				}
			
				
				//----Show Datas----
				
				
				//GUI.Label(new Rect(0, 275, 100, 25),"BP:" + BPValue);
				//GUI.Label(new Rect(0, 250, 500, 25),"AvailableActionTimes:" + availableActionTimes);
				//GUI.Label(new Rect(0, 300, 100, 25),"Turn Number:" + turnCount);
				//GUI.Label(new Rect(0, 225, 300, 25),"ActionTimes:" + actualActionTimes);
				//GUI.Label(new Rect(0, 200, 300, 25),"BraveTimes:" + braveTimes);
				
				//GUI.Label(new Rect(400, 0, 300, 25),"Player HP:" + player.curHealth + "/" + player.baseHealth);
				//GUI.Label(new Rect(400, 25, 100, 25),"Player Level:" + player.level);
				//GUI.Label(new Rect(400, 50, 100, 25),"Player exp:" + player.exp);
				for (int i = 0; i < enemies.Length; i++)
				{
				//	GUI.Label(new Rect(400, 75 + 25 * i, 300, 25),"Enemy HP:" + enemies[i].curHealth + "/" + enemies[i].baseHealth);
				}
				
			}
			
			else
			{

				
				if (comicInstances.Length > 0)
				{
					foreach (GameObject go in comicInstances)
					{	
						Destroy(go);	
					}
				}
				
				if (battleWin)
				{
					
				}
			
			
			}
			
			
			
		}
		
			if (player.exp >= player.ExpForNextLevel(player.level))
			{
				
				if (!hasLevelUp)
				{
					hasLevelUp = true;
					player.level++;
				
					// TODO: 以下写能力成长公式
				}
			}

			

		

		
	
	}
	
	

	
	void DestroyComics ()
	{
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Comic"))
		{
			Destroy(go);
		}
	}
	
	void ComicBorn ()
	{

		switch(comicPlay)
		{
		case ComicPlay.playerAttack:
			playerInstance = (GameObject)Instantiate(PrePlayerAttackComic, new Vector3(0, 0, 0), transform.rotation);
			playerInstance.transform.parent = GameObject.Find("Comic").transform;
			playerInstance.transform.localPosition = new Vector3(-210, 0, 0);
			playerInstance.transform.localScale = new Vector3(440, 220, 0); 
			break;
		case ComicPlay.enemyAttack:
			enemyInstance = (GameObject)Instantiate(PreEnemyAttackComic, new Vector3(0, 0, 0), transform.rotation);
			enemyInstance.transform.parent = GameObject.Find("Comic").transform;
			enemyInstance.transform.localPosition = new Vector3(230, 0, 0);
			enemyInstance.transform.localScale = new Vector3(440, 220, 0); 
			break;
		case ComicPlay.playerBeaten:
			playerInstance = (GameObject)Instantiate(PrePlayerBeatenComic, new Vector3(0, 0, 0), transform.rotation);
			playerInstance.transform.parent = GameObject.Find("Comic").transform;
			playerInstance.transform.localPosition = new Vector3(-210, 0, 0);
			playerInstance.transform.localScale = new Vector3(440, 220, 0); 
			break;
		case ComicPlay.playerBreak:
			playerInstance = (GameObject)Instantiate(PrePlayerBreakComic, new Vector3(0, 0, 0), transform.rotation);
			playerInstance.transform.parent = GameObject.Find("Comic").transform;
			playerInstance.transform.localPosition = new Vector3(-210, 0, 0);
			playerInstance.transform.localScale = new Vector3(440, 220, 0); 
			break;
		case ComicPlay.playerRetreat:
			playerInstance = (GameObject)Instantiate(PrePlayerDefendComic, new Vector3(0, 0, 0), transform.rotation);
			playerInstance.transform.parent = GameObject.Find("Comic").transform;
			playerInstance.transform.localPosition = new Vector3(-210, 0, 0);
			playerInstance.transform.localScale = new Vector3(440, 220, 0); 
			break;
		case ComicPlay.enemyBeaten:
			enemyInstance = (GameObject)Instantiate(PreEnemyBeatenComic, new Vector3(0, 0, 0), transform.rotation);
			enemyInstance.transform.parent = GameObject.Find("Comic").transform;
			enemyInstance.transform.localPosition = new Vector3(230, 0, 0);
			enemyInstance.transform.localScale = new Vector3(440, 220, 0); 
			break;
		}
		stopForASecond = false;
		everyTurn = false;
	}

	void OnStatus ()
	{
		if (onStatus)
		{
			onStatus = false;
		}
		else
		{
			onStatus = true;
		}
	}
	
	void OnAttacker ()
	{
		onAttacker = true;
	}
	
	void OnDefender ()
	{
		onDefender = true;
	}
	
	
	void OnBrave ()
	{
		onBrave = true;
	}
	
	void OnDefault ()
	{
		onDefault = true;
	}
	
	void OnOver ()
	{
		onOver = true;
		Over.gameObject.SetActive(false);
	}
	
	IEnumerator WaitForGameStart ()
	{
		Debug.Log("Wait for Rhythm Game Start!");
		if (!battleHasStarted)
		{
			Debug.Log("Wait for Rhythm Game Start!");
			BattleStart.enabled = true;
			if (this.gameObject.GetComponent<AudioSource>().clip.name == "战斗开始")
			{
				this.gameObject.GetComponent<AudioSource>().Play();
			}
			yield return new WaitForSeconds(2f);
			Debug.Log("Rhythm Game has Started!");
			BattleStart.enabled = false;
			battleHasStarted = true;
			if (this.gameObject.GetComponent<AudioSource>().clip.name == "战斗")
			{
				this.gameObject.GetComponent<AudioSource>().Play();
			}
		}
	}


}