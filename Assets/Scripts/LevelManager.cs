using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public float levelDuration=120;
	public float elapsedLevelTime=0;
	public int createdGames=0;
	
	public static LevelManager instance;
	
	void Start () {
		instance=this;
	}
	
	void Update () {
		elapsedLevelTime+=Time.deltaTime;
		if (elapsedLevelTime>levelDuration) {
			LoadNextLevel();
		}
	}
	
	void LoadNextLevel() {
		levelDuration*=1.5f;
		elapsedLevelTime=0;
		Application.LoadLevel(Application.loadedLevel+1);
	}
}
