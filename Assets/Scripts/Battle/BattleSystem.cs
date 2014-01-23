using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour {
	
	// ----Battle Comic Show------
	public GameObject PrePlayerBattleComic;
	public GameObject PreEnemyBattleComic;
	
	
	GameObject playerInstance;
	GameObject enemyInstance;
	
	GameObject[] comicInstances;
	
	bool newComicComing = false;
	
	
	bool stopForASecond = false;
	
	enum ComicPlay 
	{
		playerAttack,
		enemyAttack
	}
	
	ComicPlay comicPlay;
	//----------------------------
	// TODO: 使用BP值进行身份转换
	// TODO: 加入玩家类与敌人类，及其基本技能
	// TODO: 加入敌人基本AI
	// TODO: 加入NGUI与美术
	// TODO: 原型测试与讨论修改
	// TODO: 战斗评价
	// TODO: 熟练度
	// TODO: 升级系统
	int turnCount;					// 回合数
	int availableActionTimes;		// 可行动数
	int actualActionTimes;			// 实际行动累积数
	int braveTimes;					// 勇气数
	int BPValue;					// BP值
	
	int dropsDown;
	
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
	
	
	bool onStatus = false;
	bool onBrave = false;
	bool onDefault = false;
	bool onStatusAttacker = false;
	bool onStatusDefender = false;
	bool onStatusHealer = false;
	bool onSkill = false;
	public static bool onUseSkill = false;
	
	bool forOnce = true;
	bool deletedSkillButton = false;
	Skill[] skillWeHaveNow;
		//public List<Skill> skillCanBeUsed = new List<Skill>();
	List<Skill> skillCanBeUsed;
		
	Skill attack1;
	Skill attack2;
	Skill defend;
	Skill heal;
	List<GameObject> buttons = new List<GameObject>();
	
	
	struct Player
	{
		public int baseHealth;
		public int curHealth;
		public int level;
		public int exp;
		public int ATK;
		public int DEF;

		
		// TODO: 改进为数组，并且设置增加、减少等方法
		public string availableSkill;
		

		
		
		
		public int ExpForNextLevel(int lv)
		{
			// TODO: 研究经验值与等级计算公式
			//ExpForNextLevel(0) = 64;
			
			return lv * lv * lv+ 3 * lv * lv + 9 * lv + 9;
			
		}
		
		public enum STATUS 
		{
			ATTACKER,
			BLASTER,
			DEFENDER,
			HEALER,
			ENHANGCE,
			JAMMER
			
		}
		
		public STATUS status;
		
		
	}
	
	struct Enemy
	{
		public string name;
		public int baseHealth;
		public int curHealth;
		public int ATK;
		public int DEF;
		public int EXP;
		public string[] drops;
		public float[] dropsRate;
	}
	
	struct Item
	{
		
	}
	
	struct Skill {
		public string name;
		public bool exist;
		public bool available;
		public int curLevel;
		public int maxLevel;
		public int skilledIncrease;
		// TODO: 技能升级
		public int damage;
		public Enemy targetEnemy;
		public Player myPlayer;
		
		public void DamageCaculation (Enemy enemy)
		{
			
			
			enemy.curHealth -= 20;
			Debug.Log(enemy.curHealth);
		}
	}
	
	public void InitSkill()
	{
		skillCanBeUsed = new List<Skill>();
		attack1.name = "Attack1";
		attack1.myPlayer = player;
//		attack1.targetEnemy = enemies[0];
		attack1.damage = (attack1.myPlayer.ATK - attack1.targetEnemy.DEF) + Random.Range(-20, 20);
		attack2.name = "Attack2";
		defend.name = "Defend";
		heal.name = "Heal";
		skillWeHaveNow = new Skill[4]{attack1, attack2, defend, heal};
			
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
	
	Player player;
	Enemy[] enemies;
	Enemy choosedOne = new Enemy();
	// Use this for initialization
	void Start () {
	    comicPlay = ComicPlay.playerAttack;
		
		InitAll();

	}
	
	void InitAll()
	{
		//PlayerBattleComic
		player.baseHealth = 500;
		player.curHealth = player.baseHealth;
		player.level = 1;
		player.exp = 0;
		player.status = Player.STATUS.ATTACKER;
		player.ATK = 300;
		player.DEF = 20;
		
		int numberOfEnemy = Random.Range(1,4);
		enemies = new Enemy[numberOfEnemy];
		
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].name = "Enemy" + i.ToString();
			enemies[i].baseHealth = 1000;
			enemies[i].curHealth = enemies[i].baseHealth;
			enemies[i].ATK = 60;
			enemies[i].DEF = 20;
			enemies[i].EXP = 10;
			enemies[i].drops = new string[2]{"Item1", "Item2"};
			enemies[i].dropsRate = new float[2]{0.2f, 0.8f};
		}
		
		
		turnCount = 1;					// 回合数
		availableActionTimes = 0;		// 可行动数
		actualActionTimes = 0;			// 实际行动累积数
		braveTimes = 0;					// 勇气数
		BPValue = 0;					// BP值
		
	}
	
	void ResetAll()
	{
		
		player.baseHealth = 500;
		player.curHealth = player.baseHealth;
		player.status = Player.STATUS.ATTACKER;
		player.ATK = 300;
		player.DEF = 20;
		
		
		int numberOfEnemy = Random.Range(1,4);
		enemies = new Enemy[numberOfEnemy];
		
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].name = "Enemy" + i.ToString();
			enemies[i].baseHealth = 1000;
			enemies[i].curHealth = enemies[i].baseHealth;
			enemies[i].ATK = 60;
			enemies[i].DEF = 20;
			enemies[i].EXP = 10;
			enemies[i].drops = new string[2]{"Item1", "Item2"};
			enemies[i].dropsRate = new float[2]{0.2f, 0.8f};
		}
		
		turnCount = 1;					// 回合数
		availableActionTimes = 0;		// 可行动数
		actualActionTimes = 0;			// 实际行动累积数
		braveTimes = 0;					// 勇气数
		BPValue = 0;					// BP值
		battleEvaluation = false;
		
		
	}
	
	void ResetContinuousFighting()
	{
		int numberOfEnemy = Random.Range(1,4);
		enemies = new Enemy[numberOfEnemy];
		
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].name = "Enemy" + i.ToString();
			enemies[i].baseHealth = 1000;
			enemies[i].curHealth = enemies[i].baseHealth;
			enemies[i].ATK = 60;
			enemies[i].DEF = 20;
			enemies[i].EXP = 10;
			enemies[i].drops = new string[2]{"Item1", "Item2"};
			enemies[i].dropsRate = new float[2]{0.2f, 0.8f};
		}
		
		turnCount = 1;					// 回合数
		availableActionTimes = 0;		// 可行动数
		actualActionTimes = 0;			// 实际行动累积数
		braveTimes = 0;					// 勇气数
		BPValue = 0;					// BP值
		battleEvaluation = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (battleHasStarted)
		{
			if (turnCount % 2 == 1)
			{
				playersTurn = true;
				availableActionTimes = braveTimes + 1;
			}
			else 
			{
				playersTurn = false;
			}
			
			for (int i = 0; i < enemies.Length; i++)
			{
			if (enemies[i].curHealth <= 0)
			{
				
				battleWin = true;
				//Debug.Log("You Win");
				//battleHasStarted = false;
				battleEvaluation = true;
				
				
			}
				else {battleWin = true;}
			}
			if (player.curHealth <= 0)
			{
				battleWin = false;
				battleEvaluation = true;
				//battleHasStarted = false;
				
			}
		}
		else 
		{
			battleWin = false;
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
				}
			}
				
		}
		
		if (Input.GetKeyDown(KeyCode.Space) || newComicComing)
		{
			newComicComing = false;
			if (Random.Range(0,2) < 1)
			{
				PlayerBorn();
			}
			else { EnemyBorn(); }

		}
		
		if (Input.GetKeyDown(KeyCode.A))
		{
			if (!stopForASecond)
			{
				stopForASecond = true;
			}
			else { stopForASecond = false; }
		}
		//if (!hasLevelUp)
		//{
			

		//}
	}
	
	void OnGUI()
	{
		if (!battleHasStarted)
		{
			//battleWin = false;
			if (GUI.Button(new Rect(0, 0, 100, 25), "Battle Begin"))
			{
				battleHasStarted = true;	
						// TODO: 将它移到更合适的地方
				hasLevelUp = false;
				dropsDown = Random.Range(1, 10);
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
						
						checkForOnce = false;
					}
		
					if (onBrave)
					{
						stopForASecond = false;
						onBrave = false;
						if (BPValue > -3)
						{
							BPValue--;
							braveTimes++;
						}
					
					}
					
					else if (onDefault)
					{
						stopForASecond = false;
						onDefault = false;
						if ( BPValue < 3 )
						{
							BPValue++;
							turnCount++;
							
							// TODO: 增加防御效果
						}
					}
					
					
					if (actualActionTimes < availableActionTimes)
					{
						if (onStatus)
						{
							onStatus = false;
							Debug.Log("Status Change.");
							//actualActionTimes++;
							// TODO: 按照行动所需的BP值扣除
							if (changeStatus)
							{
								changeStatus = false;
							}
							else 
							{
								changeStatus = true;
							}
							
						}
						
						if (onSkill)
						{
							onSkill = false;
							//actualActionTimes++;
							//enemy.curHealth -= 500;
							// TODO: 计算伤害公式
							// TODO: 攻击对象与敌人群组
							Debug.Log("Check for available skills");
							
							
							if (checkForAvailableSkills)
							{
								checkForAvailableSkills = false;
							}
							else { checkForAvailableSkills = true; }
							
							
						}
						
						if (GUI.Button(new Rect(0, 100, 75, 25), "Auto"))
						{
							if (player.status == Player.STATUS.ATTACKER)
							{
								
							}
						}
						
						if (changeStatus)
						{
							if (onStatusAttacker)
							{
								onStatusAttacker = false;
								forOnce = true;
															
									foreach( GameObject go in buttons)
									{
										Destroy(go);
									}
								Debug.Log("Attacker");
								player.status = Player.STATUS.ATTACKER;
								//player.skillCanBeUsed.Clear();
								CheckStatusSkill();
							}
							if (onStatusDefender)
							{
								onStatusDefender = false;
							//	Debug.Log("Defender");
								forOnce = true;
															
									foreach( GameObject go in buttons)
									{
										Destroy(go);
									}
								
								player.status = Player.STATUS.DEFENDER;
								
								//player.skillCanBeUsed.Clear();
								CheckStatusSkill();
							}
							if (onStatusHealer)
							{
								onStatusHealer = false;
								forOnce = true;
															
									foreach( GameObject go in buttons)
									{
										Destroy(go);
									}
							//	Debug.Log("Healer");
								player.status = Player.STATUS.HEALER;
								//player.skillCanBeUsed.Clear();
								CheckStatusSkill();
							}
						}
					
						
						if (chooseEnemy)
						{
							for (int j = 0; j < enemies.Length; j++)
							{
											
								if (GUI.Button(new Rect(75 + j * 75, 100, 75, 25), enemies[j].name))
								{
									Debug.Log("Attack");
									actualActionTimes++;
									attack1.targetEnemy = enemies[j];
									attack1.DamageCaculation(enemies[j]);
									enemies[j].curHealth -= Random.Range(200, 300);
											
								}
							}
						}
						if (onUseSkill)
						{
							stopForASecond = false;
							
							{
								onUseSkill = false;
								Debug.Log("Use Skill");
								//actualActionTimes++;
								//enemy.curHealth -= 500;
								switch (SkillButton.skillName)
								{
								case "Attack1":
									
									Debug.Log("Attack1");
									attack1.skilledIncrease++;
									chooseEnemy = true;
									comicPlay = ComicPlay.playerAttack;
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
											Debug.Log("Attack2");
									comicPlay = ComicPlay.playerAttack;
										attack2.skilledIncrease++;
									break;		
								case "Defend":
											Debug.Log("Defend");
									comicPlay = ComicPlay.playerAttack;
										defend.skilledIncrease++;
									Debug.Log("Defend");
									GUI.Label(new Rect(0, 175, 300, 25),"Player Defend!");
										
									actualActionTimes++;
									break;
								case "Heal":
										heal.skilledIncrease++;
									comicPlay = ComicPlay.playerAttack;
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
						
						if (checkForAvailableSkills)
						{
							
							if (forOnce)
							{
								forOnce = false;
							
								
							for (int i = 0; i < skillCanBeUsed.Count; i++)
							{
									
 									GameObject a;
									
									a  =(GameObject) Instantiate(Resources.Load("SkillButton"));  
         							a.transform.parent = GameObject.Find("SkillTween").transform;  
          							//  NGUITools.AddChild(GameObject.Find("Panel"), GameObject.Find("SkillButton"));
									a.transform.localPosition = new Vector3(159, - i * 40 - 40, 0);
									a.transform.localScale = new Vector3(1, 1, 1);
									a.GetComponentInChildren<UILabel>().text = skillCanBeUsed[i].name;
									a.GetComponent<UIButtonMessage>().target = GameObject.Find("BattleSystemController");
									buttons.Add(a);
			
								
							
							}
							}
							
								
						}
						else
						{
							
						}
						
					}
					if (actualActionTimes == availableActionTimes)
					{
	
							turnCount++;
	
					}
					
	
		
				}
				else 
				{
					
					
					if (GUI.Button(new Rect(0, 75, 75, 25),"End Turn"))
					{
						// 我方伤害公式
						comicPlay = ComicPlay.enemyAttack;
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
				GUI.Label(new Rect(0, 0, 600, 25),"Battle Evaluation");
				
				if (battleWin)
				{
					for (int i = 0; i < enemies.Length; i++)
					{
						GUI.Label(new Rect(0, 50, 600, 25),"You Win! You got " + enemies[i].EXP + " points of experience!");
						// TODO: 增加获得经验值动画（数值增加）
						
						if ( dropsDown < 6)
						{
							GUI.Label(new Rect(0, 75, 600, 25),"You got " + enemies[i].drops[0]);
						}
						else {GUI.Label(new Rect(0, 75, 600, 25),"You got " + enemies[i].drops[1]);}
					}
					GUI.Label(new Rect(0, 100, 600, 25), "Turn Count:" + turnCount.ToString());
					GUI.Label(new Rect(0, 125, 600, 25), "Damage:" + (player.baseHealth - player.curHealth).ToString());
					
					// 基本战斗评价
					// TODO: 加入特殊成就达成评价
					// TODO: 加入战斗评价计算公式，现在的太糙了
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
					
					
				}
				else {GUI.Label(new Rect(0, 50, 300, 25),"You lose!");}
				
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
			
			
			
		}
		
			if (player.exp >= player.ExpForNextLevel(player.level))
			{
				Debug.Log(player.exp + "/" + player.ExpForNextLevel(player.level));
				if (!hasLevelUp)
				{
					hasLevelUp = true;
					player.level++;
				
					// TODO: 以下写能力成长公式
				}
			}

			

		

		
	
	}
	
	void EndTurn()
	{
		turnCount++;
		BPValue--;
		//BPValue = curBPValue;
	}
	
	void OnStatus ()
	{
		onStatus = true;
	}
	void OnBrave ()
	{
		onBrave = true;
	}
	
	void OnDefault ()
	{
		onDefault = true;
	}
	void OnStatusAttacker ()
	{
		onStatusAttacker = true;
	}
	void OnStatusDefender ()
	{
		onStatusDefender = true;
	}
	void OnStatusHealer ()
	{
		onStatusHealer = true;
	}
	void OnSkill ()
	{
		
//		GameObject.Find("Skill1Name").GetComponent<UILabel>().text = skillCanBeUsed[0].name;
		onSkill = true;
	}
	void OnUseSkill ()
	{
		onUseSkill = true;
		Debug.Log("So what");
	}
	
	void PlayerBorn ()
	{
		playerInstance = (GameObject)Instantiate(PrePlayerBattleComic, new Vector3(0, 0, 0), transform.rotation);
		playerInstance.transform.parent = GameObject.Find("Panel").transform;
		playerInstance.transform.localPosition = new Vector3(0, 0, 0);
		playerInstance.transform.localScale = new Vector3(200, 100, 0); 
		
	}
	
	void EnemyBorn ()
	{
		enemyInstance = (GameObject)Instantiate(PreEnemyBattleComic, new Vector3(0, 0, 0), transform.rotation);
		enemyInstance.transform.parent = GameObject.Find("Panel").transform;
		enemyInstance.transform.localPosition = new Vector3(200, 0, 0);
		enemyInstance.transform.localScale = new Vector3(200, 100, 0); 
	}
}