using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleComicShow : MonoBehaviour {
	
	//public UISprite PlayerBattleComic;
	//public UISprite EnemyBattleComic;
	
	public GameObject PrePlayerBattleComic;
	public GameObject PreEnemyBattleComic;
	
	
	GameObject playerInstance;
	GameObject enemyInstance;
	
	GameObject[] comicInstances;
	
	bool newComicComing = true;

	
	bool stopForASecond = false;
	
	bool everyTurn = true;
	bool isEnemysTurn;
	bool battleStart = false;
	int i = 1;
	enum ComicPlay 
	{
		playerAttack,
		enemyAttack
	}
	
	ComicPlay comicPlay;
	// Use this for initialization
	void Start () {
		
		comicPlay = ComicPlay.playerAttack;		
			
			
		
	}
	
	
	// Update is called once per frame
	void Update () {
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
		if (stopForASecond)
		{
			if (Input.GetKeyUp(KeyCode.Q))
			{
				comicPlay = ComicPlay.playerAttack;
				stopForASecond = false;
				everyTurn = false;
				//newComicComing = true;
				
			}
			if (Input.GetKeyUp(KeyCode.W))
			{
				comicPlay = ComicPlay.enemyAttack;
				stopForASecond = false;
				everyTurn = false;
				//newComicComing = true;
			}
			if (isEnemysTurn)
			{
				comicPlay = ComicPlay.enemyAttack;
				stopForASecond = false;
				everyTurn = false;
				
			}
		}
		
		//if (battleStart)
		{
			if (Input.GetKeyDown(KeyCode.Space))
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
			}
		}
		
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
