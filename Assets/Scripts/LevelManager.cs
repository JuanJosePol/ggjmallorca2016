﻿#pragma warning disable 0618
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public float levelDuration=120;
	public float elapsedLevelTime=0;
	public int requiredGames=0;
	public int createdGames=0;
	
	public int currentLevel=1;
	
	public static LevelManager instance;
	
	
	void Start () {
		
		
		if (instance==null) {
			instance=this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
		
	}
	
	void Update () {
		elapsedLevelTime+=Time.deltaTime;
		if (elapsedLevelTime>levelDuration) {
			if (createdGames>=requiredGames) {
				LoadNextLevel();
			} else {
				UIManager.instance.notificationText.text="You suck...\nTry again!";
				Invoke("ReloadLevel", 3);
			}
		}
		
		if (Input.GetKeyDown(KeyCode.PageDown)) {
			LoadNextLevel();
		}
	}
	
	void ReloadLevel() {
		Application.LoadLevel(Application.loadedLevel);
		elapsedLevelTime=0;
		createdGames=0;
		UIManager.instance.notificationText.text="";
	}
	
	void LoadNextLevel() {
		elapsedLevelTime=0;
		createdGames=0;
		StartCoroutine(LoadingNextLevel());
	}
	
	IEnumerator LoadingNextLevel() {
		UIManager.instance.notificationText.text="A winner is you!";
		
		StartCoroutine(GameManager.instance.FadeOut());
		
		AssetCatalog.instance.PlaySound("levelfinished");
		
		yield return new WaitForSeconds(5);
		UIManager.instance.notificationText.text="";
		
		currentLevel++;
		levelDuration*=1.2f;
		requiredGames+=3;
		elapsedLevelTime=0;
		Application.LoadLevel(Application.loadedLevel+1);
		yield return new WaitForSeconds(0.1f);
	}
	
}
