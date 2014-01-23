using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSystemS : MonoBehaviour {

	//--
	//--Battle System for Unfinished.
	//--Version 1.0
	//--Author: Qiao Xiaomeng
	//--2013.6.13

	bool check;
	bool hasInit = false;
	// TODO: 原型测试与讨论修改

	public GameObject PrePlayerBattleComic;
	public GameObject PreEnemyBattleComic;
	public GameObject PreEnemyStatus;
	
	public UIButton Attacker;
	public UIButton Defender;
	
	
	
	
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
	
	enum ComicPlay 
	{
		playerAttack,
		enemyAttack
	}
	
	ComicPlay comicPlay;
	//------Variables Used in Battle System------
	int turnCount;					// 回合数
	int availableActionTimes;		// 可行动数
	int actualActionTimes;			// 实际行动累积数
	int braveTimes;					// 勇气数
	int BPValue;					// BP值
	
	int dropsDownJudge;
	
	//------Boolens Used in Battle System------
	bool battleHasStarted = false;		
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
		public int baseHealth;
		public int curHealth;
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
		public int baseHealth;
		public int curHealth;
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
			Debug.Log(enemy.curHealth);
		}
	}
	
	public void InitSkill()
	{
	
		//----Initialization for All Skills----
		attack1.name = "Attack1";
		attack1.myPlayer = player;
		// TODO: Damage Caculation
		attack1.damage = (attack1.myPlayer.ATK - attack1.targetEnemy.DEF) + Random.Range(-20, 20);
		attack2.name = "Attack2";
		defend.name = "Defend";
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
	
		if (!hasInit)
		{
			InitAll();
			
		}
	}
	
	void InitAll()
	{

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
			
			
			int allEnemyCurHealth = 0;
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
		
		
				comicInstances = GameObject.FindGameObjectsWithTag("Comic");
		if (comicInstances.Length > 0)
		{
			Debug.Log(comicInstances.Length);
			if (!stopForASecond)
			{
				foreach (GameObject go in comicInstances)
				{
					go.transform.Translate(Vector3.up * .01f);
					
					if (go.transform.localPosition.y > 300)
						Destroy(go);	
				}
	
				if (comicInstances[comicInstances.Length - 1].transform.localPosition.y > 100)
				{
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
		
		if (newComicComing)
		{
			
			
			switch(comicPlay)
			{
			case ComicPlay.playerAttack:
				PlayerBorn();
				break;
			case ComicPlay.enemyAttack:
				EnemyBorn();
				break;
			}
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
				battleHasStarted = true;	
				// TODO: 将它移到更合适的地方
				hasLevelUp = false;
				dropsDownJudge = Random.Range(1, 10);
			}
		}
		
		
		if (battleHasStarted)
		{
			

			InitSkill();
			GUI.Label(new Rect(0, 375, 500, 50),"You encountered " + enemies.Length + " enemies.");
			
			
			if (!battleEvaluation)
			{
				continuoursFighting = false;
				if (playersTurn)
				{
					GUI.Label(new Rect(0, 350, 500, 50),"This is player's turn");
					
					
					if (checkForOnce)
					{	
						
						
						actualActionTimes = 0;
						braveTimes = 0;
						switch(comicPlay)
						{
						case ComicPlay.playerAttack:
							PlayerBorn();
							break;
						case ComicPlay.enemyAttack:
							EnemyBorn();
							break;
						}
						checkForOnce = false;
					}
		
					
					//----Option Brave----
					//if (GUI.Button(new Rect(0, 0, 75, 25), "Brave"))
					if (onBrave)
					{
						onBrave = false;
						if (BPValue > -3)
						{
							BPValue--;
							braveTimes++;
							
							comicPlay = ComicPlay.playerAttack;
							stopForASecond = false;
							everyTurn = false;
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
							//onStatus = false;

							//if (GUI.Button(new Rect(75, 50, 75, 25), "Attacker"))
							if (onAttacker)
							{
								onAttacker = false;
								
								player.status = Player.STATUS.ATTACKER;

								CheckStatusSkill();
								
								checkForAvailableSkills = true;
							}
							//if (GUI.Button(new Rect(150, 50, 75, 25), "Defender"))
							if (onDefender)
							{

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
								case "Attack1":
									
									
									
									attack1.proficiency++;
									chooseEnemy = true;
									//if (choosedOne)
									{
										choosedOne.curHealth -= attack1.damage;
									}
									//GUI.Button(new Rect(75, 100, 75, 25), "Enemy");
										
									// 伤害公式
									//int temp = (player.ATK - enemy.DEF) + Random.Range(-20, 20);
									//enemy.curHealth -= attack1.damage;
									//checkForAvailableSkills = false;
										
									
									GUI.Label(new Rect(0, 175, 300, 25),"Player Attack!" + attack1.damage);
									break;
								case "Attack2":
											chooseEnemy = true;
										attack2.proficiency++;
									break;		
								case "Defend":
										defend.proficiency++;
									Debug.Log("Defend");
									GUI.Label(new Rect(0, 175, 300, 25),"Player Defend!");
										
									actualActionTimes++;
									break;
								case "Heal":
										heal.proficiency++;
									Debug.Log("Heal");
									GUI.Label(new Rect(0, 175, 300, 25),"Player Heal 30");
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
									Debug.Log("Attack");
									actualActionTimes++;
									//attack1.targetEnemy = enemies[j];
									//attack1.DamageCaculation(enemies[j]);
									enemies[0].curHealth -= Random.Range(800, 900);
									
									comicPlay = ComicPlay.playerAttack;
									stopForASecond = false;
									everyTurn = false;
									
									
											
								}
							}
						}
						
						//----Option Auto----
					
						

					


						
					}
					if (actualActionTimes == availableActionTimes)
					{
	
							turnCount++;
	
					}
					
				
					
				}
				else 
				{
					//----Enemy's Turn----
					// TODO: enemy's turn caculation
					if (isEnemysTurn)
					{
						comicPlay = ComicPlay.enemyAttack;
						stopForASecond = false;
						everyTurn = false;
				
					}
					
					if (GUI.Button(new Rect(0, 75, 75, 25),"End Turn"))
					
					{
						
						// 我方伤害公式
						
						for(int i = 0; i < enemies.Length; i++)
						{
						int temp = enemies[i].ATK - player.DEF + Random.Range(-5, 5);
						GUI.Label(new Rect(0, 175, 300, 25),"Enemy Attack:" + temp);
						player.curHealth -= temp;
						}
						turnCount++;
						checkForOnce = true;
							// 判断下一回合玩家可否行动
						if (BPValue + 1 <= 0)
						{
								// TODO:暂停并显示标志
								Debug.Log("玩家跳过行动");
								GUI.Label(new Rect(0, 175, 300, 25),"Player passed this turn!");
								turnCount++;
								BPValue++;
								actualActionTimes = 0;
								braveTimes = 0;
						}
							
						
					}
					GUI.Label(new Rect(0, 350, 500, 50),"This is enemy's turn");
				}
			
				
				//----Show Datas----
				GUI.Label(new Rect(0, 275, 100, 25),"BP:" + BPValue);
				GUI.Label(new Rect(0, 250, 500, 25),"AvailableActionTimes:" + availableActionTimes);
				GUI.Label(new Rect(0, 300, 100, 25),"Turn Number:" + turnCount);
				GUI.Label(new Rect(0, 225, 300, 25),"ActionTimes:" + actualActionTimes);
				GUI.Label(new Rect(0, 200, 300, 25),"BraveTimes:" + braveTimes);
				
				GUI.Label(new Rect(400, 0, 300, 25),"Player HP:" + player.curHealth + "/" + player.baseHealth);
				GUI.Label(new Rect(400, 25, 100, 25),"Player Level:" + player.level);
				GUI.Label(new Rect(400, 50, 100, 25),"Player exp:" + player.exp);
				for (int i = 0; i < enemies.Length; i++)
				{
					GUI.Label(new Rect(400, 75 + 25 * i, 300, 25),"Enemy HP:" + enemies[i].curHealth + "/" + enemies[i].baseHealth);
				}
				
			}
			
			else
			{
				//----Battle Evaluation----
				GUI.Label(new Rect(0, 0, 600, 25),"Battle Evaluation");
				
				
				//----Clear the Screen----
				if (comicInstances.Length > 0)
				{
					foreach (GameObject go in comicInstances)
					{	
						Destroy(go);	
					}
				}
				
				//----Situation when You Win----
				if (battleWin)
				{
					//----What You Get from this Battle----
					int totalExp = 0;
					List<string> drops = new List<string>();

					for (int i = 0; i < enemies.Length; i++)
					{
						totalExp += enemies[i].EXP;
						// TODO: 增加获得经验值动画（数值增加）
						
						if ( dropsDownJudge < 6)
						{
							drops.Add(enemies[i].drops[0]);
							//GUI.Label(new Rect(0, 75, 600, 25),"You got " + enemies[i].drops[0]);
						}
						else {drops.Add(enemies[i].drops[1]);}
					}
					
					for (int i = 0; i < drops.Count; i++)
					{
						GUI.Label(new Rect(0, 50 - 25 * i, 600, 25),"You got " + drops[i]);
					}
					GUI.Label(new Rect(0, 75, 600, 25),"You Win! You got " + totalExp + " points of experience!");
					GUI.Label(new Rect(0, 100, 600, 25), "Turn Count:" + turnCount.ToString());
					GUI.Label(new Rect(0, 125, 600, 25), "Damage:" + (player.baseHealth - player.curHealth).ToString());
					

					// TODO: 加入特殊成就达成评价
					// TODO: 加入战斗评价计算公式，现在的太糙了
					
					//----True Battle Evaluation----
					if ( turnCount < 3 )
					{
						GUI.Label(new Rect(0, 150, 600, 25), "S");
					}
					else if ( 3 <  turnCount && turnCount < 7 )
					{
						GUI.Label(new Rect(0, 150, 600, 25), "A");
					}
					else if ( 7 < turnCount && turnCount < 12)
					{
						GUI.Label(new Rect(0, 150, 600, 25), "B");
					}
					else if ( 12 < turnCount && turnCount < 15)
					{
						GUI.Label(new Rect(0, 150, 600, 25), "C");
					}
					else if ( 15 < turnCount && turnCount < 20)
					{
						GUI.Label(new Rect(0, 150, 600, 25), "D");
					}
					else {GUI.Label(new Rect(0, 150, 600, 25), "E");}
					
					GUI.Label(new Rect(0, 175, 600, 25), "Skilled increase");
					
					if (GUI.Button(new Rect(0, 325, 150, 25), "Continue"))
					{
						continuoursFighting = true;
						battleHasStarted = false;
					}
				
					if (GUI.Button(new Rect(0, 350, 150, 25), "End the Battle"))
					{
						for (int i = 0; i < enemies.Length; i++)
						{
							player.exp += enemies[i].EXP;
						}
						battleHasStarted = false;
					}
					
				}
				//----Situation When You Lose----
				else 
				{
					
					GUI.Label(new Rect(0, 50, 300, 25),"You lose!");
					
					if (GUI.Button(new Rect(0, 350, 150, 25), "End the Battle"))
					{
						battleHasStarted = false;
					}
				
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
	
	
	void PlayerBorn ()
	{
		playerInstance = (GameObject)Instantiate(PrePlayerBattleComic, new Vector3(0, 0, 0), transform.rotation);
		playerInstance.transform.parent = GameObject.Find("Comic").transform;
		playerInstance.transform.localPosition = new Vector3(0, 0, 0);
		playerInstance.transform.localScale = new Vector3(200, 100, 0); 
		
	}
	
	void EnemyBorn ()
	{
		enemyInstance = (GameObject)Instantiate(PreEnemyBattleComic, new Vector3(0, 0, 0), transform.rotation);
		enemyInstance.transform.parent = GameObject.Find("Comic").transform;
		enemyInstance.transform.localPosition = new Vector3(200, 0, 0);
		enemyInstance.transform.localScale = new Vector3(200, 100, 0); 
	}

	void OnStatus ()
	{
		onStatus = true;
		
	}
	
	void OnAttacker ()
	{
		onAttacker = true;
	}
	
	void OnDefender ()
	{
		onDefender = true;
	}
	
	void OnBack ()
	{
		onBack = true;
	}
	
	void OnChooseEnemy ()
	{
		//onChooseEnemy = true;
	}
	
	void OnBrave ()
	{
		onBrave = true;
	}
	
	void OnDefault ()
	{
		onDefault = true;
	}

}