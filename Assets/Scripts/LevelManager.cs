#pragma warning disable 0618
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public float levelDuration=120;
	public float elapsedLevelTime=0;
	public int requiredGames=0;
	public int createdGames=0;
	
	public static LevelManager instance;
	
	void Start () {
		instance=this;
		DontDestroyOnLoad(gameObject);
	}
	
	void Update () {
		elapsedLevelTime+=Time.deltaTime;
		if (elapsedLevelTime>levelDuration && createdGames>requiredGames) {
			LoadNextLevel();
		} else {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	
	void LoadNextLevel() {
		levelDuration*=1.2f;
		requiredGames+=3;
		elapsedLevelTime=0;
		Application.LoadLevel(Application.loadedLevel+1);
	}
}
