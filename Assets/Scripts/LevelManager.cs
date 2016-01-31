using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public float levelDuration=120;
	
	public float elapsedLevelTime=0;
	
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
		Application.LoadLevel(Application.loadedLevel+1);
	}
}
